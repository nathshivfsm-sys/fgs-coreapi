using Fgs.Contracts.Api;
using Fgs.User.Application.Features.RoleMenus.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.RoleMenus.Commands.UpdateFgsRoleMenu;

public sealed record UpdateFgsRoleMenuCommand(long Id, FgsRoleMenuUpdateDto Dto)
    : IRequest<ApiResponse<FgsRoleMenuDetailDto>>;
