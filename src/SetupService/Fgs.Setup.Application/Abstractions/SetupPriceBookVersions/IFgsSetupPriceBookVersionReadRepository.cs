using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPriceBookVersions.Dtos;

namespace Fgs.Setup.Application.Abstractions.SetupPriceBookVersions;

public interface IFgsSetupPriceBookVersionReadRepository
{
    Task<FgsSetupPriceBookVersionDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsSetupPriceBookVersionSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsSetupPriceBookVersionListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsSetupPriceBookVersionLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByVersionAsync(
        string version,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}
