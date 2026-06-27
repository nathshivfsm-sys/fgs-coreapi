using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.JobTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypes.Commands.UpdateJobType;

public sealed record UpdateJobTypeCommand(long Id, JobTypeUpdateDto Dto)
    : IRequest<ApiResponse<JobTypeDetailDto>>;
