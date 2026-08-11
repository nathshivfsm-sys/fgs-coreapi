using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.NonWorkingDates;
using Fgs.Setup.Application.Features.NonWorkingDates.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.NonWorkingDates.Commands.UpdateFgsNonWorkingDate;

public sealed class UpdateFgsNonWorkingDateCommandHandler(
    IFgsNonWorkingDateWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateFgsNonWorkingDateCommandHandler> logger)
    : IRequestHandler<UpdateFgsNonWorkingDateCommand, ApiResponse<FgsNonWorkingDateDetailDto>>
{
    public async Task<ApiResponse<FgsNonWorkingDateDetailDto>> Handle(
        UpdateFgsNonWorkingDateCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
            logger.LogInformation("Updated non-working date {Id}", result.Id);
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
