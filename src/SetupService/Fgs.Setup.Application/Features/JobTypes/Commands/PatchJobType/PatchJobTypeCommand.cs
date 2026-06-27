using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.JobTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypes.Commands.PatchJobType;

public sealed record PatchJobTypeCommand(long Id, JobTypePatchDto Dto)
    : IRequest<ApiResponse<JobTypeDetailDto>>;
