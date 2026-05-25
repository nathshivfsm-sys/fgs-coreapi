using Fgs.Platform.Application.Notifications.Templates;
using Fgs.Platform.Domain.Entities;
using Fgs.Platform.Domain.Notifications;

namespace Fgs.Platform.Infrastructure.Notifications.Templates;

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

        if (companyId.HasValue)
        {
            var companyScoped = await repository.GetActiveTemplateAsync(
                tenantId,
                companyId,
                templateType,
                templateCode,
                cancellationToken);

            if (companyScoped is not null)
            {
                return companyScoped;
            }
        }

        var tenantScoped = await repository.GetActiveTemplateAsync(
            tenantId,
            companyId: null,
            templateType,
            templateCode,
            cancellationToken);

        if (tenantScoped is not null)
        {
            return tenantScoped;
        }

        var global = await repository.GetActiveTemplateAsync(
            tenantId: null,
            companyId: null,
            templateType,
            templateCode,
            cancellationToken);

        if (global is not null)
        {
            return global;
        }

        throw new CommunicationTemplateNotFoundException(tenantId, companyId, templateCode, channel);
    }
}
