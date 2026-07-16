using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.PublicEndpoints;
using Fgs.User.Application.Features.PublicEndpoints.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.PublicEndpoints.Commands.PatchFgsPublicEndpoint;

public sealed class PatchFgsPublicEndpointCommandHandler(
    IFgsPublicEndpointWriteService writeService,
    ILogger<PatchFgsPublicEndpointCommandHandler> logger)
    : IRequestHandler<PatchFgsPublicEndpointCommand, ApiResponse<FgsPublicEndpointDetailDto>>
{
    public async Task<ApiResponse<FgsPublicEndpointDetailDto>> Handle(
        PatchFgsPublicEndpointCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patched public endpoint {PublicEndpointId}", result.Id);
        return ApiResponse<FgsPublicEndpointDetailDto>.Ok(result);
    }
}
