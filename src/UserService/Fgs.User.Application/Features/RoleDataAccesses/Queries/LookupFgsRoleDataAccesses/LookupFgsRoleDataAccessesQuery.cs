using Fgs.Contracts.Api;
using Fgs.User.Application.Features.RoleDataAccesses.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.RoleDataAccesses.Queries.LookupFgsRoleDataAccesses;

public sealed record LookupFgsRoleDataAccessesQuery(long FgsRoleId)
    : IRequest<ApiResponse<IReadOnlyList<FgsRoleDataAccessLookupDto>>>;
