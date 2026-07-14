using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.Permissions.Dtos;

namespace Fgs.User.Application.Abstractions.Permissions;

public interface IFgsPermissionReadRepository
{
    Task<FgsPermissionDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsPermissionSummaryDto>> ListAsync(
        IdentityListQuery query,
        FgsPermissionListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsPermissionLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByPermissionCodeAsync(
        string permissionCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}
