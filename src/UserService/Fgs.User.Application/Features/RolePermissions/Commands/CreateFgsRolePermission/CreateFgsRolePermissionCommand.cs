using Fgs.Contracts.Api;
using Fgs.User.Application.Features.RolePermissions.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.RolePermissions.Commands.CreateFgsRolePermission;

public sealed record CreateFgsRolePermissionCommand(FgsRolePermissionCreateDto Dto)
    : IRequest<ApiResponse<FgsRolePermissionDetailDto>>;
