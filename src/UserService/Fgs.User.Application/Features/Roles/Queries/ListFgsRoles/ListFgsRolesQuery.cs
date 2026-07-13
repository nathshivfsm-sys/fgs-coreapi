using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.Roles.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Roles.Queries.ListFgsRoles;

public sealed record ListFgsRolesQuery(
    IdentityListQuery Query,
    FgsRoleListFilters Filters) : IRequest<ApiResponse<PagedResult<FgsRoleSummaryDto>>>;
