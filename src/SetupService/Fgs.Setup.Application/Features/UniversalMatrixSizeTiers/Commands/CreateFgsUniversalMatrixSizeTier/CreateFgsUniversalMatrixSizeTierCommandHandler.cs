using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.UniversalMatrixSizeTiers;
using Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Commands.CreateFgsUniversalMatrixSizeTier;

public sealed class CreateFgsUniversalMatrixSizeTierCommandHandler(
    IFgsUniversalMatrixSizeTierWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsUniversalMatrixSizeTierCommandHandler> logger)
    : IRequestHandler<CreateFgsUniversalMatrixSizeTierCommand, ApiResponse<FgsUniversalMatrixSizeTierDetailDto>>
{
    public async Task<ApiResponse<FgsUniversalMatrixSizeTierDetailDto>> Handle(
        CreateFgsUniversalMatrixSizeTierCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created universal matrix size tier {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "universalmatrixsizetier"),
            cancellationToken);
        return ApiResponse<FgsUniversalMatrixSizeTierDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
