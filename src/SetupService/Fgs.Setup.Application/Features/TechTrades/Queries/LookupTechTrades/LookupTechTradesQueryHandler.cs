using Fgs.Contracts.Api;
using Fgs.Setup.Application.Abstractions.TechTrades;
using Fgs.Setup.Application.Features.TechTrades.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.TechTrades.Queries.LookupTechTrades;

public sealed class LookupTechTradesQueryHandler(ITechTradeReadRepository readRepository)
    : IRequestHandler<LookupTechTradesQuery, ApiResponse<IReadOnlyList<TechTradeLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<TechTradeLookupDto>>> Handle(
        LookupTechTradesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
        return ApiResponse<IReadOnlyList<TechTradeLookupDto>>.Ok(result);
    }
}
