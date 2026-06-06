using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using MediatR;

namespace Fgs.User.Application.Features.Tenants.Queries.GetTenant;

public sealed record GetTenantQuery(long TenantId) : IRequest<ApiResponse<TenantDto>>;
