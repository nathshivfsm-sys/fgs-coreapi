using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupPostalCodes;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPostalCodes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPostalCodes.Queries.ListActiveSetupPostalCodes;

public sealed class ListActiveSetupPostalCodesQueryHandler(IFgsSetupPostalCodeReadRepository readRepository)
    : IRequestHandler<ListActiveSetupPostalCodesQuery, ApiResponse<PagedResult<FgsSetupPostalCodeSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSetupPostalCodeSummaryDto>>> Handle(
        ListActiveSetupPostalCodesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var query = new SetupListQuery(
                request.Page,
                request.PageSize,
                request.SortBy,
                request.SortDirection,
                request.Search,
                IsActive: true);

            var result = await readRepository.ListAsync(
                query,
                request.Filters ?? new FgsSetupPostalCodeListFilters(),
                cancellationToken);

            return ApiResponse<PagedResult<FgsSetupPostalCodeSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<FgsSetupPostalCodeSummaryDto>>(ex);
        }
    }
}
