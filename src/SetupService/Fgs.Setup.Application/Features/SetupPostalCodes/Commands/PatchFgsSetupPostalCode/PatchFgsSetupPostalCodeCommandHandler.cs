using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPostalCodes;
using Fgs.Setup.Application.Features.SetupPostalCodes.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupPostalCodes.Commands.PatchFgsSetupPostalCode;

public sealed class PatchFgsSetupPostalCodeCommandHandler(
    IFgsSetupPostalCodeWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<PatchFgsSetupPostalCodeCommandHandler> logger)
    : IRequestHandler<PatchFgsSetupPostalCodeCommand, ApiResponse<FgsSetupPostalCodeDetailDto>>
{
    public async Task<ApiResponse<FgsSetupPostalCodeDetailDto>> Handle(
        PatchFgsSetupPostalCodeCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patchd postal code {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "postalcode"),
                cancellationToken);
        return ApiResponse<FgsSetupPostalCodeDetailDto>.Ok(result);
    }
}
