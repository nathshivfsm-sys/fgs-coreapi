using Fgs.Foundation.CatalogCrud;
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
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByCategoryCodeAsync(
        string categoryCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}
