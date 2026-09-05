using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.EntityDefaultTermsConditions;
using Fgs.Setup.Application.Features.EntityDefaultTermsConditions.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.EntityDefaultTermsConditions.Commands.PatchFgsEntityDefaultTermsCondition;

public sealed class PatchFgsEntityDefaultTermsConditionCommandHandler(
    IFgsEntityDefaultTermsConditionWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<PatchFgsEntityDefaultTermsConditionCommandHandler> logger)
    : IRequestHandler<PatchFgsEntityDefaultTermsConditionCommand, ApiResponse<FgsEntityDefaultTermsConditionDetailDto>>
{
    public async Task<ApiResponse<FgsEntityDefaultTermsConditionDetailDto>> Handle(
        PatchFgsEntityDefaultTermsConditionCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patched entity default terms condition {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "entitydefaulttermscondition"),
            cancellationToken);
        return ApiResponse<FgsEntityDefaultTermsConditionDetailDto>.Ok(result);
    }
}
