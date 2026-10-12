using Fgs.Contracts.Api;
using Fgs.Crm.Application.Features.Customers.Dtos;
using MediatR;

namespace Fgs.Crm.Application.Features.Customers.Commands.UpdateCrmCustomerServiceLocation;

public sealed record UpdateCrmCustomerServiceLocationCommand(
    long CustomerId,
    long ServiceLocationId,
    CrmServiceLocationUpdateDto Dto)
    : IRequest<ApiResponse<CrmServiceLocationDetailDto>>;
