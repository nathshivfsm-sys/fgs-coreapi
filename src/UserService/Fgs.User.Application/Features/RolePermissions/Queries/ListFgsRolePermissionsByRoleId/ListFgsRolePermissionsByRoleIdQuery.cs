using Fgs.Contracts.Api;
using Fgs.User.Application.Features.RolePermissions.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.RolePermissions.Queries.ListFgsRolePermissionsByRoleId;

public sealed record ListFgsRolePermissionsByRoleIdQuery(long FgsRoleId)
    : IRequest<ApiResponse<IReadOnlyList<FgsRolePermissionDetailDto>>>;
