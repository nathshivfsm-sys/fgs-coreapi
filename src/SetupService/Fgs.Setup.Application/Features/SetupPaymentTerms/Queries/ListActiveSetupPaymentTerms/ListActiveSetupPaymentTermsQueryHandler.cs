using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupPaymentTerms;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPaymentTerms.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPaymentTerms.Queries.ListActiveSetupPaymentTerms;

public sealed class ListActiveSetupPaymentTermsQueryHandler(IFgsSetupPaymentTermReadRepository readRepository)
    : IRequestHandler<ListActiveSetupPaymentTermsQuery, ApiResponse<PagedResult<FgsSetupPaymentTermSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSetupPaymentTermSummaryDto>>> Handle(
        ListActiveSetupPaymentTermsQuery request,
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
                request.Filters ?? new FgsSetupPaymentTermListFilters(),
                cancellationToken);

            return ApiResponse<PagedResult<FgsSetupPaymentTermSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<PagedResult<FgsSetupPaymentTermSummaryDto>>(ex);
        }
    }
}
