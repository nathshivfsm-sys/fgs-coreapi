using Fgs.Contracts.Api;
using Fgs.User.Application.Features.UserRoles.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.UserRoles.Commands.UpdateFgsUserRole;

public sealed record UpdateFgsUserRoleCommand(long Id, FgsUserRoleUpdateDto Dto)
    : IRequest<ApiResponse<FgsUserRoleDetailDto>>;
