using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.ResolutionCodes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.ResolutionCodes.Commands.UpdateResolutionCode;

public sealed record UpdateResolutionCodeCommand(long Id, ResolutionCodeUpdateDto Dto)
    : IRequest<ApiResponse<ResolutionCodeDetailDto>>;
