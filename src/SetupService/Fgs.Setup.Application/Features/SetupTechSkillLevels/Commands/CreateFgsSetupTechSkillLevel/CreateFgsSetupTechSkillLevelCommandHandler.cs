using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupTechSkillLevels;
using Fgs.Setup.Application.Features.SetupTechSkillLevels.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupTechSkillLevels.Commands.CreateFgsSetupTechSkillLevel;

public sealed class CreateFgsSetupTechSkillLevelCommandHandler(
    IFgsSetupTechSkillLevelWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsSetupTechSkillLevelCommandHandler> logger)
    : IRequestHandler<CreateFgsSetupTechSkillLevelCommand, ApiResponse<FgsSetupTechSkillLevelDetailDto>>
{
    public async Task<ApiResponse<FgsSetupTechSkillLevelDetailDto>> Handle(
        CreateFgsSetupTechSkillLevelCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created tech skill level {Id} with code {Code}", result.Id, result.Code);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "techskilllevels"),
                cancellationToken);
        return ApiResponse<FgsSetupTechSkillLevelDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
