using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.Tags.Dtos;

namespace Fgs.Setup.Application.Abstractions.Tags;

public interface IFgsTagReadRepository
{
    Task<FgsTagDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsTagSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsTagListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsTagLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

}
