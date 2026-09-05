using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.TermsConditions;
using Fgs.Setup.Application.Features.TermsConditions.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.TermsConditions.Commands.UpdateFgsTermsCondition;

public sealed class UpdateFgsTermsConditionCommandHandler(
    IFgsTermsConditionWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateFgsTermsConditionCommandHandler> logger)
    : IRequestHandler<UpdateFgsTermsConditionCommand, ApiResponse<FgsTermsConditionDetailDto>>
{
    public async Task<ApiResponse<FgsTermsConditionDetailDto>> Handle(
        UpdateFgsTermsConditionCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated terms condition {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "termscondition"),
            cancellationToken);
        return ApiResponse<FgsTermsConditionDetailDto>.Ok(result);
    }
}
