using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Abstractions.GLBreaks;
using Fgs.Setup.Application.Features.GLBreaks.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.GLBreaks.Queries.ListGLBreaks;

public sealed class ListGLBreaksQueryHandler(IGLBreakReadRepository readRepository)
    : IRequestHandler<ListGLBreaksQuery, ApiResponse<PagedResult<GLBreakSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<GLBreakSummaryDto>>> Handle(
        ListGLBreaksQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<GLBreakSummaryDto>>.Ok(result);
    }
}
