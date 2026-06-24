using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.CatalogCrud;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupTaxes;
using Fgs.Setup.Application.Features.SetupTaxes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTaxes.Queries.GetFgsSetupTaxById;

public sealed class GetFgsSetupTaxByIdQueryHandler(
    IFgsSetupTaxReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsSetupTaxByIdQuery, ApiResponse<FgsSetupTaxDetailDto>>
{
    public async Task<ApiResponse<FgsSetupTaxDetailDto>> Handle(
        GetFgsSetupTaxByIdQuery request,
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
                    "taxes",
                    request.Id.ToString());

                var cached = await cache.GetAsync<FgsSetupTaxDetailDto>(cacheKey, cancellationToken);
                if (cached is not null)
                {
                    return ApiResponse<FgsSetupTaxDetailDto>.Ok(cached);
                }

                var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
                if (result is null)
                {
                    return ApiResponse<FgsSetupTaxDetailDto>.Fail(
                        [$"Tax '{request.Id}' was not found."],
                        ApiStatusCodes.NotFound);
                }

                await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
                return ApiResponse<FgsSetupTaxDetailDto>.Ok(result);
            }

            var uncached = await readRepository.GetByIdAsync(request.Id, cancellationToken);
            if (uncached is null)
            {
                return ApiResponse<FgsSetupTaxDetailDto>.Fail(
                    [$"Tax '{request.Id}' was not found."],
                    ApiStatusCodes.NotFound);
            }

            return ApiResponse<FgsSetupTaxDetailDto>.Ok(uncached);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<FgsSetupTaxDetailDto>(ex);
        }
    }
}
