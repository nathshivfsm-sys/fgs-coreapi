using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.DataAccessScopes;
using Fgs.User.Application.Features.DataAccessScopes.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.DataAccessScopes.Queries.GetFgsDataAccessScopeById;

public sealed class GetFgsDataAccessScopeByIdQueryHandler(IFgsDataAccessScopeReadRepository readRepository)
    : IRequestHandler<GetFgsDataAccessScopeByIdQuery, ApiResponse<FgsDataAccessScopeDetailDto>>
{
    public async Task<ApiResponse<FgsDataAccessScopeDetailDto>> Handle(
        GetFgsDataAccessScopeByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsDataAccessScopeDetailDto>.Fail(
                [$"Data access scope '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        return ApiResponse<FgsDataAccessScopeDetailDto>.Ok(result);
    }
}
