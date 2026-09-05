using Fgs.Contracts.Api;
using Fgs.User.Application.Features.RoleMenus.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.RoleMenus.Commands.PatchFgsRoleMenu;

public sealed record PatchFgsRoleMenuCommand(long Id, FgsRoleMenuPatchDto Dto)
    : IRequest<ApiResponse<FgsRoleMenuDetailDto>>;
