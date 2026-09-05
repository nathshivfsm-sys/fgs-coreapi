using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.PriceBooks;
using Fgs.Setup.Application.Features.PriceBooks.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.PriceBooks.Commands.PatchFgsPriceBook;

public sealed class PatchFgsPriceBookCommandHandler(
    IFgsPriceBookWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<PatchFgsPriceBookCommandHandler> logger)
    : IRequestHandler<PatchFgsPriceBookCommand, ApiResponse<FgsPriceBookDetailDto>>
{
    public async Task<ApiResponse<FgsPriceBookDetailDto>> Handle(
        PatchFgsPriceBookCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patched price book {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "pricebook"),
            cancellationToken);
        return ApiResponse<FgsPriceBookDetailDto>.Ok(result);
    }
}
