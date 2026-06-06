using Fgs.Notification.Application.Notifications.Templates;
using Fgs.Notification.Domain.Entities;
using Fgs.Notification.Domain.Notifications;

namespace Fgs.Notification.Infrastructure.Notifications.Templates;

public sealed class CommunicationTemplateService(ICommunicationTemplateRepository repository)
    : ICommunicationTemplateService
{
    public async Task<FgsSetupCommunicationTemplate> GetActiveTemplateAsync(
        long tenantId,
        long? companyId,
        NotificationChannel channel,
        string templateCode,
        CancellationToken cancellationToken = default)
    {
        var templateType = channel.ToCommunicationTemplateType();

        var template = await repository.GetActiveTemplateAsync(
            tenantId,
            companyId,
            templateType,
            templateCode,
            cancellationToken);

        return template
            ?? throw new CommunicationTemplateNotFoundException(tenantId, companyId, templateCode, channel);
    }
}
