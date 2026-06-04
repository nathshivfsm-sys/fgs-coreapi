using Fgs.Contracts.Api;
using Fgs.Contracts.Health;
using MediatR;

namespace Fgs.Integration.Application.Features.Health.Queries.GetServiceHealth;

public sealed record GetServiceHealthQuery : IRequest<ApiResponse<ServiceHealthDto>>;
