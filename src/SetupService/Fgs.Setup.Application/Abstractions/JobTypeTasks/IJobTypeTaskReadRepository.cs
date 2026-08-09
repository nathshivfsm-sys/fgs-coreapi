using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.JobTypeTasks.Dtos;

namespace Fgs.Setup.Application.Abstractions.JobTypeTasks;

public interface IJobTypeTaskReadRepository
{
    Task<JobTypeTaskDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<JobTypeTaskSummaryDto>> ListAsync(
        SetupListQuery query,
        JobTypeTaskListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<JobTypeTaskLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsJobTypeCategoryIdAsync(
        long id,
        CancellationToken cancellationToken = default);
    Task<bool> ExistsTradeIdAsync(
        long id,
        CancellationToken cancellationToken = default);
}
