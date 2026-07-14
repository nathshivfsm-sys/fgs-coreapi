using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Abstractions.DataAccessScopes;
using Fgs.User.Application.Features.DataAccessScopes.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.DataAccessScopes.Queries.ListFgsDataAccessScopes;

public sealed class ListFgsDataAccessScopesQueryHandler(IFgsDataAccessScopeReadRepository readRepository)
    : IRequestHandler<ListFgsDataAccessScopesQuery, ApiResponse<PagedResult<FgsDataAccessScopeSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsDataAccessScopeSummaryDto>>> Handle(
        ListFgsDataAccessScopesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsDataAccessScopeSummaryDto>>.Ok(result);
    }
}
