using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.GLBreaks.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.GLBreaks.Commands.CreateGLBreak;

public sealed record CreateGLBreakCommand(GLBreakCreateDto Dto)
    : IRequest<ApiResponse<GLBreakDetailDto>>;
