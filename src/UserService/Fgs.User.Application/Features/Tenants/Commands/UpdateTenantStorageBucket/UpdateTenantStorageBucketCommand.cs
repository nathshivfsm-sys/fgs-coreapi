using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using MediatR;

namespace Fgs.User.Application.Features.Tenants.Commands.UpdateTenantStorageBucket;

public sealed record UpdateTenantStorageBucketCommand(long TenantId, UpdateTenantStorageBucketRequest Request)
    : IRequest<ApiResponse<object>>;
