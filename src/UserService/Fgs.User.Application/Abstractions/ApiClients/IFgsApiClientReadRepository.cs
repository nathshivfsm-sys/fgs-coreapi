using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.ApiClients.Dtos;

namespace Fgs.User.Application.Abstractions.ApiClients;

public interface IFgsApiClientReadRepository
{
    Task<FgsApiClientDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsApiClientSummaryDto>> ListAsync(
        IdentityListQuery query,
        FgsApiClientListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsApiClientLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByApplicationNameAsync(
        string applicationName,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}
