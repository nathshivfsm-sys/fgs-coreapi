using Fgs.Contracts.Api;
using Fgs.User.Application.Features.UserRoles.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.UserRoles.Queries.LookupFgsUserRoles;

public sealed record LookupFgsUserRolesQuery(Guid UserId)
    : IRequest<ApiResponse<IReadOnlyList<FgsUserRoleLookupDto>>>;
