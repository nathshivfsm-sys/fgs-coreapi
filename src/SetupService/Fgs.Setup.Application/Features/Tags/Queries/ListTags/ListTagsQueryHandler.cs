using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.Tags;
using Fgs.Setup.Application.Features.Tags.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Tags.Queries.ListTags;

public sealed class ListTagsQueryHandler(IFgsTagReadRepository readRepository)
    : IRequestHandler<ListTagsQuery, ApiResponse<PagedResult<FgsTagSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsTagSummaryDto>>> Handle(
        ListTagsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsTagSummaryDto>>.Ok(result);
    }
}
