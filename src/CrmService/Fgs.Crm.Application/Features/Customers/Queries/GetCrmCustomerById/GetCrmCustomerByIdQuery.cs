using Fgs.Contracts.Api;
using Fgs.Crm.Application.Features.Customers.Dtos;
using MediatR;

namespace Fgs.Crm.Application.Features.Customers.Queries.GetCrmCustomerById;

public sealed record GetCrmCustomerByIdQuery(long Id)
    : IRequest<ApiResponse<CrmCustomerDetailDto>>;
