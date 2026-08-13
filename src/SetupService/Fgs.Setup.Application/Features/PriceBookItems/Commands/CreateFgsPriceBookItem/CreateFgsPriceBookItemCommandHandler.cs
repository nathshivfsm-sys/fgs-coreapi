using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.PriceBookItems;
using Fgs.Setup.Application.Features.PriceBookItems.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.PriceBookItems.Commands.CreateFgsPriceBookItem;

public sealed class CreateFgsPriceBookItemCommandHandler(
    IFgsPriceBookItemWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsPriceBookItemCommandHandler> logger)
    : IRequestHandler<CreateFgsPriceBookItemCommand, ApiResponse<FgsPriceBookItemDetailDto>>
{
    public async Task<ApiResponse<FgsPriceBookItemDetailDto>> Handle(
        CreateFgsPriceBookItemCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created price book item {Id} for price book {PriceBookId}", result.Id, result.PriceBookId);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "pricebookitem"),
            cancellationToken);
        return ApiResponse<FgsPriceBookItemDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
