using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.ApiSecrets;
using Fgs.User.Application.Features.ApiSecrets.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.ApiSecrets.Commands.CreateFgsApiSecret;

public sealed class CreateFgsApiSecretCommandHandler(
    IFgsApiSecretWriteService writeService,
    ILogger<CreateFgsApiSecretCommandHandler> logger)
    : IRequestHandler<CreateFgsApiSecretCommand, ApiResponse<FgsApiSecretCreateResultDto>>
{
    public async Task<ApiResponse<FgsApiSecretCreateResultDto>> Handle(
        CreateFgsApiSecretCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation(
            "Created API secret {ApiSecretId} for client {FgsApiClientId}",
            result.Id,
            result.FgsApiClientId);
        return ApiResponse<FgsApiSecretCreateResultDto>.Ok(result, ApiStatusCodes.Created);
    }
}
