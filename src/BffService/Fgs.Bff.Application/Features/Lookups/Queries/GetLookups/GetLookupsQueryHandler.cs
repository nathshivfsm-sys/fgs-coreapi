using Fgs.Bff.Application.Features.Lookups.Abstractions;
using Fgs.Bff.Application.Features.Lookups.Dtos;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Bff.Application.Features.Lookups.Queries.GetLookups;

public sealed class GetLookupsQueryHandler(ILookupGateway gateway)
    : IRequestHandler<GetLookupsQuery, ApiResponse<IReadOnlyList<LookupResultDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<LookupResultDto>>> Handle(
        GetLookupsQuery request,
        CancellationToken cancellationToken)
    {
        var tasks = request.Requests
            .Select(r => gateway.GetLookupAsync(r, cancellationToken))
            .ToArray();

        var results = await Task.WhenAll(tasks);
        return ApiResponse<IReadOnlyList<LookupResultDto>>.Ok(results);
    }
}
