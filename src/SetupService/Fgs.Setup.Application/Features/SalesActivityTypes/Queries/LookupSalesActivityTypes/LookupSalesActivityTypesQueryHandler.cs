using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SalesActivityTypes;
using Fgs.Setup.Application.Features.SalesActivityTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesActivityTypes.Queries.LookupSalesActivityTypes;

public sealed class LookupSalesActivityTypesQueryHandler(IFgsSalesActivityTypeReadRepository readRepository)
    : IRequestHandler<LookupSalesActivityTypesQuery, ApiResponse<IReadOnlyList<FgsSalesActivityTypeLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsSalesActivityTypeLookupDto>>> Handle(
        LookupSalesActivityTypesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
            return ApiResponse<IReadOnlyList<FgsSalesActivityTypeLookupDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<IReadOnlyList<FgsSalesActivityTypeLookupDto>>(ex);
        }
    }
}
