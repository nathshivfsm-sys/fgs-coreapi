using Fgs.Contracts.Api;
using Fgs.User.Application.Features.RolePermissions.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.RolePermissions.Commands.UpdateFgsRolePermission;

public sealed record UpdateFgsRolePermissionCommand(long Id, FgsRolePermissionUpdateDto Dto)
    : IRequest<ApiResponse<FgsRolePermissionDetailDto>>;
