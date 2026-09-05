using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.EntityDefaultTermsConditions;
using Fgs.Setup.Application.Features.EntityDefaultTermsConditions.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.EntityDefaultTermsConditions.Commands.UpdateFgsEntityDefaultTermsCondition;

public sealed class UpdateFgsEntityDefaultTermsConditionCommandHandler(
    IFgsEntityDefaultTermsConditionWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateFgsEntityDefaultTermsConditionCommandHandler> logger)
    : IRequestHandler<UpdateFgsEntityDefaultTermsConditionCommand, ApiResponse<FgsEntityDefaultTermsConditionDetailDto>>
{
    public async Task<ApiResponse<FgsEntityDefaultTermsConditionDetailDto>> Handle(
        UpdateFgsEntityDefaultTermsConditionCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated entity default terms condition {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "entitydefaulttermscondition"),
            cancellationToken);
        return ApiResponse<FgsEntityDefaultTermsConditionDetailDto>.Ok(result);
    }
}
