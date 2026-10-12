using Fgs.Contracts.Api;
using Fgs.Crm.Application.Abstractions.Customers;
using Fgs.Crm.Application.Features.Customers.Dtos;
using MediatR;

namespace Fgs.Crm.Application.Features.Customers.Queries.ListCrmCompanyServiceLocations;

public sealed class ListCrmCompanyServiceLocationsQueryHandler(ICrmCustomerReadRepository readRepository)
    : IRequestHandler<ListCrmCompanyServiceLocationsQuery, ApiResponse<CrmCompanyServiceLocationListResultDto>>
{
    public async Task<ApiResponse<CrmCompanyServiceLocationListResultDto>> Handle(
        ListCrmCompanyServiceLocationsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListCompanyServiceLocationsAsync(
            request.Query,
            request.Filters,
            request.IncludeSummary,
            cancellationToken);

        return ApiResponse<CrmCompanyServiceLocationListResultDto>.Ok(result);
    }
}
