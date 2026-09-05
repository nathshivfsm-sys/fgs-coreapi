using Fgs.Contracts.Api;
using Fgs.User.Application.Features.RoleMenus.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.RoleMenus.Commands.CreateFgsRoleMenu;

public sealed record CreateFgsRoleMenuCommand(FgsRoleMenuCreateDto Dto)
    : IRequest<ApiResponse<FgsRoleMenuDetailDto>>;
