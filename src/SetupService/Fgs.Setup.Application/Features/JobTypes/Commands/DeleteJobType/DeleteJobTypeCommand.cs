using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.JobTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypes.Commands.DeleteJobType;

public sealed record DeleteJobTypeCommand(long Id)
    : IRequest<ApiResponse<JobTypeDetailDto>>;
