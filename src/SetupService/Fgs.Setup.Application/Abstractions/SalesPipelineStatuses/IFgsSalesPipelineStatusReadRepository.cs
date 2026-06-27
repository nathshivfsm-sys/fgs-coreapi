using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SalesPipelineStatuses.Dtos;

namespace Fgs.Setup.Application.Abstractions.SalesPipelineStatuses;

public interface IFgsSalesPipelineStatusReadRepository
{
    Task<FgsSalesPipelineStatusDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsSalesPipelineStatusSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsSalesPipelineStatusListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsSalesPipelineStatusLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByStatusCodeAsync(
        string statusCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
    Task<bool> ExistsByStatusNameAsync(
        string statusName,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}
