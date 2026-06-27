using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Credentials.Options;
using Fgs.Notification.Application.Notifications.Templates;
using Fgs.Notification.Domain.Entities;
using Microsoft.Extensions.Options;

namespace Fgs.Notification.Infrastructure.Notifications.Templates;

public sealed class CommunicationTemplateRepository(
    ISetupClient setupClient,
    IOptions<CredentialDistributionOptions> distributionOptions) : ICommunicationTemplateRepository
{
    public async Task<FgsSetupCommunicationTemplate?> GetActiveTemplateAsync(
        long? tenantId,
        long? companyId,
        string templateType,
        string code,
        CancellationToken cancellationToken = default)
    {
        var response = await setupClient.GetActiveTemplateAsync(
            tenantId,
            companyId,
            templateType.Trim(),
            code.Trim(),
            distributionOptions.Value.InternalServiceKey,
            cancellationToken);

        if (!response.Success || response.Data is null)
        {
            return null;
        }

        return Map(response.Data);
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
