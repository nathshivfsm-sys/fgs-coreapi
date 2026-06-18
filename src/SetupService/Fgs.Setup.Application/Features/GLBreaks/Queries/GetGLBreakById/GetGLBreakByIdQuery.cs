using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.GLBreaks.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.GLBreaks.Queries.GetGLBreakById;

public sealed record GetGLBreakByIdQuery(long Id)
    : IRequest<ApiResponse<GLBreakDetailDto>>;
