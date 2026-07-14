using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.ApiWebhookSubscriptions;
using Fgs.User.Application.Features.ApiWebhookSubscriptions.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ApiWebhookSubscriptions.Queries.GetFgsApiWebhookSubscriptionById;

public sealed class GetFgsApiWebhookSubscriptionByIdQueryHandler(
    IFgsApiWebhookSubscriptionReadRepository readRepository)
    : IRequestHandler<GetFgsApiWebhookSubscriptionByIdQuery, ApiResponse<FgsApiWebhookSubscriptionDetailDto>>
{
    public async Task<ApiResponse<FgsApiWebhookSubscriptionDetailDto>> Handle(
        GetFgsApiWebhookSubscriptionByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsApiWebhookSubscriptionDetailDto>.Fail(
                [$"API webhook subscription '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        return ApiResponse<FgsApiWebhookSubscriptionDetailDto>.Ok(result);
    }
}
