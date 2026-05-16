using Fgs.User.Application.IntegrationEvents;
using Fgs.User.Domain.Enums;
using Fgs.User.Infrastructure.Database;
using Fgs.User.Infrastructure.Messaging;
using Fgs.User.Infrastructure.Options;
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

        var messages = await context.FgsOutboxMessages
            .Where(m => !m.IsDeleted
                //&& m.Status == OutboxMessageStatus.Pending
                && m.RetryCount < _options.MaxRetryCount)
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
                var routingKey = IntegrationEventRoutingKeys.ForEventType(
                    message.EventType,
                    _rabbitOptions.RoutingKeyPrefix);
                await publisher.PublishAsync(routingKey, message.Payload, message.CorrelationId, cancellationToken);

                message.Status = OutboxMessageStatus.Published;
                message.ProcessedOn = DateTimeOffset.UtcNow;
                message.LastError = null;
            }
            catch (Exception ex)
            {
                message.RetryCount++;
                message.Status = message.RetryCount >= _options.MaxRetryCount
                    ? OutboxMessageStatus.Failed
                    : OutboxMessageStatus.Pending;
                message.LastError = ex.Message;
                logger.LogWarning(ex, "Failed to publish outbox message {MessageId}", message.Id);
            }

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
