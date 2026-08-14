using Fgs.Contracts.Api;
using Fgs.User.Application.Features.RolePermissions.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.RolePermissions.Commands.SyncFgsRolePermissions;

public sealed record SyncFgsRolePermissionsCommand(FgsRolePermissionSyncDto Dto)
    : IRequest<ApiResponse<IReadOnlyList<FgsRolePermissionDetailDto>>>;
