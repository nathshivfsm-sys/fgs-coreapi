using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.GLBreaks.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.GLBreaks.Queries.LookupGLBreaks;

public sealed record LookupGLBreaksQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<GLBreakLookupDto>>>;
