using Microsoft.Extensions.Logging;
using UserService.Application.Common.Abstractions;
using UserService.Domain.IntegrationEvents;

namespace UserService.Infrastructure.Events;

public sealed class NoOpIntegrationEventPublisher : IIntegrationEventPublisher
{
    private readonly ILogger<NoOpIntegrationEventPublisher> _logger;

    public NoOpIntegrationEventPublisher(ILogger<NoOpIntegrationEventPublisher> logger) => _logger = logger;

    public Task PublishAdminUserInviteCreatedAsync(
        AdminUserInviteCreatedEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        _logger.LogWarning(
            "Service Bus is not configured; skipping publish of {Event} for {Email}.",
            nameof(AdminUserInviteCreatedEvent),
            integrationEvent.Email);

        return Task.CompletedTask;
    }
}
