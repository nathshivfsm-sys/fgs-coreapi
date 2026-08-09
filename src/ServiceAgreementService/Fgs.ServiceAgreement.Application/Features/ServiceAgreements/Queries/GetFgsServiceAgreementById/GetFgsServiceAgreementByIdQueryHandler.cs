using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.ServiceAgreement.Application.Abstractions.ServiceAgreements;
using Fgs.ServiceAgreement.Application.Features.ServiceAgreements.Dtos;
using MediatR;

namespace Fgs.ServiceAgreement.Application.Features.ServiceAgreements.Queries.GetFgsServiceAgreementById;

public sealed class GetFgsServiceAgreementByIdQueryHandler(
    IFgsServiceAgreementReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsServiceAgreementByIdQuery, ApiResponse<FgsServiceAgreementDetailDto>>
{
    public async Task<ApiResponse<FgsServiceAgreementDetailDto>> Handle(
        GetFgsServiceAgreementByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "serviceagreement",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsServiceAgreementDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsServiceAgreementDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsServiceAgreementDetailDto>.Fail(
                [$"Service agreement '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsServiceAgreementDetailDto>.Ok(result);
    }
}
