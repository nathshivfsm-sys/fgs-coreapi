using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.ApiClients;
using Fgs.User.Application.Features.ApiClients.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ApiClients.Queries.GetFgsApiClientById;

public sealed class GetFgsApiClientByIdQueryHandler(IFgsApiClientReadRepository readRepository)
    : IRequestHandler<GetFgsApiClientByIdQuery, ApiResponse<FgsApiClientDetailDto>>
{
    public async Task<ApiResponse<FgsApiClientDetailDto>> Handle(
        GetFgsApiClientByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsApiClientDetailDto>.Fail(
                [$"API client '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        return ApiResponse<FgsApiClientDetailDto>.Ok(result);
    }
}
