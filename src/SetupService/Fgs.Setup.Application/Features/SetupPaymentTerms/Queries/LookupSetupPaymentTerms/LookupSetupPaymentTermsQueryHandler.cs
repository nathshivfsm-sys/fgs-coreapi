using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.CatalogCrud;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPaymentTerms;
using Fgs.Setup.Application.Features.SetupPaymentTerms.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPaymentTerms.Queries.LookupSetupPaymentTerms;

public sealed class LookupSetupPaymentTermsQueryHandler(
    IFgsSetupPaymentTermReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupSetupPaymentTermsQuery, ApiResponse<IReadOnlyList<FgsSetupPaymentTermLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsSetupPaymentTermLookupDto>>> Handle(
        LookupSetupPaymentTermsQuery request,
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
                    "paymentterms",
                    CacheKeys.LookupSegment(request.ActiveOnly));

                var result = await cache.GetOrSetAsync(
                    cacheKey,
                    () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
                    cancellationToken: cancellationToken);

                return ApiResponse<IReadOnlyList<FgsSetupPaymentTermLookupDto>>.Ok(result ?? Array.Empty<FgsSetupPaymentTermLookupDto>());
            }

            var uncached = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
            return ApiResponse<IReadOnlyList<FgsSetupPaymentTermLookupDto>>.Ok(uncached);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<IReadOnlyList<FgsSetupPaymentTermLookupDto>>(ex);
        }
    }
}
