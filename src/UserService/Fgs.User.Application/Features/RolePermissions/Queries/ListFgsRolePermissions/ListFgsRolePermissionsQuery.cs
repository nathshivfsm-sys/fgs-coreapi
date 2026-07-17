using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.RolePermissions.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.RolePermissions.Queries.ListFgsRolePermissions;

public sealed record ListFgsRolePermissionsQuery(
    IdentityListQuery Query,
    FgsRolePermissionListFilters Filters) : IRequest<ApiResponse<PagedResult<FgsRolePermissionSummaryDto>>>;
