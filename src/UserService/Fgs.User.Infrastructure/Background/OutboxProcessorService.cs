using Fgs.User.Application.Abstractions.Messaging;
using Fgs.User.Application.IntegrationEvents;
using Fgs.User.Domain.Enums;
using Fgs.User.Infrastructure.Common.Options;
using Fgs.User.Infrastructure.Persistence.Database.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fgs.User.Infrastructure.Background;

public sealed class OutboxProcessorService(
    IServiceScopeFactory scopeFactory,
    IOptions<OutboxOptions> options,
    IOptions<RabbitMqOptions> rabbitOptions,
    ILogger<OutboxProcessorService> logger) : BackgroundService
{
    private readonly OutboxOptions _options = options.Value;
    private readonly RabbitMqOptions _rabbitOptions = rabbitOptions.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Outbox processor started (poll every {Seconds}s)", _options.PollIntervalSeconds);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessBatchAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Outbox batch processing failed");
            }

            await Task.Delay(TimeSpan.FromSeconds(_options.PollIntervalSeconds), stoppingToken);
        }
    }

    private async Task ProcessBatchAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<FgsUserDbContext>();
        var publisher = scope.ServiceProvider.GetRequiredService<IRabbitMqPublisher>();
        var now = DateTimeOffset.UtcNow;

        var messages = await context.GloOutboxMessages
            .Where(m => (m.Status == OutboxMessageStatus.Pending || m.Status == OutboxMessageStatus.Retry)
                && (m.NextRetryOn == null || m.NextRetryOn <= now)
                && m.RetryCount < m.MaxRetryCount)
            .OrderBy(m => m.CreatedOn)
            .Take(_options.BatchSize)
            .ToListAsync(cancellationToken);

        foreach (var message in messages)
        {
            message.Status = OutboxMessageStatus.Processing;
        }

        if (messages.Count > 0)
        {
            await context.SaveChangesAsync(cancellationToken);
        }

        foreach (var message in messages)
        {
            try
            {
                var routingKey = !string.IsNullOrWhiteSpace(message.RoutingKey)
                    ? message.RoutingKey
                    : IntegrationEventRoutingKeys.ForEventType(
                        message.EventType,
                        _rabbitOptions.RoutingKeyPrefix);
                await publisher.PublishAsync(
                    routingKey,
                    message.Payload,
                    message.CorrelationId.ToString(),
                    cancellationToken);

                message.Status = OutboxMessageStatus.Published;
                message.ProcessedOn = now;
                message.LastError = null;
                message.NextRetryOn = null;
            }
            catch (Exception ex)
            {
                message.RetryCount++;
                if (message.RetryCount >= message.MaxRetryCount)
                {
                    message.Status = OutboxMessageStatus.Failed;
                    message.NextRetryOn = null;
                }
                else
                {
                    message.Status = OutboxMessageStatus.Retry;
                    message.NextRetryOn = now.AddSeconds(Math.Pow(2, message.RetryCount));
                }

                message.LastError = ex.Message;
                logger.LogWarning(ex, "Failed to publish outbox message {MessageId}", message.Id);
            }

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
