using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Abstractions.ApiWebhooks;
using Fgs.User.Application.Features.ApiWebhooks.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ApiWebhooks.Queries.ListFgsApiWebhooks;

public sealed class ListFgsApiWebhooksQueryHandler(IFgsApiWebhookReadRepository readRepository)
    : IRequestHandler<ListFgsApiWebhooksQuery, ApiResponse<PagedResult<FgsApiWebhookSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsApiWebhookSummaryDto>>> Handle(
        ListFgsApiWebhooksQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsApiWebhookSummaryDto>>.Ok(result);
    }
}
