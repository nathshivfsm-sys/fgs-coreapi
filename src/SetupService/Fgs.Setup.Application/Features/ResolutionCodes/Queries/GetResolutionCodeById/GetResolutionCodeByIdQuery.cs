using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.ResolutionCodes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.ResolutionCodes.Queries.GetResolutionCodeById;

public sealed record GetResolutionCodeByIdQuery(long Id)
    : IRequest<ApiResponse<ResolutionCodeDetailDto>>;
