using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPaymentTerms;
using Fgs.Setup.Application.Features.SetupPaymentTerms.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupPaymentTerms.Commands.PatchFgsSetupPaymentTerm;

public sealed class PatchFgsSetupPaymentTermCommandHandler(
    IFgsSetupPaymentTermWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<PatchFgsSetupPaymentTermCommandHandler> logger)
    : IRequestHandler<PatchFgsSetupPaymentTermCommand, ApiResponse<FgsSetupPaymentTermDetailDto>>
{
    public async Task<ApiResponse<FgsSetupPaymentTermDetailDto>> Handle(
        PatchFgsSetupPaymentTermCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patchd payment term {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "paymentterm"),
                cancellationToken);
        return ApiResponse<FgsSetupPaymentTermDetailDto>.Ok(result);
    }
}
