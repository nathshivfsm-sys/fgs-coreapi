using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.CommunicationTemplates;
using Fgs.Setup.Application.Features.CommunicationTemplates.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.CommunicationTemplates.Queries.LookupCommunicationTemplates;

public sealed class LookupCommunicationTemplatesQueryHandler(
    IFgsSetupCommunicationTemplateReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupCommunicationTemplatesQuery, ApiResponse<IReadOnlyList<FgsSetupCommunicationTemplateLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsSetupCommunicationTemplateLookupDto>>> Handle(
        LookupCommunicationTemplatesQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "communication-template",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<FgsSetupCommunicationTemplateLookupDto>>.Ok(result ?? Array.Empty<FgsSetupCommunicationTemplateLookupDto>());
    }
}
