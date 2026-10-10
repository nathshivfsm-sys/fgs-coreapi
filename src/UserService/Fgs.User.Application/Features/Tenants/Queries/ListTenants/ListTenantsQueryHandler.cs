using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Security.Abstractions;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Features.Tenants.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Tenants.Queries.ListTenants;

public sealed class ListTenantsQueryHandler(
    ITenantCatalogReadRepository tenantCatalogReadRepository,
    IFgsUserContext userContext)
    : IRequestHandler<ListTenantsQuery, ApiResponse<PagedResult<TenantSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<TenantSummaryDto>>> Handle(
        ListTenantsQuery request,
        CancellationToken cancellationToken)
    {
        long? scopedTenantId = userContext.IsAuthenticated && userContext.TenantId is long tenantId
            ? tenantId
            : null;

        var page = await tenantCatalogReadRepository.ListTenantsAsync(
            request.Query,
            scopedTenantId,
            cancellationToken);

        return ApiResponse<PagedResult<TenantSummaryDto>>.Ok(page);
    }
}
