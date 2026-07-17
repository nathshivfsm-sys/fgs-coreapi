using Fgs.User.Application.Features.ApiWebhooks.Dtos;

namespace Fgs.User.Application.Abstractions.ApiWebhooks;

public interface IFgsApiWebhookWriteService
{
    Task<FgsApiWebhookDetailDto> CreateAsync(FgsApiWebhookCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsApiWebhookDetailDto> UpdateAsync(long id, FgsApiWebhookUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsApiWebhookDetailDto> PatchAsync(long id, FgsApiWebhookPatchDto dto, CancellationToken cancellationToken = default);
}
