using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Abstractions.Roles;
using Fgs.User.Application.Features.Roles.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Roles.Queries.ListFgsRoles;

public sealed class ListFgsRolesQueryHandler(IFgsRoleReadRepository readRepository)
    : IRequestHandler<ListFgsRolesQuery, ApiResponse<PagedResult<FgsRoleSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsRoleSummaryDto>>> Handle(
        ListFgsRolesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsRoleSummaryDto>>.Ok(result);
    }
}
