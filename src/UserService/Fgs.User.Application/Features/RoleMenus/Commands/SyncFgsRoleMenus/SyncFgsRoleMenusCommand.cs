using Fgs.Contracts.Api;
using Fgs.User.Application.Features.RoleMenus.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.RoleMenus.Commands.SyncFgsRoleMenus;

public sealed record SyncFgsRoleMenusCommand(FgsRoleMenuSyncDto Dto)
    : IRequest<ApiResponse<IReadOnlyList<FgsRoleMenuDetailDto>>>;
