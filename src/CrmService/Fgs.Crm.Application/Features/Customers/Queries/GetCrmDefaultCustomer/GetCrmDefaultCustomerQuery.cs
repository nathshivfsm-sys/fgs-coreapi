using Fgs.Contracts.Api;
using Fgs.Crm.Application.Features.Customers.Dtos;
using MediatR;

namespace Fgs.Crm.Application.Features.Customers.Queries.GetCrmDefaultCustomer;

public sealed record GetCrmDefaultCustomerQuery
    : IRequest<ApiResponse<CrmDefaultCustomerDto>>;
