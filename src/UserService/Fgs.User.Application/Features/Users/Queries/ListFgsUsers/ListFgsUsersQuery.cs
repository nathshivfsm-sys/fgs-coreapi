using Fgs.Contracts.Api;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.Users.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Users.Queries.ListFgsUsers;

/// <param name="IncludeSummary">
/// When true (default), loads company-scoped card counts. When false, skips COUNT queries and returns zeros.
/// </param>
public sealed record ListFgsUsersQuery(
    IdentityListQuery Query,
    FgsUserListFilters Filters,
    bool IncludeSummary = true) : IRequest<ApiResponse<FgsUserListResultDto>>;
