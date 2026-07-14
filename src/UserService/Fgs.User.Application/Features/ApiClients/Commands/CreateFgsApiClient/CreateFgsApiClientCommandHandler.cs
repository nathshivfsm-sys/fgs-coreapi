using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.ApiClients;
using Fgs.User.Application.Features.ApiClients.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.ApiClients.Commands.CreateFgsApiClient;

public sealed class CreateFgsApiClientCommandHandler(
    IFgsApiClientWriteService writeService,
    ILogger<CreateFgsApiClientCommandHandler> logger)
    : IRequestHandler<CreateFgsApiClientCommand, ApiResponse<FgsApiClientDetailDto>>
{
    public async Task<ApiResponse<FgsApiClientDetailDto>> Handle(
        CreateFgsApiClientCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation(
            "Created API client {ApiClientId} with client id {ClientId}",
            result.Id,
            result.ClientId);
        return ApiResponse<FgsApiClientDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
