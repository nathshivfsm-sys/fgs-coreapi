using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using MediatR;

namespace Fgs.User.Application.Features.Tenants.Commands.UpdateTenantStatus;

public sealed record UpdateTenantStatusCommand(long TenantId, UpdateTenantStatusRequest Request)
    : IRequest<ApiResponse<object>>;
