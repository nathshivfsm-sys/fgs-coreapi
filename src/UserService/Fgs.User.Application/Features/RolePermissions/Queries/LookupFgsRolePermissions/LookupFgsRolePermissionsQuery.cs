using Fgs.Contracts.Api;
using Fgs.User.Application.Features.RolePermissions.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.RolePermissions.Queries.LookupFgsRolePermissions;

public sealed record LookupFgsRolePermissionsQuery(long FgsRoleId)
    : IRequest<ApiResponse<IReadOnlyList<FgsRolePermissionLookupDto>>>;
