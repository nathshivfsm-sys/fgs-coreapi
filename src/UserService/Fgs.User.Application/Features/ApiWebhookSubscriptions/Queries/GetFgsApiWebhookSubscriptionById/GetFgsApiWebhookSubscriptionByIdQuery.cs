using Fgs.Contracts.Api;
using Fgs.User.Application.Features.ApiWebhookSubscriptions.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ApiWebhookSubscriptions.Queries.GetFgsApiWebhookSubscriptionById;

public sealed record GetFgsApiWebhookSubscriptionByIdQuery(long Id)
    : IRequest<ApiResponse<FgsApiWebhookSubscriptionDetailDto>>;
