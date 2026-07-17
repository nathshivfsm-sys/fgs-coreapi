using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.ApiWebhooks;
using Fgs.User.Application.Features.ApiWebhooks.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.ApiWebhooks.Commands.UpdateFgsApiWebhook;

public sealed class UpdateFgsApiWebhookCommandHandler(
    IFgsApiWebhookWriteService writeService,
    ILogger<UpdateFgsApiWebhookCommandHandler> logger)
    : IRequestHandler<UpdateFgsApiWebhookCommand, ApiResponse<FgsApiWebhookDetailDto>>
{
    public async Task<ApiResponse<FgsApiWebhookDetailDto>> Handle(
        UpdateFgsApiWebhookCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated API webhook {ApiWebhookId}", result.Id);
        return ApiResponse<FgsApiWebhookDetailDto>.Ok(result);
    }
}
