using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.GLBreaks.Dtos;

namespace Fgs.Setup.Application.Abstractions.GLBreaks;

public interface IGLBreakReadRepository
{
    Task<GLBreakDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<GLBreakSummaryDto>> ListAsync(
        SetupListQuery query,
        GLBreakListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<GLBreakLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByCodeAndBreakLevelAsync(
        string code,
        short breakLevel,
        long? excludeId = null,
        CancellationToken cancellationToken = default);

    Task<short?> GetBreakLevelByIdAsync(long id, CancellationToken cancellationToken = default);
}
