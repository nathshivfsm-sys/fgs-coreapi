using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.UniversalMatrixFrequencyDiscounts;
using Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Commands.CreateFgsUniversalMatrixFrequencyDiscount;

public sealed class CreateFgsUniversalMatrixFrequencyDiscountCommandHandler(
    IFgsUniversalMatrixFrequencyDiscountWriteRepository writeRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsUniversalMatrixFrequencyDiscountCommandHandler> logger)
    : IRequestHandler<CreateFgsUniversalMatrixFrequencyDiscountCommand, ApiResponse<FgsUniversalMatrixFrequencyDiscountDetailDto>>
{
    public async Task<ApiResponse<FgsUniversalMatrixFrequencyDiscountDetailDto>> Handle(
        CreateFgsUniversalMatrixFrequencyDiscountCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeRepository.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created universal matrix frequency discount {Id} with code {Name}", result.Id, result.Name);
            var tenantScope = tenantContextAccessor.Current!;
            await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "universalmatrixfrequencydiscount"),
                cancellationToken);
        return ApiResponse<FgsUniversalMatrixFrequencyDiscountDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
