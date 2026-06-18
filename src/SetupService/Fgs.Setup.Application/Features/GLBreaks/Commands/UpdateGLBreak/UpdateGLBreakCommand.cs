using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.GLBreaks.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.GLBreaks.Commands.UpdateGLBreak;

public sealed record UpdateGLBreakCommand(long Id, GLBreakUpdateDto Dto)
    : IRequest<ApiResponse<GLBreakDetailDto>>;
