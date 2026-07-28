using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.JobTypeTasks.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypeTasks.Commands.PatchJobTypeTask;

public sealed record PatchJobTypeTaskCommand(long Id, JobTypeTaskPatchDto Dto)
    : IRequest<ApiResponse<JobTypeTaskDetailDto>>;
