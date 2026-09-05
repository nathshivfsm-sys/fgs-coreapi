using Fgs.Contracts.Api;
using Fgs.User.Application.Features.RolePermissions.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.RolePermissions.Queries.GetFgsRolePermissionById;

public sealed record GetFgsRolePermissionByIdQuery(long Id) : IRequest<ApiResponse<FgsRolePermissionDetailDto>>;
