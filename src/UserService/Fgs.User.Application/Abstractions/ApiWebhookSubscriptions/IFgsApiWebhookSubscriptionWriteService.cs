using Fgs.User.Application.Features.ApiWebhookSubscriptions.Dtos;

namespace Fgs.User.Application.Abstractions.ApiWebhookSubscriptions;

public interface IFgsApiWebhookSubscriptionWriteService
{
    Task<FgsApiWebhookSubscriptionDetailDto> CreateAsync(
        FgsApiWebhookSubscriptionCreateDto dto,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}
