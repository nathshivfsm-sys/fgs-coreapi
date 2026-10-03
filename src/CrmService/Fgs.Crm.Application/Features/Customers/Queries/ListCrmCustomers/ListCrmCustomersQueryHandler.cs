using Fgs.Contracts.Api;
using Fgs.Crm.Application.Abstractions.Customers;
using Fgs.Crm.Application.Features.Customers.Dtos;
using MediatR;

namespace Fgs.Crm.Application.Features.Customers.Queries.ListCrmCustomers;

public sealed class ListCrmCustomersQueryHandler(ICrmCustomerReadRepository readRepository)
    : IRequestHandler<ListCrmCustomersQuery, ApiResponse<CrmCustomerListResultDto>>
{
    public async Task<ApiResponse<CrmCustomerListResultDto>> Handle(
        ListCrmCustomersQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(
            request.Query,
            request.Filters,
            request.IncludeSummary,
            cancellationToken);
        return ApiResponse<CrmCustomerListResultDto>.Ok(result);
    }
}
