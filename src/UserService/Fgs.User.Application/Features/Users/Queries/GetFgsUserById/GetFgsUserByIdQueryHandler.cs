using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.Users;
using Fgs.User.Application.Features.Users.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Users.Queries.GetFgsUserById;

public sealed class GetFgsUserByIdQueryHandler(IFgsUserReadRepository readRepository)
    : IRequestHandler<GetFgsUserByIdQuery, ApiResponse<FgsUserDetailDto>>
{
    public async Task<ApiResponse<FgsUserDetailDto>> Handle(
        GetFgsUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsUserDetailDto>.Fail(
                [$"User '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        return ApiResponse<FgsUserDetailDto>.Ok(result);
    }
}
