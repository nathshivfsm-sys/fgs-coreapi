using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrices;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupPricingMatrices.Commands.CreateFgsSetupPricingMatrix;

public sealed class CreateFgsSetupPricingMatrixCommandHandler(
    IFgsSetupPricingMatrixWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsSetupPricingMatrixCommandHandler> logger)
    : IRequestHandler<CreateFgsSetupPricingMatrixCommand, ApiResponse<FgsSetupPricingMatrixDetailDto>>
{
    public async Task<ApiResponse<FgsSetupPricingMatrixDetailDto>> Handle(
        CreateFgsSetupPricingMatrixCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created pricing matrix {Id} with code {Code}", result.Id, result.Code);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "pricingmatrix"),
            cancellationToken);
        return ApiResponse<FgsSetupPricingMatrixDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
