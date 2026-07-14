using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.ApiSecrets.Dtos;

namespace Fgs.User.Application.Abstractions.ApiSecrets;

public interface IFgsApiSecretReadRepository
{
    Task<FgsApiSecretDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsApiSecretSummaryDto>> ListAsync(
        IdentityListQuery query,
        FgsApiSecretListFilters filters,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByNameAsync(
        long fgsApiClientId,
        string name,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}
