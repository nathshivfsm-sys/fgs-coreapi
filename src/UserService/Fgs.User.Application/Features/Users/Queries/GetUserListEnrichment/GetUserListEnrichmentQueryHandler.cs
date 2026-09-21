using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.User.Application.Abstractions.Users;
using MediatR;

namespace Fgs.User.Application.Features.Users.Queries.GetUserListEnrichment;

public sealed class GetUserListEnrichmentQueryHandler(IFgsUserReadRepository readRepository)
    : IRequestHandler<GetUserListEnrichmentQuery, ApiResponse<IReadOnlyList<FgsUserListEnrichmentDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsUserListEnrichmentDto>>> Handle(
        GetUserListEnrichmentQuery request,
        CancellationToken cancellationToken)
    {
        if (request.UserIds is not { Count: > 0 })
        {
            return ApiResponse<IReadOnlyList<FgsUserListEnrichmentDto>>.Ok([]);
        }

        var rows = await readRepository.GetListEnrichmentAsync(request.UserIds, cancellationToken);
        return ApiResponse<IReadOnlyList<FgsUserListEnrichmentDto>>.Ok(rows);
    }
}
