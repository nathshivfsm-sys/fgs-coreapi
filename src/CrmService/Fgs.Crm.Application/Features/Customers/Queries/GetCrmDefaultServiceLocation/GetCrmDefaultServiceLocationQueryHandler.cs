using Fgs.Contracts.Api;
using Fgs.Crm.Application.Abstractions.Customers;
using Fgs.Crm.Application.Features.Customers.Dtos;
using MediatR;

namespace Fgs.Crm.Application.Features.Customers.Queries.GetCrmDefaultServiceLocation;

public sealed class GetCrmDefaultServiceLocationQueryHandler(ICrmCustomerReadRepository readRepository)
    : IRequestHandler<GetCrmDefaultServiceLocationQuery, ApiResponse<CrmDefaultServiceLocationDto>>
{
    public async Task<ApiResponse<CrmDefaultServiceLocationDto>> Handle(
        GetCrmDefaultServiceLocationQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.GetDefaultServiceLocationAsync(cancellationToken);
        return ApiResponse<CrmDefaultServiceLocationDto>.Ok(result ?? CrmDefaultServiceLocationDto.Empty);
    }
}
