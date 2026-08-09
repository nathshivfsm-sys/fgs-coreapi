using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Abstractions.TruckStockTemplates;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.TruckStockTemplates.Queries.ListTruckStockTemplates;

public sealed class ListTruckStockTemplatesQueryHandler(IFgsTruckStockTemplateReadRepository readRepository)
    : IRequestHandler<ListTruckStockTemplatesQuery, ApiResponse<PagedResult<FgsTruckStockTemplateSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsTruckStockTemplateSummaryDto>>> Handle(
        ListTruckStockTemplatesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsTruckStockTemplateSummaryDto>>.Ok(result);
    }
}
