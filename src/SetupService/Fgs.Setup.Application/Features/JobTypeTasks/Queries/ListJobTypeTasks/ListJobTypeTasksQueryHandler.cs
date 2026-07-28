using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Abstractions.JobTypeTasks;
using Fgs.Setup.Application.Features.JobTypeTasks.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypeTasks.Queries.ListJobTypeTasks;

public sealed class ListJobTypeTasksQueryHandler(IJobTypeTaskReadRepository readRepository)
    : IRequestHandler<ListJobTypeTasksQuery, ApiResponse<PagedResult<JobTypeTaskSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<JobTypeTaskSummaryDto>>> Handle(
        ListJobTypeTasksQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<JobTypeTaskSummaryDto>>.Ok(result);
    }
}
