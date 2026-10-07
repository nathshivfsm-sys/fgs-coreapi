using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Abstractions.Sources;
using Fgs.Setup.Application.Features.Sources.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Sources.Queries.ListSources;

public sealed class ListSourcesQueryHandler(ISourceReadRepository readRepository)
    : IRequestHandler<ListSourcesQuery, ApiResponse<PagedResult<SourcesummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<SourcesummaryDto>>> Handle(
        ListSourcesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<SourcesummaryDto>>.Ok(result);
    }
}
