using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.CommunicationTemplates;
using Fgs.Setup.Application.Features.CommunicationTemplates;
using MediatR;

namespace Fgs.Setup.Application.Features.CommunicationTemplates.Queries.GetActiveCommunicationTemplate;

public sealed class GetActiveCommunicationTemplateQueryHandler(
    IActiveCommunicationTemplateReadRepository readRepository,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetActiveCommunicationTemplateQuery, ApiResponse<CommunicationTemplateDto>>
{
    public async Task<ApiResponse<CommunicationTemplateDto>> Handle(
        GetActiveCommunicationTemplateQuery request,
        CancellationToken cancellationToken)
    {
        var scope = ResolveScope(request);
        if (scope.Error is not null)
        {
            return scope.Error;
        }

        var normalizedType = request.TemplateType.Trim();
        var normalizedCode = request.Code.Trim();
        var templates = await readRepository.ListFgsMatchesAsync(
            scope.TenantId,
            scope.CompanyId,
            normalizedType,
            normalizedCode,
            cancellationToken);

        var template = templates
            .Select(t => (Template: t, Priority: GetScopePriority(t, scope.TenantId, scope.CompanyId)))
            .Where(x => x.Priority > 0)
            .OrderByDescending(x => x.Priority)
            .ThenByDescending(x => x.Template.Id)
            .Select(x => x.Template)
            .FirstOrDefault();
        if (template is not null)
        {
            return ApiResponse<CommunicationTemplateDto>.Ok(Map(template));
        }

        if (!CommunicationTemplateChannelMapper.TryMapTemplateTypeToCommunicationChannel(
                normalizedType,
                out var communicationChannel))
        {
            return ApiResponse<CommunicationTemplateDto>.Fail(
                ["Template not found."],
                ApiStatusCodes.NotFound);
        }

        var gloTemplate = await readRepository.FindLatestGloAsync(
            normalizedType,
            communicationChannel,
            normalizedCode,
            cancellationToken);
        if (gloTemplate is null)
        {
            return ApiResponse<CommunicationTemplateDto>.Fail(
                ["Template not found."],
                ApiStatusCodes.NotFound);
        }

        return ApiResponse<CommunicationTemplateDto>.Ok(Map(gloTemplate));
    }

    private (long? TenantId, long? CompanyId, ApiResponse<CommunicationTemplateDto>? Error) ResolveScope(
        GetActiveCommunicationTemplateQuery request)
    {
        if (request.IsInternalService)
        {
            return (request.TenantId, request.CompanyId, null);
        }

        if (tenantContextAccessor.Current is not { } tenantScope)
        {
            return (null, null, ApiResponse<CommunicationTemplateDto>.Fail(
                ["Tenant context is required."],
                ApiStatusCodes.BadRequest));
        }

        if (request.TenantId is { } requestedTenant && requestedTenant != tenantScope.TenantId)
        {
            return (null, null, ApiResponse<CommunicationTemplateDto>.Fail(
                ["Template tenant/company does not match the active tenant scope."],
                ApiStatusCodes.Forbidden));
        }

        if (request.CompanyId is { } requestedCompany && requestedCompany != tenantScope.CompanyId)
        {
            return (null, null, ApiResponse<CommunicationTemplateDto>.Fail(
                ["Template tenant/company does not match the active tenant scope."],
                ApiStatusCodes.Forbidden));
        }

        return (tenantScope.TenantId, tenantScope.CompanyId, null);
    }

    private static int GetScopePriority(
        ActiveCommunicationTemplateCandidate template,
        long? tenantId,
        long? companyId)
    {
        if (companyId.HasValue
            && template.TenantId == tenantId
            && template.CompanyId == companyId)
        {
            return 3;
        }

        if (tenantId.HasValue
            && template.TenantId == tenantId
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

    private static CommunicationTemplateDto Map(ActiveCommunicationTemplateCandidate template) =>
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
}
