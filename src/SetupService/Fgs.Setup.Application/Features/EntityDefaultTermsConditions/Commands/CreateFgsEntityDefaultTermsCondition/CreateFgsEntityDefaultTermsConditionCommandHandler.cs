using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.EntityDefaultTermsConditions;
using Fgs.Setup.Application.Features.EntityDefaultTermsConditions.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.EntityDefaultTermsConditions.Commands.CreateFgsEntityDefaultTermsCondition;

public sealed class CreateFgsEntityDefaultTermsConditionCommandHandler(
    IFgsEntityDefaultTermsConditionWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsEntityDefaultTermsConditionCommandHandler> logger)
    : IRequestHandler<CreateFgsEntityDefaultTermsConditionCommand, ApiResponse<FgsEntityDefaultTermsConditionDetailDto>>
{
    public async Task<ApiResponse<FgsEntityDefaultTermsConditionDetailDto>> Handle(
        CreateFgsEntityDefaultTermsConditionCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation(
            "Created entity default terms condition {Id} for entity type {EntityType}",
            result.Id,
            result.EntityType);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "entitydefaulttermscondition"),
            cancellationToken);
        return ApiResponse<FgsEntityDefaultTermsConditionDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
