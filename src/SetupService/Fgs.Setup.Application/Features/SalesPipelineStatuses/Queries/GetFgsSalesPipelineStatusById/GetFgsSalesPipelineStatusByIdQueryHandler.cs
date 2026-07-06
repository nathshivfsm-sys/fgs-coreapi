using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SalesPipelineStatuses;
using Fgs.Setup.Application.Features.SalesPipelineStatuses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesPipelineStatuses.Queries.GetFgsSalesPipelineStatusById;

public sealed class GetFgsSalesPipelineStatusByIdQueryHandler(
    IFgsSalesPipelineStatusReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsSalesPipelineStatusByIdQuery, ApiResponse<FgsSalesPipelineStatusDetailDto>>
{
    public async Task<ApiResponse<FgsSalesPipelineStatusDetailDto>> Handle(
        GetFgsSalesPipelineStatusByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "salespipelinestatus",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsSalesPipelineStatusDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsSalesPipelineStatusDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsSalesPipelineStatusDetailDto>.Fail(
                [$"Sales Pipeline Status '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsSalesPipelineStatusDetailDto>.Ok(result);
    }
}
