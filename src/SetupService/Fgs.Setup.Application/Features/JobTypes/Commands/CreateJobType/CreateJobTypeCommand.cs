using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.JobTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypes.Commands.CreateJobType;

public sealed record CreateJobTypeCommand(JobTypeCreateDto Dto)
    : IRequest<ApiResponse<JobTypeDetailDto>>;
