using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Abstractions.Permissions;
using Fgs.User.Application.Features.Permissions.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Permissions.Queries.ListFgsPermissions;

public sealed class ListFgsPermissionsQueryHandler(IFgsPermissionReadRepository readRepository)
    : IRequestHandler<ListFgsPermissionsQuery, ApiResponse<PagedResult<FgsPermissionSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsPermissionSummaryDto>>> Handle(
        ListFgsPermissionsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsPermissionSummaryDto>>.Ok(result);
    }
}
