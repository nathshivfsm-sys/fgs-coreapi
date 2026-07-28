using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.JobTypeTasks.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypeTasks.Queries.GetJobTypeTaskById;

public sealed record GetJobTypeTaskByIdQuery(long Id)
    : IRequest<ApiResponse<JobTypeTaskDetailDto>>;
