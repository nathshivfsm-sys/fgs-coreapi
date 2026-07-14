using Fgs.Contracts.Api;
using Fgs.User.Application.Features.ApiWebhooks.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ApiWebhooks.Queries.LookupFgsApiWebhooks;

public sealed record LookupFgsApiWebhooksQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsApiWebhookLookupDto>>>;
