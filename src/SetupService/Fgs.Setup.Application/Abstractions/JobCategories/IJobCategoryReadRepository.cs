using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.JobCategories.Dtos;

namespace Fgs.Setup.Application.Abstractions.JobCategories;

public interface IJobCategoryReadRepository
{
    Task<JobCategoryDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<JobCategorySummaryDto>> ListAsync(
        SetupListQuery query,
        JobCategoryListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<JobCategoryLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByCategoryCodeAsync(
        string categoryCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}
