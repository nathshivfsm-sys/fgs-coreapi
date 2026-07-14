using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.ApiWebhooks.Dtos;

namespace Fgs.User.Application.Abstractions.ApiWebhooks;

public interface IFgsApiWebhookReadRepository
{
    Task<FgsApiWebhookDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsApiWebhookSummaryDto>> ListAsync(
        IdentityListQuery query,
        FgsApiWebhookListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsApiWebhookLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);
}
