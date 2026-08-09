using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.JobTypeTasks.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypeTasks.Queries.ListActiveJobTypeTasks;

public sealed record ListActiveJobTypeTasksQuery(
    int Page = 1, int PageSize = 25, string? SortBy = null, SortDirection SortDirection = SortDirection.Asc, string? Search = null, JobTypeTaskListFilters? Filters = null)
    : IRequest<ApiResponse<PagedResult<JobTypeTaskSummaryDto>>>;
