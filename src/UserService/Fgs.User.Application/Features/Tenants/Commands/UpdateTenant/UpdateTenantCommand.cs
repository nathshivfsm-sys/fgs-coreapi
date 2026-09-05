using Fgs.Contracts.Api;
using Fgs.User.Application.Features.Tenants.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Tenants.Commands.UpdateTenant;

public sealed record UpdateTenantCommand(long TenantId, TenantUpdateDto Dto)
    : IRequest<ApiResponse<TenantDetailDto>>;
