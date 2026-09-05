using Fgs.Contracts.Api;
using Fgs.User.Application.Features.RoleMenus.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.RoleMenus.Queries.GetFgsRoleMenuById;

public sealed record GetFgsRoleMenuByIdQuery(long Id) : IRequest<ApiResponse<FgsRoleMenuDetailDto>>;
