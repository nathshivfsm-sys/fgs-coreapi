using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Abstractions.RoleDataAccesses;
using Fgs.User.Application.Features.RoleDataAccesses.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.RoleDataAccesses.Queries.ListFgsRoleDataAccesses;

public sealed class ListFgsRoleDataAccessesQueryHandler(IFgsRoleDataAccessReadRepository readRepository)
    : IRequestHandler<ListFgsRoleDataAccessesQuery, ApiResponse<PagedResult<FgsRoleDataAccessSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsRoleDataAccessSummaryDto>>> Handle(
        ListFgsRoleDataAccessesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsRoleDataAccessSummaryDto>>.Ok(result);
    }
}
