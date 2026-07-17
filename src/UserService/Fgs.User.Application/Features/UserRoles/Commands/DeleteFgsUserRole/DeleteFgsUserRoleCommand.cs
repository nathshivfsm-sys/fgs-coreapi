using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.User.Application.Features.UserRoles.Commands.DeleteFgsUserRole;

public sealed record DeleteFgsUserRoleCommand(long Id) : IRequest<ApiResponse<object>>;
