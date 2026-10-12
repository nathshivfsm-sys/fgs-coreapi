using Fgs.Contracts.Api;
using Fgs.Crm.Application.Abstractions.Customers;
using Fgs.Crm.Application.Features.Customers.Dtos;
using MediatR;

namespace Fgs.Crm.Application.Features.Customers.Queries.GetCrmCustomerServiceLocationById;

public sealed class GetCrmCustomerServiceLocationByIdQueryHandler(ICrmCustomerReadRepository readRepository)
    : IRequestHandler<GetCrmCustomerServiceLocationByIdQuery, ApiResponse<CrmServiceLocationDetailDto>>
{
    public async Task<ApiResponse<CrmServiceLocationDetailDto>> Handle(
        GetCrmCustomerServiceLocationByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.GetServiceLocationByIdAsync(
            request.CustomerId,
            request.ServiceLocationId,
            cancellationToken);

        if (result is null)
        {
            return ApiResponse<CrmServiceLocationDetailDto>.Fail(
                [$"Service location '{request.ServiceLocationId}' was not found."],
                ApiStatusCodes.NotFound);
        }

        return ApiResponse<CrmServiceLocationDetailDto>.Ok(result);
    }
}
