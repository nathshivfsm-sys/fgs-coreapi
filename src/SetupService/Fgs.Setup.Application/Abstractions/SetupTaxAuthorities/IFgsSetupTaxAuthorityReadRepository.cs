using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Dtos;

namespace Fgs.Setup.Application.Abstractions.SetupTaxAuthorities;

public interface IFgsSetupTaxAuthorityReadRepository
{
    Task<FgsSetupTaxAuthorityDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsSetupTaxAuthoritySummaryDto>> ListAsync(
        SetupListQuery query,
        FgsSetupTaxAuthorityListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsSetupTaxAuthorityLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByCodeAsync(
        string code,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}
