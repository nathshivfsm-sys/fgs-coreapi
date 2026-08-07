using Fgs.Contracts.Api;
using Fgs.Crm.Application.Abstractions.Customers;
using Fgs.Crm.Application.Features.Customers.Dtos;
using Fgs.Foundation.Paging;
using MediatR;

namespace Fgs.Crm.Application.Features.Customers.Queries.ListCrmCustomers;

public sealed class ListCrmCustomersQueryHandler(ICrmCustomerReadRepository readRepository)
    : IRequestHandler<ListCrmCustomersQuery, ApiResponse<PagedResult<CrmCustomerSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<CrmCustomerSummaryDto>>> Handle(
        ListCrmCustomersQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<CrmCustomerSummaryDto>>.Ok(result);
    }
}
