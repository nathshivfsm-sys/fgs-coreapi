using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.GLBreaks.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.GLBreaks.Commands.PatchGLBreak;

public sealed record PatchGLBreakCommand(long Id, GLBreakPatchDto Dto)
    : IRequest<ApiResponse<GLBreakDetailDto>>;
