using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.ApiEvents;
using Fgs.User.Application.Features.ApiEvents.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ApiEvents.Queries.LookupFgsApiEvents;

public sealed class LookupFgsApiEventsQueryHandler(IFgsApiEventReadRepository readRepository)
    : IRequestHandler<LookupFgsApiEventsQuery, ApiResponse<IReadOnlyList<FgsApiEventLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsApiEventLookupDto>>> Handle(
        LookupFgsApiEventsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
        return ApiResponse<IReadOnlyList<FgsApiEventLookupDto>>.Ok(result);
    }
}
