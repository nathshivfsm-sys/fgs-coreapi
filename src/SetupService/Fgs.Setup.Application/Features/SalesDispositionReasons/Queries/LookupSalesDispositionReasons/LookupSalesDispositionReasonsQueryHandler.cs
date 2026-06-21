using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SalesDispositionReasons;
using Fgs.Setup.Application.Features.SalesDispositionReasons.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesDispositionReasons.Queries.LookupSalesDispositionReasons;

public sealed class LookupSalesDispositionReasonsQueryHandler(IFgsSalesDispositionReasonReadRepository readRepository)
    : IRequestHandler<LookupSalesDispositionReasonsQuery, ApiResponse<IReadOnlyList<FgsSalesDispositionReasonLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsSalesDispositionReasonLookupDto>>> Handle(
        LookupSalesDispositionReasonsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
            return ApiResponse<IReadOnlyList<FgsSalesDispositionReasonLookupDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<IReadOnlyList<FgsSalesDispositionReasonLookupDto>>(ex);
        }
    }
}
