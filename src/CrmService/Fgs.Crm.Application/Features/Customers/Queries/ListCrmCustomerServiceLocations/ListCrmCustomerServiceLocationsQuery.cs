using Fgs.Contracts.Api;
using Fgs.Crm.Application.Common.CrmCrud;
using Fgs.Crm.Application.Features.Customers.Dtos;
using MediatR;

namespace Fgs.Crm.Application.Features.Customers.Queries.ListCrmCustomerServiceLocations;

public sealed record ListCrmCustomerServiceLocationsQuery(
    long CustomerId,
    CrmListQuery Query,
    bool IncludeSummary = true)
    : IRequest<ApiResponse<CrmServiceLocationListResultDto>>;
