using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.JobTypeTasks.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypeTasks.Queries.ListJobTypeTasks;

public sealed record ListJobTypeTasksQuery(
    SetupListQuery Query, JobTypeTaskListFilters Filters)
    : IRequest<ApiResponse<PagedResult<JobTypeTaskSummaryDto>>>;
