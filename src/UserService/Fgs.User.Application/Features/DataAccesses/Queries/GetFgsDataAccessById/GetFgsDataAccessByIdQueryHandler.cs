using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.DataAccesses;
using Fgs.User.Application.Features.DataAccesses.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.DataAccesses.Queries.GetFgsDataAccessById;

public sealed class GetFgsDataAccessByIdQueryHandler(IFgsDataAccessReadRepository readRepository)
    : IRequestHandler<GetFgsDataAccessByIdQuery, ApiResponse<FgsDataAccessDetailDto>>
{
    public async Task<ApiResponse<FgsDataAccessDetailDto>> Handle(
        GetFgsDataAccessByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsDataAccessDetailDto>.Fail(
                [$"Data access '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        return ApiResponse<FgsDataAccessDetailDto>.Ok(result);
    }
}
