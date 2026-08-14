using Fgs.Contracts.Api;
using Fgs.User.Application.Features.UserRoles.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.UserRoles.Commands.SyncFgsUserRoles;

public sealed record SyncFgsUserRolesCommand(FgsUserRoleSyncDto Dto)
    : IRequest<ApiResponse<IReadOnlyList<FgsUserRoleDetailDto>>>;
