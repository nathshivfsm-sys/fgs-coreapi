using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.PublicEndpoints.Dtos;

namespace Fgs.User.Application.Abstractions.PublicEndpoints;

public interface IFgsPublicEndpointReadRepository
{
    Task<FgsPublicEndpointDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsPublicEndpointSummaryDto>> ListAsync(
        IdentityListQuery query,
        FgsPublicEndpointListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsPublicEndpointLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsPublicEndpointDetailDto>> ListActiveForTenantCompanyAsync(
        long tenantId,
        long companyId,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByTypeAndEnvironmentAsync(
        string endpointType,
        string environmentCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}
