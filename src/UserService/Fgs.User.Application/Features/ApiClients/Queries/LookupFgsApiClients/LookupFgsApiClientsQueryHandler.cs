using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.ApiClients;
using Fgs.User.Application.Features.ApiClients.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ApiClients.Queries.LookupFgsApiClients;

public sealed class LookupFgsApiClientsQueryHandler(IFgsApiClientReadRepository readRepository)
    : IRequestHandler<LookupFgsApiClientsQuery, ApiResponse<IReadOnlyList<FgsApiClientLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsApiClientLookupDto>>> Handle(
        LookupFgsApiClientsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
        return ApiResponse<IReadOnlyList<FgsApiClientLookupDto>>.Ok(result);
    }
}
