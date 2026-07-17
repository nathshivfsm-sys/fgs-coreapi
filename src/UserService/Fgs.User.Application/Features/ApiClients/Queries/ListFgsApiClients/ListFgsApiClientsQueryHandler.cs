using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Abstractions.ApiClients;
using Fgs.User.Application.Features.ApiClients.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ApiClients.Queries.ListFgsApiClients;

public sealed class ListFgsApiClientsQueryHandler(IFgsApiClientReadRepository readRepository)
    : IRequestHandler<ListFgsApiClientsQuery, ApiResponse<PagedResult<FgsApiClientSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsApiClientSummaryDto>>> Handle(
        ListFgsApiClientsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsApiClientSummaryDto>>.Ok(result);
    }
}
