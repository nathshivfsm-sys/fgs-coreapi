using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.ApiClients;
using Fgs.User.Application.Features.ApiClients.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.ApiClients.Commands.UpdateFgsApiClient;

public sealed class UpdateFgsApiClientCommandHandler(
    IFgsApiClientWriteService writeService,
    ILogger<UpdateFgsApiClientCommandHandler> logger)
    : IRequestHandler<UpdateFgsApiClientCommand, ApiResponse<FgsApiClientDetailDto>>
{
    public async Task<ApiResponse<FgsApiClientDetailDto>> Handle(
        UpdateFgsApiClientCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated API client {ApiClientId}", result.Id);
        return ApiResponse<FgsApiClientDetailDto>.Ok(result);
    }
}
