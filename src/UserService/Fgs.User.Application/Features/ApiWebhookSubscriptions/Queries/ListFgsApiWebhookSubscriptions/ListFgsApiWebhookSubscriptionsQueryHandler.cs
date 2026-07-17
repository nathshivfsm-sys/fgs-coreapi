using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Abstractions.ApiWebhookSubscriptions;
using Fgs.User.Application.Features.ApiWebhookSubscriptions.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ApiWebhookSubscriptions.Queries.ListFgsApiWebhookSubscriptions;

public sealed class ListFgsApiWebhookSubscriptionsQueryHandler(
    IFgsApiWebhookSubscriptionReadRepository readRepository)
    : IRequestHandler<ListFgsApiWebhookSubscriptionsQuery, ApiResponse<PagedResult<FgsApiWebhookSubscriptionSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsApiWebhookSubscriptionSummaryDto>>> Handle(
        ListFgsApiWebhookSubscriptionsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsApiWebhookSubscriptionSummaryDto>>.Ok(result);
    }
}
