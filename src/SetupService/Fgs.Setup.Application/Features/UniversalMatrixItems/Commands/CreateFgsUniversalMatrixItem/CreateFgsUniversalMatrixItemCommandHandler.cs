using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.UniversalMatrixItems;
using Fgs.Setup.Application.Features.UniversalMatrixItems.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.UniversalMatrixItems.Commands.CreateFgsUniversalMatrixItem;

public sealed class CreateFgsUniversalMatrixItemCommandHandler(
    IFgsUniversalMatrixItemWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsUniversalMatrixItemCommandHandler> logger)
    : IRequestHandler<CreateFgsUniversalMatrixItemCommand, ApiResponse<FgsUniversalMatrixItemDetailDto>>
{
    public async Task<ApiResponse<FgsUniversalMatrixItemDetailDto>> Handle(
        CreateFgsUniversalMatrixItemCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created universal matrix item {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "universalmatrixitem"),
            cancellationToken);
        return ApiResponse<FgsUniversalMatrixItemDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
