using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.ApiWebhooks;
using Fgs.User.Application.Features.ApiWebhooks.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ApiWebhooks.Queries.GetFgsApiWebhookById;

public sealed class GetFgsApiWebhookByIdQueryHandler(IFgsApiWebhookReadRepository readRepository)
    : IRequestHandler<GetFgsApiWebhookByIdQuery, ApiResponse<FgsApiWebhookDetailDto>>
{
    public async Task<ApiResponse<FgsApiWebhookDetailDto>> Handle(
        GetFgsApiWebhookByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsApiWebhookDetailDto>.Fail(
                [$"API webhook '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        return ApiResponse<FgsApiWebhookDetailDto>.Ok(result);
    }
}
