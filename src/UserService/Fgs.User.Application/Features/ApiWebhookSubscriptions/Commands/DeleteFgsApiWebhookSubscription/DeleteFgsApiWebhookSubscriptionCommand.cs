using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.User.Application.Features.ApiWebhookSubscriptions.Commands.DeleteFgsApiWebhookSubscription;

public sealed record DeleteFgsApiWebhookSubscriptionCommand(long Id) : IRequest<ApiResponse<object>>;
