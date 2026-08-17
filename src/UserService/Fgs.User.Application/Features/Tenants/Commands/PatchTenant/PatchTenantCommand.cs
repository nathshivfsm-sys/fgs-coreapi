using Fgs.Contracts.Api;
using Fgs.User.Application.Features.Tenants.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Tenants.Commands.PatchTenant;

public sealed record PatchTenantCommand(long TenantId, TenantPatchDto Dto)
    : IRequest<ApiResponse<TenantDetailDto>>;
