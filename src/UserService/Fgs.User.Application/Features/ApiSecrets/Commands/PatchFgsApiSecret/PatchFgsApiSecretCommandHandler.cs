using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.ApiSecrets;
using Fgs.User.Application.Features.ApiSecrets.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.ApiSecrets.Commands.PatchFgsApiSecret;

public sealed class PatchFgsApiSecretCommandHandler(
    IFgsApiSecretWriteService writeService,
    ILogger<PatchFgsApiSecretCommandHandler> logger)
    : IRequestHandler<PatchFgsApiSecretCommand, ApiResponse<FgsApiSecretDetailDto>>
{
    public async Task<ApiResponse<FgsApiSecretDetailDto>> Handle(
        PatchFgsApiSecretCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patched API secret {ApiSecretId}", result.Id);
        return ApiResponse<FgsApiSecretDetailDto>.Ok(result);
    }
}
