using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.UniversalMatrixOneTimeFees;
using Fgs.Setup.Application.Features.UniversalMatrixOneTimeFees.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.UniversalMatrixOneTimeFees.Commands.CreateFgsUniversalMatrixOneTimeFee;

public sealed class CreateFgsUniversalMatrixOneTimeFeeCommandHandler(
    IFgsUniversalMatrixOneTimeFeeWriteRepository writeRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsUniversalMatrixOneTimeFeeCommandHandler> logger)
    : IRequestHandler<CreateFgsUniversalMatrixOneTimeFeeCommand, ApiResponse<FgsUniversalMatrixOneTimeFeeDetailDto>>
{
    public async Task<ApiResponse<FgsUniversalMatrixOneTimeFeeDetailDto>> Handle(
        CreateFgsUniversalMatrixOneTimeFeeCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeRepository.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created universal matrix one-time fee {Id} with code {Name}", result.Id, result.Name);
            var tenantScope = tenantContextAccessor.Current!;
            await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "universalmatrixonetimefee"),
                cancellationToken);
        return ApiResponse<FgsUniversalMatrixOneTimeFeeDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
