using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.PublicEndpoints;
using Fgs.User.Application.Features.PublicEndpoints.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.PublicEndpoints.Queries.LookupFgsPublicEndpoints;

public sealed class LookupFgsPublicEndpointsQueryHandler(IFgsPublicEndpointReadRepository readRepository)
    : IRequestHandler<LookupFgsPublicEndpointsQuery, ApiResponse<IReadOnlyList<FgsPublicEndpointLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsPublicEndpointLookupDto>>> Handle(
        LookupFgsPublicEndpointsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
        return ApiResponse<IReadOnlyList<FgsPublicEndpointLookupDto>>.Ok(result);
    }
}
