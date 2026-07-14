using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.UserRoles.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.UserRoles.Queries.ListFgsUserRoles;

public sealed record ListFgsUserRolesQuery(
    IdentityListQuery Query,
    FgsUserRoleListFilters Filters) : IRequest<ApiResponse<PagedResult<FgsUserRoleSummaryDto>>>;
