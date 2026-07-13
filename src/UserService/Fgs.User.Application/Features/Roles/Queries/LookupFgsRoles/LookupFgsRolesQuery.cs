using Fgs.Contracts.Api;
using Fgs.User.Application.Features.Roles.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Roles.Queries.LookupFgsRoles;

public sealed record LookupFgsRolesQuery(bool ActiveOnly = true) : IRequest<ApiResponse<IReadOnlyList<FgsRoleLookupDto>>>;
