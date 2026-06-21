using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SalesActivityOutcomes.Dtos;

namespace Fgs.Setup.Application.Abstractions.SalesActivityOutcomes;

public interface IFgsSalesActivityOutcomeReadRepository
{
    Task<FgsSalesActivityOutcomeDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsSalesActivityOutcomeSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsSalesActivityOutcomeListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsSalesActivityOutcomeLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByOutcomeCodeAsync(
        string outcomeCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
    Task<bool> ExistsByOutcomeNameAsync(
        string outcomeName,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
    Task<bool> ExistsSalesPipelineStatusIdAsync(
        long? id,
        CancellationToken cancellationToken = default);
}
