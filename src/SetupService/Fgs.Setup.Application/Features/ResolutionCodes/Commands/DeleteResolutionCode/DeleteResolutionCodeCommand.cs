using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.ResolutionCodes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.ResolutionCodes.Commands.DeleteResolutionCode;

public sealed record DeleteResolutionCodeCommand(long Id)
    : IRequest<ApiResponse<ResolutionCodeDetailDto>>;
