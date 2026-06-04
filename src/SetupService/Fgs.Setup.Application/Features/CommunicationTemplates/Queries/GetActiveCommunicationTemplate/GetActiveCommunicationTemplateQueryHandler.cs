using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Common.Options;
using Fgs.Setup.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Options;

namespace Fgs.Setup.Application.Features.CommunicationTemplates.Queries.GetActiveCommunicationTemplate;

public sealed class GetActiveCommunicationTemplateQueryHandler(
    IUnitOfWork unitOfWork,
    IOptions<CredentialDistributionOptions> distributionOptions)
    : IRequestHandler<GetActiveCommunicationTemplateQuery, ApiResponse<CommunicationTemplateDto>>
{
    public async Task<ApiResponse<CommunicationTemplateDto>> Handle(
        GetActiveCommunicationTemplateQuery request,
        CancellationToken cancellationToken)
    {
        if (!IsInternalServiceAuthorized(request.InternalServiceKey, distributionOptions.Value))
        {
            return ApiResponse<CommunicationTemplateDto>.Fail(
                ["Unauthorized."],
                ApiStatusCodes.Unauthorized);
        }

        var normalizedType = request.TemplateType.Trim();
        var normalizedCode = request.Code.Trim();

        var templates = await unitOfWork.Repository<FgsSetupCommunicationTemplate>()
            .ListAsync(
                t => t.TemplateType == normalizedType
                     && t.Code == normalizedCode
                     && t.IsActive
                     && t.TenantId == request.TenantId
                     && t.CompanyId == request.CompanyId,
                cancellationToken);

        var template = templates.OrderByDescending(t => t.Id).FirstOrDefault();
        if (template is null)
        {
            return ApiResponse<CommunicationTemplateDto>.Fail(
                ["Template not found."],
                ApiStatusCodes.NotFound);
        }

        return ApiResponse<CommunicationTemplateDto>.Ok(new CommunicationTemplateDto(
            template.Id,
            template.TenantId,
            template.CompanyId,
            template.TemplateType,
            template.Code,
            template.Name,
            template.Subject,
            template.Body,
            template.IsMobileVisible,
            template.IsActive));
    }

    private static bool IsInternalServiceAuthorized(
        string? providedKey,
        CredentialDistributionOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.InternalServiceKey))
        {
            return false;
        }

        return string.Equals(providedKey, options.InternalServiceKey, StringComparison.Ordinal);
    }
}
