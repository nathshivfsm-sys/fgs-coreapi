using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.TechTrades.Dtos;

namespace Fgs.Setup.Application.Abstractions.TechTrades;

public interface ITechTradeReadRepository
{
    Task<TechTradeDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<TechTradeSummaryDto>> ListAsync(
        SetupListQuery query,
        TechTradeListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TechTradeLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByTradeCodeAsync(
        string tradeCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByNameAsync(
        string name,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}
