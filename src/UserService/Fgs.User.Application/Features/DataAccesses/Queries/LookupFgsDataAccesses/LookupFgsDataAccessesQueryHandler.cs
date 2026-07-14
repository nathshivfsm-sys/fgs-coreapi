using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.DataAccesses;
using Fgs.User.Application.Features.DataAccesses.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.DataAccesses.Queries.LookupFgsDataAccesses;

public sealed class LookupFgsDataAccessesQueryHandler(IFgsDataAccessReadRepository readRepository)
    : IRequestHandler<LookupFgsDataAccessesQuery, ApiResponse<IReadOnlyList<FgsDataAccessLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsDataAccessLookupDto>>> Handle(
        LookupFgsDataAccessesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
        return ApiResponse<IReadOnlyList<FgsDataAccessLookupDto>>.Ok(result);
    }
}
