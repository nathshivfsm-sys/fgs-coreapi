using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.ApiClients;
using Fgs.User.Application.Features.ApiClients.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.ApiClients.Commands.PatchFgsApiClient;

public sealed class PatchFgsApiClientCommandHandler(
    IFgsApiClientWriteService writeService,
    ILogger<PatchFgsApiClientCommandHandler> logger)
    : IRequestHandler<PatchFgsApiClientCommand, ApiResponse<FgsApiClientDetailDto>>
{
    public async Task<ApiResponse<FgsApiClientDetailDto>> Handle(
        PatchFgsApiClientCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patched API client {ApiClientId}", result.Id);
        return ApiResponse<FgsApiClientDetailDto>.Ok(result);
    }
}
