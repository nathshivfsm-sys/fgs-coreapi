using Fgs.Contracts.Api;
using Fgs.Crm.Application.Features.Customers.Dtos;
using MediatR;

namespace Fgs.Crm.Application.Features.Customers.Commands.AddCrmCustomerServiceLocation;

public sealed record AddCrmCustomerServiceLocationCommand(long CustomerId, CrmServiceLocationCreateDto Dto)
    : IRequest<ApiResponse<CrmServiceLocationCreatedDto>>;
