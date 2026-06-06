using Fgs.Contracts.Api;
using Fgs.Contracts.Health;
using Fgs.Foundation.Api;
using MediatR;

namespace Fgs.Integration.Application.Features.Health.Queries.GetServiceHealth;

public sealed class GetServiceHealthQueryHandler : IRequestHandler<GetServiceHealthQuery, ApiResponse<ServiceHealthDto>>
{
    public Task<ApiResponse<ServiceHealthDto>> Handle(GetServiceHealthQuery request, CancellationToken cancellationToken) =>
        Task.FromResult(ApiResponse<ServiceHealthDto>.Ok(
            new ServiceHealthDto("Fgs.Integration", "healthy", FgsApiVersions.V1)));
}
