using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.ApiEvents;
using Fgs.User.Application.Features.ApiEvents.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ApiEvents.Queries.GetFgsApiEventById;

public sealed class GetFgsApiEventByIdQueryHandler(IFgsApiEventReadRepository readRepository)
    : IRequestHandler<GetFgsApiEventByIdQuery, ApiResponse<FgsApiEventDetailDto>>
{
    public async Task<ApiResponse<FgsApiEventDetailDto>> Handle(
        GetFgsApiEventByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsApiEventDetailDto>.Fail(
                [$"API event '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        return ApiResponse<FgsApiEventDetailDto>.Ok(result);
    }
}
