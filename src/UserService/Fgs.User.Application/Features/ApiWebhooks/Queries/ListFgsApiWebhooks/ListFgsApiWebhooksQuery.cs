using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.ApiWebhooks.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ApiWebhooks.Queries.ListFgsApiWebhooks;

public sealed record ListFgsApiWebhooksQuery(
    IdentityListQuery Query,
    FgsApiWebhookListFilters Filters) : IRequest<ApiResponse<PagedResult<FgsApiWebhookSummaryDto>>>;
