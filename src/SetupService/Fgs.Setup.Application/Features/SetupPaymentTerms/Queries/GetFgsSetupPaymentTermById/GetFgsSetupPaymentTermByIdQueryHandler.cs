using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPaymentTerms;
using Fgs.Setup.Application.Features.SetupPaymentTerms.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPaymentTerms.Queries.GetFgsSetupPaymentTermById;

public sealed class GetFgsSetupPaymentTermByIdQueryHandler(
    IFgsSetupPaymentTermReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsSetupPaymentTermByIdQuery, ApiResponse<FgsSetupPaymentTermDetailDto>>
{
    public async Task<ApiResponse<FgsSetupPaymentTermDetailDto>> Handle(
        GetFgsSetupPaymentTermByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "paymentterm",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsSetupPaymentTermDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsSetupPaymentTermDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsSetupPaymentTermDetailDto>.Fail(
                [$"Payment Term '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsSetupPaymentTermDetailDto>.Ok(result);
    }
}
