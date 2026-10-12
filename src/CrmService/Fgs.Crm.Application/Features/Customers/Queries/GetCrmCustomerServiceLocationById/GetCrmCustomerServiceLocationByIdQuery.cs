using Fgs.Contracts.Api;
using Fgs.Crm.Application.Features.Customers.Dtos;
using MediatR;

namespace Fgs.Crm.Application.Features.Customers.Queries.GetCrmCustomerServiceLocationById;

public sealed record GetCrmCustomerServiceLocationByIdQuery(long CustomerId, long ServiceLocationId)
    : IRequest<ApiResponse<CrmServiceLocationDetailDto>>;
