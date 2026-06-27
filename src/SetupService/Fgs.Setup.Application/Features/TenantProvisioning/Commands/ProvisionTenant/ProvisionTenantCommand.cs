using Fgs.Contracts.Api;
using Fgs.Contracts.Requests;
using MediatR;

namespace Fgs.Setup.Application.Features.TenantProvisioning.Commands.ProvisionTenant;

public sealed record ProvisionTenantCommand(
    ProvisionTenantRequest Request,
    string? InternalServiceKey = null,
    string? RequestingServiceName = null)
    : IRequest<ApiResponse<object>>;
