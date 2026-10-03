using Fgs.Contracts.Api;
using Fgs.Crm.Application.Abstractions.Customers;
using Fgs.Crm.Application.Features.Customers.Dtos;
using MediatR;

namespace Fgs.Crm.Application.Features.Customers.Queries.ListCrmCustomerServiceLocations;

public sealed class ListCrmCustomerServiceLocationsQueryHandler(ICrmCustomerReadRepository readRepository)
    : IRequestHandler<ListCrmCustomerServiceLocationsQuery, ApiResponse<CrmServiceLocationListResultDto>>
{
    public async Task<ApiResponse<CrmServiceLocationListResultDto>> Handle(
        ListCrmCustomerServiceLocationsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListServiceLocationsAsync(
            request.CustomerId,
            request.Query,
            request.IncludeSummary,
            cancellationToken);

        if (result is null)
        {
            return ApiResponse<CrmServiceLocationListResultDto>.Fail(
                [$"Customer '{request.CustomerId}' was not found."],
                ApiStatusCodes.NotFound);
        }

        return ApiResponse<CrmServiceLocationListResultDto>.Ok(result);
    }
}