using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
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
        try
        {
            var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
            return ApiResponse<IReadOnlyList<TechTradeLookupDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<IReadOnlyList<TechTradeLookupDto>>(ex);
        }
    }
}
