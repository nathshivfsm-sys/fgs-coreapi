using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupZones;
using Fgs.Setup.Application.Features.SetupZones.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupZones.Commands.CreateFgsSetupZone;

public sealed class CreateFgsSetupZoneCommandHandler(
    IFgsSetupZoneWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsSetupZoneCommandHandler> logger)
    : IRequestHandler<CreateFgsSetupZoneCommand, ApiResponse<FgsSetupZoneDetailDto>>
{
    public async Task<ApiResponse<FgsSetupZoneDetailDto>> Handle(
        CreateFgsSetupZoneCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created zone {Id} with code {Code}", result.Id, result.Code);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "zones"),
                cancellationToken);
        return ApiResponse<FgsSetupZoneDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
