using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.PriceBooks.Dtos;

namespace Fgs.Setup.Application.Abstractions.PriceBooks;

public interface IFgsPriceBookReadRepository
{
    Task<FgsPriceBookDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsPriceBookSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsPriceBookListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsPriceBookLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByCodeAsync(
        string priceBookCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsJobTypeIdAsync(
        long id,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByIdAsync(
        long id,
        CancellationToken cancellationToken = default);
}
