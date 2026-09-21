using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.Users.Dtos;

namespace Fgs.User.Application.Abstractions.Users;

public interface IFgsUserReadRepository
{
    Task<FgsUserDetailDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <param name="includeSummary">
    /// When true, runs company-scoped summary COUNTs (ignores list filters). When false, returns zeroed summary.
    /// </param>
    Task<FgsUserListResultDto> ListAsync(
        IdentityListQuery query,
        FgsUserListFilters filters,
        bool includeSummary = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByEmailAsync(
        string email,
        Guid? excludeId = null,
        CancellationToken cancellationToken = default);

    Task<bool> HasAcceptedInvitationAsync(Guid userId, CancellationToken cancellationToken = default);
}
