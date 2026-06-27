using Fgs.Contracts.Api;
using Fgs.Setup.Application.Abstractions.GLBreaks;
using Fgs.Setup.Application.Features.GLBreaks.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.GLBreaks.Queries.LookupGLBreaks;

public sealed class LookupGLBreaksQueryHandler(IGLBreakReadRepository readRepository)
    : IRequestHandler<LookupGLBreaksQuery, ApiResponse<IReadOnlyList<GLBreakLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<GLBreakLookupDto>>> Handle(
        LookupGLBreaksQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
        return ApiResponse<IReadOnlyList<GLBreakLookupDto>>.Ok(result);
    }
}
