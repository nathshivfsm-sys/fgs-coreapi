using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.PublicEndpoints;
using Fgs.User.Application.Features.PublicEndpoints.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.PublicEndpoints.Commands.CreateFgsPublicEndpoint;

public sealed class CreateFgsPublicEndpointCommandHandler(
    IFgsPublicEndpointWriteService writeService,
    ILogger<CreateFgsPublicEndpointCommandHandler> logger)
    : IRequestHandler<CreateFgsPublicEndpointCommand, ApiResponse<FgsPublicEndpointDetailDto>>
{
    public async Task<ApiResponse<FgsPublicEndpointDetailDto>> Handle(
        CreateFgsPublicEndpointCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation(
            "Created public endpoint {PublicEndpointId} ({EndpointType}/{EnvironmentCode})",
            result.Id,
            result.EndpointType,
            result.EnvironmentCode);
        return ApiResponse<FgsPublicEndpointDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
