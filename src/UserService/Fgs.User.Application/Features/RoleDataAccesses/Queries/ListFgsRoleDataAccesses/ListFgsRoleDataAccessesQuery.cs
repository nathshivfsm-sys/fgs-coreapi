using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.RoleDataAccesses.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.RoleDataAccesses.Queries.ListFgsRoleDataAccesses;

public sealed record ListFgsRoleDataAccessesQuery(
    IdentityListQuery Query,
    FgsRoleDataAccessListFilters Filters) : IRequest<ApiResponse<PagedResult<FgsRoleDataAccessSummaryDto>>>;
