using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrixOthers;
using Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Commands.CreateFgsSetupPricingMatrixOther;

public sealed class CreateFgsSetupPricingMatrixOtherCommandHandler(
    IFgsSetupPricingMatrixOtherWriteService writeService, ICacheService cache, ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsSetupPricingMatrixOtherCommandHandler> logger)
    : IRequestHandler<CreateFgsSetupPricingMatrixOtherCommand, ApiResponse<FgsSetupPricingMatrixOtherDetailDto>>
{
    public async Task<ApiResponse<FgsSetupPricingMatrixOtherDetailDto>> Handle(CreateFgsSetupPricingMatrixOtherCommand request, CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created Pricing Matrix Other {Id}", result.Id);
        var scope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(CacheKeys.EntityPrefix(scope.TenantId, scope.CompanyId, "pricingmatrixother"), cancellationToken);
        return ApiResponse<FgsSetupPricingMatrixOtherDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
