using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.PublicEndpoints;
using Fgs.User.Application.Features.PublicEndpoints.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.PublicEndpoints.Commands.UpdateFgsPublicEndpoint;

public sealed class UpdateFgsPublicEndpointCommandHandler(
    IFgsPublicEndpointWriteService writeService,
    ILogger<UpdateFgsPublicEndpointCommandHandler> logger)
    : IRequestHandler<UpdateFgsPublicEndpointCommand, ApiResponse<FgsPublicEndpointDetailDto>>
{
    public async Task<ApiResponse<FgsPublicEndpointDetailDto>> Handle(
        UpdateFgsPublicEndpointCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated public endpoint {PublicEndpointId}", result.Id);
        return ApiResponse<FgsPublicEndpointDetailDto>.Ok(result);
    }
}
