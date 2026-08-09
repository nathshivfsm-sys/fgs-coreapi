using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.JobTypeTasks.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypeTasks.Commands.DeleteJobTypeTask;

public sealed record DeleteJobTypeTaskCommand(long Id)
    : IRequest<ApiResponse<JobTypeTaskDetailDto>>;
