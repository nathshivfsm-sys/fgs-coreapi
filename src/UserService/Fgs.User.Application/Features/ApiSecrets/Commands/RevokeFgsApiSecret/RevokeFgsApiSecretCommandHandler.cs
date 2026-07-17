using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.ApiSecrets;
using Fgs.User.Application.Features.ApiSecrets.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.ApiSecrets.Commands.RevokeFgsApiSecret;

public sealed class RevokeFgsApiSecretCommandHandler(
    IFgsApiSecretWriteService writeService,
    ILogger<RevokeFgsApiSecretCommandHandler> logger)
    : IRequestHandler<RevokeFgsApiSecretCommand, ApiResponse<FgsApiSecretDetailDto>>
{
    public async Task<ApiResponse<FgsApiSecretDetailDto>> Handle(
        RevokeFgsApiSecretCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.RevokeAsync(request.Id, cancellationToken);
        logger.LogInformation("Revoked API secret {ApiSecretId}", result.Id);
        return ApiResponse<FgsApiSecretDetailDto>.Ok(result);
    }
}
