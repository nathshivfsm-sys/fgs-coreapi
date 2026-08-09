using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.JobTypeCategories.Dtos;

namespace Fgs.Setup.Application.Abstractions.JobTypeCategories;

public interface IJobTypeCategoryReadRepository
{
    Task<JobTypeCategoryDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<JobTypeCategorySummaryDto>> ListAsync(
        SetupListQuery query,
        JobTypeCategoryListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<JobTypeCategoryLookupDto>> LookupAsync(
        bool activeOnly = true,
        long? jobTypeId = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByJobTypeIdAndJobCategoryIdAsync(
        long jobTypeId, long jobCategoryId,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
    Task<bool> ExistsJobTypeIdAsync(
        long id,
        CancellationToken cancellationToken = default);
    Task<bool> ExistsJobCategoryIdAsync(
        long id,
        CancellationToken cancellationToken = default);
}
