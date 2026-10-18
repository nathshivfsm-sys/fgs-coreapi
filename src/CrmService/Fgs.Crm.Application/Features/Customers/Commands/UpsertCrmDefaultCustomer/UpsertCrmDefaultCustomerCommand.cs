using Fgs.Contracts.Api;
using Fgs.Crm.Application.Features.Customers.Dtos;
using MediatR;

namespace Fgs.Crm.Application.Features.Customers.Commands.UpsertCrmDefaultCustomer;

public sealed record UpsertCrmDefaultCustomerCommand(CrmDefaultCustomerUpsertDto Dto)
    : IRequest<ApiResponse<CrmDefaultCustomerDto>>;
