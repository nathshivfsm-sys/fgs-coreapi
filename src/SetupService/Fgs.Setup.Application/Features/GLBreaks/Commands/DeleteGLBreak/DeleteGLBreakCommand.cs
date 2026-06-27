using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.GLBreaks.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.GLBreaks.Commands.DeleteGLBreak;

public sealed record DeleteGLBreakCommand(long Id)
    : IRequest<ApiResponse<GLBreakDetailDto>>;
