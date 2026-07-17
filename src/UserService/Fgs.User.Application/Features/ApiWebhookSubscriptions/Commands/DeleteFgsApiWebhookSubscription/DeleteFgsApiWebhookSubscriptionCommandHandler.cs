using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.ApiWebhookSubscriptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.ApiWebhookSubscriptions.Commands.DeleteFgsApiWebhookSubscription;

public sealed class DeleteFgsApiWebhookSubscriptionCommandHandler(
    IFgsApiWebhookSubscriptionWriteService writeService,
    ILogger<DeleteFgsApiWebhookSubscriptionCommandHandler> logger)
    : IRequestHandler<DeleteFgsApiWebhookSubscriptionCommand, ApiResponse<object>>
{
    public async Task<ApiResponse<object>> Handle(
        DeleteFgsApiWebhookSubscriptionCommand request,
        CancellationToken cancellationToken)
    {
        await writeService.DeleteAsync(request.Id, cancellationToken);
        logger.LogInformation("Removed API webhook subscription {Id}", request.Id);
        return ApiResponse<object>.Ok(new object());
    }
}
