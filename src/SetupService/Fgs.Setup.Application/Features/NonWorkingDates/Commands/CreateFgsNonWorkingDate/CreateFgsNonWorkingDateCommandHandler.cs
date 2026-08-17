using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.NonWorkingDates;
using Fgs.Setup.Application.Features.NonWorkingDates.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.NonWorkingDates.Commands.CreateFgsNonWorkingDate;

public sealed class CreateFgsNonWorkingDateCommandHandler(
    IFgsNonWorkingDateWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsNonWorkingDateCommandHandler> logger)
    : IRequestHandler<CreateFgsNonWorkingDateCommand, ApiResponse<FgsNonWorkingDateDetailDto>>
{
    public async Task<ApiResponse<FgsNonWorkingDateDetailDto>> Handle(
        CreateFgsNonWorkingDateCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation(
            "Created non-working date {Id} for {Date}",
            result.Id,
            result.NonWorkingDate);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "nonworkingdate"),
            cancellationToken);
        return ApiResponse<FgsNonWorkingDateDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
