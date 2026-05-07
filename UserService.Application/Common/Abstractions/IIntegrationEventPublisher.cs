using UserService.Domain.IntegrationEvents;

namespace UserService.Application.Common.Abstractions;

public interface IIntegrationEventPublisher
{
    Task PublishAdminUserInviteCreatedAsync(
        AdminUserInviteCreatedEvent integrationEvent,
        CancellationToken cancellationToken = default);
}
