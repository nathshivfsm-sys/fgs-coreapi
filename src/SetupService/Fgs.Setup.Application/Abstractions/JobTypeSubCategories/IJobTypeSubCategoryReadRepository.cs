using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.JobTypeSubCategories.Dtos;

namespace Fgs.Setup.Application.Abstractions.JobTypeSubCategories;

public interface IJobTypeSubCategoryReadRepository
{
    Task<JobTypeSubCategoryDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<JobTypeSubCategorySummaryDto>> ListAsync(
        SetupListQuery query,
        JobTypeSubCategoryListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<JobTypeSubCategoryLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsBySubCategoryCodeAsync(
        string subCategoryCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}
