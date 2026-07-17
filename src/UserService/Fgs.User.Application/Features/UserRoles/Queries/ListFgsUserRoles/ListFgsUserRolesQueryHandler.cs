using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Abstractions.UserRoles;
using Fgs.User.Application.Features.UserRoles.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.UserRoles.Queries.ListFgsUserRoles;

public sealed class ListFgsUserRolesQueryHandler(IFgsUserRoleReadRepository readRepository)
    : IRequestHandler<ListFgsUserRolesQuery, ApiResponse<PagedResult<FgsUserRoleSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsUserRoleSummaryDto>>> Handle(
        ListFgsUserRolesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsUserRoleSummaryDto>>.Ok(result);
    }
}
