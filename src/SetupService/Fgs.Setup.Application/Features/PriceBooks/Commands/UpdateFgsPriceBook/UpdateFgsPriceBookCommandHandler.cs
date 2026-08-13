using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.PriceBooks;
using Fgs.Setup.Application.Features.PriceBooks.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.PriceBooks.Commands.UpdateFgsPriceBook;

public sealed class UpdateFgsPriceBookCommandHandler(
    IFgsPriceBookWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateFgsPriceBookCommandHandler> logger)
    : IRequestHandler<UpdateFgsPriceBookCommand, ApiResponse<FgsPriceBookDetailDto>>
{
    public async Task<ApiResponse<FgsPriceBookDetailDto>> Handle(
        UpdateFgsPriceBookCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated price book {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "pricebook"),
            cancellationToken);
        return ApiResponse<FgsPriceBookDetailDto>.Ok(result);
    }
}
