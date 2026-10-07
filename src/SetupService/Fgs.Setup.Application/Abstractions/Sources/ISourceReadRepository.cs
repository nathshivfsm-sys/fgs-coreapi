using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.Sources.Dtos;

namespace Fgs.Setup.Application.Abstractions.Sources;

public interface ISourceReadRepository
{
    Task<SourceDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<SourcesummaryDto>> ListAsync(
        SetupListQuery query,
        SourceListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SourceLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsBySourceCodeAsync(
        string sourceCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}
