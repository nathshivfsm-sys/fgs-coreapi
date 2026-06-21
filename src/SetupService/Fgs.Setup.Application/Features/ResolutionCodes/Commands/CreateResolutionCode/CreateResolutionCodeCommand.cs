using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.ResolutionCodes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.ResolutionCodes.Commands.CreateResolutionCode;

public sealed record CreateResolutionCodeCommand(ResolutionCodeCreateDto Dto)
    : IRequest<ApiResponse<ResolutionCodeDetailDto>>;
