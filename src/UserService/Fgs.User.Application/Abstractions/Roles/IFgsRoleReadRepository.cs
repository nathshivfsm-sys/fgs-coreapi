using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.Roles.Dtos;

namespace Fgs.User.Application.Abstractions.Roles;

public interface IFgsRoleReadRepository
{
    Task<FgsRoleDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsRoleSummaryDto>> ListAsync(
        IdentityListQuery query,
        FgsRoleListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsRoleLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByRoleCodeAsync(
        string roleCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default);

    Task<bool> HasActiveUserAssignmentsAsync(long roleId, CancellationToken cancellationToken = default);

    /// <summary>
    /// True when another active (non-deleted) user in the current tenant/company already has the role code.
    /// </summary>
    Task<bool> HasOtherActiveUserWithRoleCodeAsync(
        string roleCode,
        Guid? excludeUserId = null,
        CancellationToken cancellationToken = default);
}
