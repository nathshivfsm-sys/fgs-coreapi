using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.JobTypeTasks.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypeTasks.Queries.LookupJobTypeTasks;

public sealed record LookupJobTypeTasksQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<JobTypeTaskLookupDto>>>;
