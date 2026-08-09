using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.JobTypeTasks.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypeTasks.Commands.UpdateJobTypeTask;

public sealed record UpdateJobTypeTaskCommand(long Id, JobTypeTaskUpdateDto Dto)
    : IRequest<ApiResponse<JobTypeTaskDetailDto>>;
