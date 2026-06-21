using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.JobTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypes.Queries.GetJobTypeById;

public sealed record GetJobTypeByIdQuery(long Id)
    : IRequest<ApiResponse<JobTypeDetailDto>>;
