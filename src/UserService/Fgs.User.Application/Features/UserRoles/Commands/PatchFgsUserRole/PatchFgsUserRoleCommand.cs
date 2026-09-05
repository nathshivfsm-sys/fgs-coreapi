using Fgs.Contracts.Api;
using Fgs.User.Application.Features.UserRoles.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.UserRoles.Commands.PatchFgsUserRole;

public sealed record PatchFgsUserRoleCommand(long Id, FgsUserRolePatchDto Dto)
    : IRequest<ApiResponse<FgsUserRoleDetailDto>>;
