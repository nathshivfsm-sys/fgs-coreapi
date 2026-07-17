using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.ApiSecrets;
using Fgs.User.Application.Features.ApiSecrets.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ApiSecrets.Queries.GetFgsApiSecretById;

public sealed class GetFgsApiSecretByIdQueryHandler(IFgsApiSecretReadRepository readRepository)
    : IRequestHandler<GetFgsApiSecretByIdQuery, ApiResponse<FgsApiSecretDetailDto>>
{
    public async Task<ApiResponse<FgsApiSecretDetailDto>> Handle(
        GetFgsApiSecretByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsApiSecretDetailDto>.Fail(
                [$"API secret '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        return ApiResponse<FgsApiSecretDetailDto>.Ok(result);
    }
}
