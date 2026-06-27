using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.ResolutionCodes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.ResolutionCodes.Commands.PatchResolutionCode;

public sealed record PatchResolutionCodeCommand(long Id, ResolutionCodePatchDto Dto)
    : IRequest<ApiResponse<ResolutionCodeDetailDto>>;
