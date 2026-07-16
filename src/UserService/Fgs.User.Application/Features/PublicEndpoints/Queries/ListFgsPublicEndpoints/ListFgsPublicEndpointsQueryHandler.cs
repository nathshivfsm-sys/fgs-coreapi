using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Abstractions.PublicEndpoints;
using Fgs.User.Application.Features.PublicEndpoints.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.PublicEndpoints.Queries.ListFgsPublicEndpoints;

public sealed class ListFgsPublicEndpointsQueryHandler(IFgsPublicEndpointReadRepository readRepository)
    : IRequestHandler<ListFgsPublicEndpointsQuery, ApiResponse<PagedResult<FgsPublicEndpointSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsPublicEndpointSummaryDto>>> Handle(
        ListFgsPublicEndpointsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsPublicEndpointSummaryDto>>.Ok(result);
    }
}
