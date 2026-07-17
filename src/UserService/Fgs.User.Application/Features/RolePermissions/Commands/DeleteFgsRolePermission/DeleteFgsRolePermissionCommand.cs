using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.User.Application.Features.RolePermissions.Commands.DeleteFgsRolePermission;

public sealed record DeleteFgsRolePermissionCommand(long Id) : IRequest<ApiResponse<object>>;
