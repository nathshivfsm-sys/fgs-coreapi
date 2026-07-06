using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPaymentMethods;
using Fgs.Setup.Application.Features.SetupPaymentMethods.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPaymentMethods.Queries.GetFgsSetupPaymentMethodById;

public sealed class GetFgsSetupPaymentMethodByIdQueryHandler(
    IFgsSetupPaymentMethodReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsSetupPaymentMethodByIdQuery, ApiResponse<FgsSetupPaymentMethodDetailDto>>
{
    public async Task<ApiResponse<FgsSetupPaymentMethodDetailDto>> Handle(
        GetFgsSetupPaymentMethodByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "paymentmethod",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsSetupPaymentMethodDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsSetupPaymentMethodDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsSetupPaymentMethodDetailDto>.Fail(
                [$"Payment Method '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsSetupPaymentMethodDetailDto>.Ok(result);
    }
}
