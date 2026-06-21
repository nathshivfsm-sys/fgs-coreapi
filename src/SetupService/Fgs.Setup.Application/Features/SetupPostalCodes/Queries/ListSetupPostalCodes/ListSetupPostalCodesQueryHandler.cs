using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupPostalCodes;
using Fgs.Setup.Application.Features.SetupPostalCodes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPostalCodes.Queries.ListSetupPostalCodes;

public sealed class ListSetupPostalCodesQueryHandler(IFgsSetupPostalCodeReadRepository readRepository)
    : IRequestHandler<ListSetupPostalCodesQuery, ApiResponse<PagedResult<FgsSetupPostalCodeSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSetupPostalCodeSummaryDto>>> Handle(
        ListSetupPostalCodesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
            return ApiResponse<PagedResult<FgsSetupPostalCodeSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<FgsSetupPostalCodeSummaryDto>>(ex);
        }
    }
}
