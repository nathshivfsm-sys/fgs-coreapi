using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.NonWorkingDates;
using Fgs.Setup.Application.Features.NonWorkingDates.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.NonWorkingDates.Commands.PatchFgsNonWorkingDate;

public sealed class PatchFgsNonWorkingDateCommandHandler(
    IFgsNonWorkingDateWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<PatchFgsNonWorkingDateCommandHandler> logger)
    : IRequestHandler<PatchFgsNonWorkingDateCommand, ApiResponse<FgsNonWorkingDateDetailDto>>
{
    public async Task<ApiResponse<FgsNonWorkingDateDetailDto>> Handle(
        PatchFgsNonWorkingDateCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Patched non-working date {Id}", result.Id);
            var tenantScope = tenantContextAccessor.Current!;
            await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "nonworkingdate"),
                cancellationToken);
            return ApiResponse<FgsNonWorkingDateDetailDto>.Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return ApiResponse<FgsNonWorkingDateDetailDto>.Fail(
                ["Non-working date not found."],
                ApiStatusCodes.NotFound);
        }
    }
}
