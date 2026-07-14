using Fgs.Contracts.Api;
using Fgs.User.Application.Features.ApiWebhookSubscriptions.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ApiWebhookSubscriptions.Commands.CreateFgsApiWebhookSubscription;

public sealed record CreateFgsApiWebhookSubscriptionCommand(FgsApiWebhookSubscriptionCreateDto Dto)
    : IRequest<ApiResponse<FgsApiWebhookSubscriptionDetailDto>>;
