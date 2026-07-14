using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.User.Application.Features.RoleDataAccesses.Commands.DeleteFgsRoleDataAccess;

public sealed record DeleteFgsRoleDataAccessCommand(long Id) : IRequest<ApiResponse<object>>;
