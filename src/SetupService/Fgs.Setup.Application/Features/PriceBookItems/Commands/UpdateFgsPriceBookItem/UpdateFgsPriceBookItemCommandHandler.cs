using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.PriceBookItems;
using Fgs.Setup.Application.Features.PriceBookItems.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.PriceBookItems.Commands.UpdateFgsPriceBookItem;

public sealed class UpdateFgsPriceBookItemCommandHandler(
    IFgsPriceBookItemWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateFgsPriceBookItemCommandHandler> logger)
    : IRequestHandler<UpdateFgsPriceBookItemCommand, ApiResponse<FgsPriceBookItemDetailDto>>
{
    public async Task<ApiResponse<FgsPriceBookItemDetailDto>> Handle(
        UpdateFgsPriceBookItemCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated price book item {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "pricebookitem"),
            cancellationToken);
        return ApiResponse<FgsPriceBookItemDetailDto>.Ok(result);
    }
}
