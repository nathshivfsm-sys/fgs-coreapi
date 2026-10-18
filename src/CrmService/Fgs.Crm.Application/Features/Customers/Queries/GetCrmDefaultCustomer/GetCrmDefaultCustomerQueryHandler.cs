using Fgs.Contracts.Api;
using Fgs.Crm.Application.Abstractions.Customers;
using Fgs.Crm.Application.Features.Customers.Dtos;
using MediatR;

namespace Fgs.Crm.Application.Features.Customers.Queries.GetCrmDefaultCustomer;

public sealed class GetCrmDefaultCustomerQueryHandler(ICrmCustomerReadRepository readRepository)
    : IRequestHandler<GetCrmDefaultCustomerQuery, ApiResponse<CrmDefaultCustomerDto>>
{
    public async Task<ApiResponse<CrmDefaultCustomerDto>> Handle(
        GetCrmDefaultCustomerQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.GetDefaultCustomerAsync(cancellationToken);
        return ApiResponse<CrmDefaultCustomerDto>.Ok(result ?? CrmDefaultCustomerDto.Empty);
    }
}
