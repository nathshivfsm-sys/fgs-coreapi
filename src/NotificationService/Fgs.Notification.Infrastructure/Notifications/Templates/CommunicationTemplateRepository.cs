using Fgs.Contracts.Clients;
using Fgs.Notification.Application.Notifications.Templates;
using Fgs.Notification.Domain.Entities;
using Fgs.Notification.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace Fgs.Notification.Infrastructure.Notifications.Templates;

public sealed class CommunicationTemplateRepository(
    ISetupTemplateClient setupTemplateClient,
    IOptions<UserServiceCredentialClientOptions> clientOptions) : ICommunicationTemplateRepository
{
    public async Task<FgsSetupCommunicationTemplate?> GetActiveTemplateAsync(
        long? tenantId,
        long? companyId,
        string templateType,
        string code,
        CancellationToken cancellationToken = default)
    {
        var dto = await setupTemplateClient.GetActiveTemplateAsync(
            tenantId,
            companyId,
            templateType.Trim(),
            code.Trim(),
            clientOptions.Value.InternalServiceKey,
            cancellationToken);

        return dto is null ? null : Map(dto);
    }

    private static FgsSetupCommunicationTemplate Map(CommunicationTemplateDto dto) => new()
    {
        Id = dto.Id,
        TenantId = dto.TenantId,
        CompanyId = dto.CompanyId,
        TemplateType = dto.TemplateType,
        Code = dto.Code,
        Name = dto.Name,
        Subject = dto.Subject,
        Body = dto.Body,
        IsMobileVisible = dto.IsMobileVisible,
        IsActive = dto.IsActive
    };
}
