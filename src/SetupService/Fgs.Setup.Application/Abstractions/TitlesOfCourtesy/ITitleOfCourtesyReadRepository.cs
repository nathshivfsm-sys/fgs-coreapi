using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Dtos;

namespace Fgs.Setup.Application.Abstractions.TitlesOfCourtesy;

public interface ITitleOfCourtesyReadRepository
{
    Task<TitleOfCourtesyDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<TitleOfCourtesySummaryDto>> ListAsync(
        SetupListQuery query,
        TitleOfCourtesyListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TitleOfCourtesyLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByCodeAsync(
        string code,
        long? excludeId = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByDisplayNameAsync(
        string displayName,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}
