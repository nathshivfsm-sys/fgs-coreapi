using Fgs.Contracts.Api;
using Fgs.Crm.Application.Features.Customers.Dtos;
using MediatR;

namespace Fgs.Crm.Application.Features.Customers.Commands.UpdateCrmCustomer;

public sealed record UpdateCrmCustomerCommand(long Id, CrmCustomerUpdateDto Dto)
    : IRequest<ApiResponse<CrmCustomerDetailDto>>;
