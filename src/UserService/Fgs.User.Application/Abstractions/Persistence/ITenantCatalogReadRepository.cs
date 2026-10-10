using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.Tenants.Dtos;

namespace Fgs.User.Application.Abstractions.Persistence;

public interface ITenantCatalogReadRepository
{
    Task<PagedResult<TenantSummaryDto>> ListTenantsAsync(
        IdentityListQuery query,
        long? scopedTenantId,
        CancellationToken cancellationToken = default);

    Task<long> GetMaxCompanyNumberAsync(long tenantId, CancellationToken cancellationToken = default);
}
