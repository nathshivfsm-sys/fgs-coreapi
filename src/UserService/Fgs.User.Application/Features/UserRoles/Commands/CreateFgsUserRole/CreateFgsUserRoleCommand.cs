using Fgs.Contracts.Api;
using Fgs.User.Application.Features.UserRoles.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.UserRoles.Commands.CreateFgsUserRole;

public sealed record CreateFgsUserRoleCommand(FgsUserRoleCreateDto Dto)
    : IRequest<ApiResponse<FgsUserRoleDetailDto>>;
