using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.PriceBooks;
using Fgs.Setup.Application.Features.PriceBooks.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.PriceBooks.Commands.CreateFgsPriceBook;

public sealed class CreateFgsPriceBookCommandHandler(
    IFgsPriceBookWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsPriceBookCommandHandler> logger)
    : IRequestHandler<CreateFgsPriceBookCommand, ApiResponse<FgsPriceBookDetailDto>>
{
    public async Task<ApiResponse<FgsPriceBookDetailDto>> Handle(
        CreateFgsPriceBookCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created price book {Id} ({Code})", result.Id, result.PriceBookCode);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "pricebook"),
            cancellationToken);
        return ApiResponse<FgsPriceBookDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
