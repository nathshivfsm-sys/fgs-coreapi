using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.PriceBookItems.Dtos;

namespace Fgs.Setup.Application.Abstractions.PriceBookItems;

public interface IFgsPriceBookItemReadRepository
{
    Task<FgsPriceBookItemDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsPriceBookItemSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsPriceBookItemListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsPriceBookItemLookupDto>> LookupAsync(
        long? priceBookId = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsPriceBookIdAsync(
        long id,
        CancellationToken cancellationToken = default);
}
