using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.Permissions;
using Fgs.User.Application.Features.Permissions.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Permissions.Queries.LookupFgsPermissions;

public sealed class LookupFgsPermissionsQueryHandler(IFgsPermissionReadRepository readRepository)
    : IRequestHandler<LookupFgsPermissionsQuery, ApiResponse<IReadOnlyList<FgsPermissionLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsPermissionLookupDto>>> Handle(
        LookupFgsPermissionsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
        return ApiResponse<IReadOnlyList<FgsPermissionLookupDto>>.Ok(result);
    }
}
