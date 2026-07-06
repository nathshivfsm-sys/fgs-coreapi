using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.Vendors;
using Fgs.Inventory.Application.Features.Vendors.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Inventory.Application.Features.Vendors.Commands.UpdateFgsVendor;

public sealed class UpdateFgsVendorCommandHandler(
    IFgsVendorWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateFgsVendorCommandHandler> logger)
    : IRequestHandler<UpdateFgsVendorCommand, ApiResponse<FgsVendorDetailDto>>
{
    public async Task<ApiResponse<FgsVendorDetailDto>> Handle(
        UpdateFgsVendorCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated vendor {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "vendor"),
            cancellationToken);
        return ApiResponse<FgsVendorDetailDto>.Ok(result);
    }
}
