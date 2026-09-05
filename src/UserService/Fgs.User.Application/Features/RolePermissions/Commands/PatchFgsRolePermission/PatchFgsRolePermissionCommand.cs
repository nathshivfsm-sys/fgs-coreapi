using Fgs.Contracts.Api;
using Fgs.User.Application.Features.RolePermissions.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.RolePermissions.Commands.PatchFgsRolePermission;

public sealed record PatchFgsRolePermissionCommand(long Id, FgsRolePermissionPatchDto Dto)
    : IRequest<ApiResponse<FgsRolePermissionDetailDto>>;
