using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SalesDispositionReasons;
using Fgs.Setup.Application.Features.SalesDispositionReasons.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesDispositionReasons.Queries.GetFgsSalesDispositionReasonById;

public sealed class GetFgsSalesDispositionReasonByIdQueryHandler(
    IFgsSalesDispositionReasonReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsSalesDispositionReasonByIdQuery, ApiResponse<FgsSalesDispositionReasonDetailDto>>
{
    public async Task<ApiResponse<FgsSalesDispositionReasonDetailDto>> Handle(
        GetFgsSalesDispositionReasonByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "salesdispositionreason",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsSalesDispositionReasonDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsSalesDispositionReasonDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsSalesDispositionReasonDetailDto>.Fail(
                [$"Sales Disposition Reason '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsSalesDispositionReasonDetailDto>.Ok(result);
    }
}
