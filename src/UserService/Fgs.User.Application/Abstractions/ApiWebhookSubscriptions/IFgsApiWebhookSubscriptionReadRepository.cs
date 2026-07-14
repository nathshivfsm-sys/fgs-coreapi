using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.ApiWebhookSubscriptions.Dtos;

namespace Fgs.User.Application.Abstractions.ApiWebhookSubscriptions;

public interface IFgsApiWebhookSubscriptionReadRepository
{
    Task<FgsApiWebhookSubscriptionDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsApiWebhookSubscriptionSummaryDto>> ListAsync(
        IdentityListQuery query,
        FgsApiWebhookSubscriptionListFilters filters,
        CancellationToken cancellationToken = default);
}
