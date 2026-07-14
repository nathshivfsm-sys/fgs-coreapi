using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.ApiWebhooks;
using Fgs.User.Application.Features.ApiWebhooks.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.ApiWebhooks.Commands.CreateFgsApiWebhook;

public sealed class CreateFgsApiWebhookCommandHandler(
    IFgsApiWebhookWriteService writeService,
    ILogger<CreateFgsApiWebhookCommandHandler> logger)
    : IRequestHandler<CreateFgsApiWebhookCommand, ApiResponse<FgsApiWebhookDetailDto>>
{
    public async Task<ApiResponse<FgsApiWebhookDetailDto>> Handle(
        CreateFgsApiWebhookCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created API webhook {ApiWebhookId} with name {Name}", result.Id, result.Name);
        return ApiResponse<FgsApiWebhookDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
