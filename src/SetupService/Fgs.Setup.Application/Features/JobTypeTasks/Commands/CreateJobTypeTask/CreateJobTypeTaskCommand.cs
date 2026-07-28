using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.JobTypeTasks.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypeTasks.Commands.CreateJobTypeTask;

public sealed record CreateJobTypeTaskCommand(JobTypeTaskCreateDto Dto)
    : IRequest<ApiResponse<JobTypeTaskDetailDto>>;
