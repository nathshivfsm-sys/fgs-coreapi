using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupTechSkillLevels;
using Fgs.Setup.Application.Features.SetupTechSkillLevels.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTechSkillLevels.Queries.LookupSetupTechSkillLevels;

public sealed class LookupSetupTechSkillLevelsQueryHandler(
    IFgsSetupTechSkillLevelReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupSetupTechSkillLevelsQuery, ApiResponse<IReadOnlyList<FgsSetupTechSkillLevelLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsSetupTechSkillLevelLookupDto>>> Handle(
        LookupSetupTechSkillLevelsQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "techskilllevels",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<FgsSetupTechSkillLevelLookupDto>>.Ok(result ?? Array.Empty<FgsSetupTechSkillLevelLookupDto>());
    }
}
