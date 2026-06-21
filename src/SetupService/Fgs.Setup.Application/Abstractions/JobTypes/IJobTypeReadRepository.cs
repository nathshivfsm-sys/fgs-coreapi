using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.JobTypes.Dtos;

namespace Fgs.Setup.Application.Abstractions.JobTypes;

public interface IJobTypeReadRepository
{
    Task<JobTypeDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<JobTypeSummaryDto>> ListAsync(
        SetupListQuery query,
        JobTypeListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<JobTypeLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByJobTypeCodeAsync(
        string jobTypeCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
    Task<bool> ExistsJobTypeCategoryIdAsync(
        long id,
        CancellationToken cancellationToken = default);
    Task<bool> ExistsJobTypeSubCategoryIdAsync(
        long? id,
        CancellationToken cancellationToken = default);
}
