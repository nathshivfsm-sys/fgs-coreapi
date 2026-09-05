using Fgs.Contracts.Api;
using Fgs.User.Application.Features.RoleMenus.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.RoleMenus.Queries.LookupFgsRoleMenus;

public sealed record LookupFgsRoleMenusQuery(long RoleId, bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsRoleMenuLookupDto>>>;
