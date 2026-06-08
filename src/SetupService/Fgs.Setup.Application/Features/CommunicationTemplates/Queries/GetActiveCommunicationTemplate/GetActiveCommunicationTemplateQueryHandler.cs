using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Persistence.Abstractions;
using Fgs.Credentials.Options;
using Fgs.Setup.Application.Features.CommunicationTemplates;
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
        var normalizedType = request.TemplateType.Trim();
        var normalizedCode = request.Code.Trim();

        var templates = await unitOfWork.Repository<FgsSetupCommunicationTemplate>()
            .ListAsync(
                t => t.TemplateType == normalizedType
                     && t.Code == normalizedCode
                     && t.IsActive
                     && (
                         (request.CompanyId.HasValue
                          && t.TenantId == request.TenantId
                          && t.CompanyId == request.CompanyId)
                         || (request.TenantId.HasValue
                             && t.TenantId == request.TenantId
                             && t.CompanyId == null)
                         || (t.TenantId == null && t.CompanyId == null)),
                cancellationToken);

        var template = templates
            .Select(t => (Template: t, Priority: GetScopePriority(t, request)))
            .Where(x => x.Priority > 0)
            .OrderByDescending(x => x.Priority)
            .ThenByDescending(x => x.Template.Id)
            .Select(x => x.Template)
            .FirstOrDefault();
        if (template is not null)
        {
            return ApiResponse<CommunicationTemplateDto>.Ok(MapFgsTemplate(template));
        }

        if (!CommunicationTemplateChannelMapper.TryMapTemplateTypeToCommunicationChannel(
                normalizedType,
                out var communicationChannel))
        {
            return ApiResponse<CommunicationTemplateDto>.Fail(
                ["Template not found."],
                ApiStatusCodes.NotFound);
        }

        var gloTemplates = await unitOfWork.Repository<GloCommunicationTemplate>()
            .ListAsync(
                t => t.TemplateCode == normalizedCode
                     && t.CommunicationChannel == communicationChannel
                     && t.IsActive,
                cancellationToken);

        var gloTemplate = gloTemplates.OrderByDescending(t => t.Id).FirstOrDefault();
        if (gloTemplate is null)
        {
            return ApiResponse<CommunicationTemplateDto>.Fail(
                ["Template not found."],
                ApiStatusCodes.NotFound);
        }

        return ApiResponse<CommunicationTemplateDto>.Ok(MapGloTemplate(gloTemplate, normalizedType));
    }

    private static int GetScopePriority(
        FgsSetupCommunicationTemplate template,
        GetActiveCommunicationTemplateQuery request)
    {
        if (request.CompanyId.HasValue
            && template.TenantId == request.TenantId
            && template.CompanyId == request.CompanyId)
        {
            return 3;
        }

        if (request.TenantId.HasValue
            && template.TenantId == request.TenantId
            && template.CompanyId is null)
        {
            return 2;
        }

        if (template.TenantId is null && template.CompanyId is null)
        {
            return 1;
        }

        return 0;
    }

    private static CommunicationTemplateDto MapFgsTemplate(FgsSetupCommunicationTemplate template) =>
        new(
            template.Id,
            template.TenantId,
            template.CompanyId,
            template.TemplateType,
            template.Code,
            template.Name,
            template.Subject,
            template.Body,
            template.IsMobileVisible,
            template.IsActive);

    private static CommunicationTemplateDto MapGloTemplate(
        GloCommunicationTemplate template,
        string templateType) =>
        new(
            template.Id,
            TenantId: null,
            CompanyId: null,
            templateType,
            template.TemplateCode,
            template.Name,
            template.Subject,
            template.Body,
            template.IsMobileVisible,
            template.IsActive);

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
