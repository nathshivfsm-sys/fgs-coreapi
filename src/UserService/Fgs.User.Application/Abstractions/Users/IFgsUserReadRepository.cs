using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.Users.Dtos;

namespace Fgs.User.Application.Abstractions.Users;

public interface IFgsUserReadRepository
{
    Task<FgsUserDetailDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsUserSummaryDto>> ListAsync(
        IdentityListQuery query,
        FgsUserListFilters filters,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByEmailAsync(
        string email,
        Guid? excludeId = null,
        CancellationToken cancellationToken = default);

    Task<bool> HasAcceptedInvitationAsync(Guid userId, CancellationToken cancellationToken = default);
}
