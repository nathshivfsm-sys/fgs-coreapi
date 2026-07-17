using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.Permissions.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Permissions.Queries.ListFgsPermissions;

public sealed record ListFgsPermissionsQuery(
    IdentityListQuery Query,
    FgsPermissionListFilters Filters) : IRequest<ApiResponse<PagedResult<FgsPermissionSummaryDto>>>;
