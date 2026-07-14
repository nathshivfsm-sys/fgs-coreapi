using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.ApiWebhooks;
using Fgs.User.Application.Features.ApiWebhooks.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.ApiWebhooks.Commands.PatchFgsApiWebhook;

public sealed class PatchFgsApiWebhookCommandHandler(
    IFgsApiWebhookWriteService writeService,
    ILogger<PatchFgsApiWebhookCommandHandler> logger)
    : IRequestHandler<PatchFgsApiWebhookCommand, ApiResponse<FgsApiWebhookDetailDto>>
{
    public async Task<ApiResponse<FgsApiWebhookDetailDto>> Handle(
        PatchFgsApiWebhookCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patched API webhook {ApiWebhookId}", result.Id);
        return ApiResponse<FgsApiWebhookDetailDto>.Ok(result);
    }
}
