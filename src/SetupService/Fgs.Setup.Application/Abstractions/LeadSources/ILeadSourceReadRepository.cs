using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.LeadSources.Dtos;

namespace Fgs.Setup.Application.Abstractions.LeadSources;

public interface ILeadSourceReadRepository
{
    Task<LeadSourceDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<LeadSourceSummaryDto>> ListAsync(
        SetupListQuery query,
        LeadSourceListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<LeadSourceLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsBySourceCodeAsync(
        string sourceCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}
