using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SalesActivityOutcomes;
using Fgs.Setup.Application.Features.SalesActivityOutcomes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesActivityOutcomes.Queries.LookupSalesActivityOutcomes;

public sealed class LookupSalesActivityOutcomesQueryHandler(IFgsSalesActivityOutcomeReadRepository readRepository)
    : IRequestHandler<LookupSalesActivityOutcomesQuery, ApiResponse<IReadOnlyList<FgsSalesActivityOutcomeLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsSalesActivityOutcomeLookupDto>>> Handle(
        LookupSalesActivityOutcomesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
            return ApiResponse<IReadOnlyList<FgsSalesActivityOutcomeLookupDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<IReadOnlyList<FgsSalesActivityOutcomeLookupDto>>(ex);
        }
    }
}
