using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.UniversalMatrixAddOns;
using Fgs.Setup.Application.Features.UniversalMatrixAddOns.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.UniversalMatrixAddOns.Commands.CreateFgsUniversalMatrixAddOn;

public sealed class CreateFgsUniversalMatrixAddOnCommandHandler(
    IFgsUniversalMatrixAddOnWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsUniversalMatrixAddOnCommandHandler> logger)
    : IRequestHandler<CreateFgsUniversalMatrixAddOnCommand, ApiResponse<FgsUniversalMatrixAddOnDetailDto>>
{
    public async Task<ApiResponse<FgsUniversalMatrixAddOnDetailDto>> Handle(
        CreateFgsUniversalMatrixAddOnCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created universal matrix add-on {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "universalmatrixaddon"),
            cancellationToken);
        return ApiResponse<FgsUniversalMatrixAddOnDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
