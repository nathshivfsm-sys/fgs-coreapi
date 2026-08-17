using Fgs.Contracts.Api;
using Fgs.User.Application.Features.RoleDataAccesses.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.RoleDataAccesses.Queries.ListFgsRoleDataAccessesByRoleId;

public sealed record ListFgsRoleDataAccessesByRoleIdQuery(long FgsRoleId)
    : IRequest<ApiResponse<IReadOnlyList<FgsRoleDataAccessDetailDto>>>;
