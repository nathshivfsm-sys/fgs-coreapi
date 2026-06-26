using Fgs.Contracts.Api;
using Fgs.Contracts.Health;
using Fgs.Foundation.Api;
using MediatR;

namespace Fgs.Foundation.Health;

public sealed record GetServiceHealthQuery : IRequest<ApiResponse<ServiceHealthDto>>;

public sealed class GetServiceHealthQueryHandler(string serviceName)
    : IRequestHandler<GetServiceHealthQuery, ApiResponse<ServiceHealthDto>>
{
    public Task<ApiResponse<ServiceHealthDto>> Handle(
        GetServiceHealthQuery request,
        CancellationToken cancellationToken) =>
        Task.FromResult(ApiResponse<ServiceHealthDto>.Ok(
            new ServiceHealthDto(serviceName, "healthy", FgsApiVersions.V1)));
}
