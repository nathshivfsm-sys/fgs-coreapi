using Fgs.Contracts.Api;
using Fgs.User.Application.Features.RoleMenus.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.RoleMenus.Queries.ListFgsRoleMenusByRoleId;

public sealed record ListFgsRoleMenusByRoleIdQuery(long RoleId)
    : IRequest<ApiResponse<IReadOnlyList<FgsRoleMenuDetailDto>>>;
