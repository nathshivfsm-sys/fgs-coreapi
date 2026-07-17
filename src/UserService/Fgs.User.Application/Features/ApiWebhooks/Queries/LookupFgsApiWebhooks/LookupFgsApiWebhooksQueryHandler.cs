using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.ApiWebhooks;
using Fgs.User.Application.Features.ApiWebhooks.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ApiWebhooks.Queries.LookupFgsApiWebhooks;

public sealed class LookupFgsApiWebhooksQueryHandler(IFgsApiWebhookReadRepository readRepository)
    : IRequestHandler<LookupFgsApiWebhooksQuery, ApiResponse<IReadOnlyList<FgsApiWebhookLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsApiWebhookLookupDto>>> Handle(
        LookupFgsApiWebhooksQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
        return ApiResponse<IReadOnlyList<FgsApiWebhookLookupDto>>.Ok(result);
    }
}
