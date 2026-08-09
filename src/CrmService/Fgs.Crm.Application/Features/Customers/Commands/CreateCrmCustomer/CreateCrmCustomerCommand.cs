using Fgs.Contracts.Api;
using Fgs.Crm.Application.Features.Customers.Dtos;
using MediatR;

namespace Fgs.Crm.Application.Features.Customers.Commands.CreateCrmCustomer;

public sealed record CreateCrmCustomerCommand(CrmCustomerCreateDto Dto)
    : IRequest<ApiResponse<CrmCustomerDetailDto>>;
