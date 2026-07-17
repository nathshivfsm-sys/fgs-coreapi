using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.ApiWebhookSubscriptions;
using Fgs.User.Application.Features.ApiWebhookSubscriptions.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.ApiWebhookSubscriptions.Commands.CreateFgsApiWebhookSubscription;

public sealed class CreateFgsApiWebhookSubscriptionCommandHandler(
    IFgsApiWebhookSubscriptionWriteService writeService,
    ILogger<CreateFgsApiWebhookSubscriptionCommandHandler> logger)
    : IRequestHandler<CreateFgsApiWebhookSubscriptionCommand, ApiResponse<FgsApiWebhookSubscriptionDetailDto>>
{
    public async Task<ApiResponse<FgsApiWebhookSubscriptionDetailDto>> Handle(
        CreateFgsApiWebhookSubscriptionCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created API webhook subscription {Id}", result.Id);
        return ApiResponse<FgsApiWebhookSubscriptionDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
