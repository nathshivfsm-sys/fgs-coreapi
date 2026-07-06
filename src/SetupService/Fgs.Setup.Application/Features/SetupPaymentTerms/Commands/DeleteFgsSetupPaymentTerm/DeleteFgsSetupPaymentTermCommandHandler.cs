using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPaymentTerms;
using Fgs.Setup.Application.Features.SetupPaymentTerms.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupPaymentTerms.Commands.DeleteFgsSetupPaymentTerm;

public sealed class DeleteFgsSetupPaymentTermCommandHandler(
    IFgsSetupPaymentTermWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<DeleteFgsSetupPaymentTermCommandHandler> logger)
    : IRequestHandler<DeleteFgsSetupPaymentTermCommand, ApiResponse<FgsSetupPaymentTermDetailDto>>
{
    public async Task<ApiResponse<FgsSetupPaymentTermDetailDto>> Handle(
        DeleteFgsSetupPaymentTermCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.DeleteAsync(request.Id, cancellationToken);
        logger.LogInformation("Soft-deleted payment term {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "paymentterm"),
                cancellationToken);
        return ApiResponse<FgsSetupPaymentTermDetailDto>.Ok(result);
    }
}
