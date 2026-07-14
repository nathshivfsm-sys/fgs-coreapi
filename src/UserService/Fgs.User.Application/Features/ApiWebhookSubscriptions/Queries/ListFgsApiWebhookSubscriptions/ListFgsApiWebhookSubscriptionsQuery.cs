using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.ApiWebhookSubscriptions.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ApiWebhookSubscriptions.Queries.ListFgsApiWebhookSubscriptions;

public sealed record ListFgsApiWebhookSubscriptionsQuery(
    IdentityListQuery Query,
    FgsApiWebhookSubscriptionListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsApiWebhookSubscriptionSummaryDto>>>;
