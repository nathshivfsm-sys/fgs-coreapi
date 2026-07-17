using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.PublicEndpoints;
using Fgs.User.Application.Features.PublicEndpoints.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.PublicEndpoints.Queries.GetFgsPublicEndpointById;

public sealed class GetFgsPublicEndpointByIdQueryHandler(IFgsPublicEndpointReadRepository readRepository)
    : IRequestHandler<GetFgsPublicEndpointByIdQuery, ApiResponse<FgsPublicEndpointDetailDto>>
{
    public async Task<ApiResponse<FgsPublicEndpointDetailDto>> Handle(
        GetFgsPublicEndpointByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsPublicEndpointDetailDto>.Fail(
                [$"Public endpoint '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        return ApiResponse<FgsPublicEndpointDetailDto>.Ok(result);
    }
}
