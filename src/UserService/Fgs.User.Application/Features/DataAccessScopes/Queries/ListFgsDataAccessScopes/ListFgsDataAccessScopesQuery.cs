using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.DataAccessScopes.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.DataAccessScopes.Queries.ListFgsDataAccessScopes;

public sealed record ListFgsDataAccessScopesQuery(
    IdentityListQuery Query,
    FgsDataAccessScopeListFilters Filters) : IRequest<ApiResponse<PagedResult<FgsDataAccessScopeSummaryDto>>>;
