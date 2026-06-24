using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.CatalogCrud;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPostalCodes;
using Fgs.Setup.Application.Features.SetupPostalCodes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPostalCodes.Queries.GetFgsSetupPostalCodeById;

public sealed class GetFgsSetupPostalCodeByIdQueryHandler(
    IFgsSetupPostalCodeReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsSetupPostalCodeByIdQuery, ApiResponse<FgsSetupPostalCodeDetailDto>>
{
    public async Task<ApiResponse<FgsSetupPostalCodeDetailDto>> Handle(
        GetFgsSetupPostalCodeByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var tenantScope = tenantContextAccessor.Current;
            if (tenantScope?.IsResolved == true)
            {
                var cacheKey = CacheKeys.Build(
                    tenantScope.TenantId,
                    tenantScope.CompanyId,
                    "postalcodes",
                    request.Id.ToString());

                var cached = await cache.GetAsync<FgsSetupPostalCodeDetailDto>(cacheKey, cancellationToken);
                if (cached is not null)
                {
                    return ApiResponse<FgsSetupPostalCodeDetailDto>.Ok(cached);
                }

                var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
                if (result is null)
                {
                    return ApiResponse<FgsSetupPostalCodeDetailDto>.Fail(
                        [$"Postal Code '{request.Id}' was not found."],
                        ApiStatusCodes.NotFound);
                }

                await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
                return ApiResponse<FgsSetupPostalCodeDetailDto>.Ok(result);
            }

            var uncached = await readRepository.GetByIdAsync(request.Id, cancellationToken);
            if (uncached is null)
            {
                return ApiResponse<FgsSetupPostalCodeDetailDto>.Fail(
                    [$"Postal Code '{request.Id}' was not found."],
                    ApiStatusCodes.NotFound);
            }

            return ApiResponse<FgsSetupPostalCodeDetailDto>.Ok(uncached);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<FgsSetupPostalCodeDetailDto>(ex);
        }
    }
}
