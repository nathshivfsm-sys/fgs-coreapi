using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.Users.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Users.Queries.ListFgsUsers;

public sealed record ListFgsUsersQuery(
    IdentityListQuery Query,
    FgsUserListFilters Filters) : IRequest<ApiResponse<PagedResult<FgsUserSummaryDto>>>;
