using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.User.Application.Features.Users.Queries.GetUserIdsByRoles;

public sealed record GetUserIdsByRolesQuery(IReadOnlyList<long> RoleIds)
    : IRequest<ApiResponse<IReadOnlyList<Guid>>>;
