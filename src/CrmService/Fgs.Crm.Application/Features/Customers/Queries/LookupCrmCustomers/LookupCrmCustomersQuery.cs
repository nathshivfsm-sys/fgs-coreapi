using Fgs.Contracts.Api;
using Fgs.Crm.Application.Features.Customers.Dtos;
using MediatR;

namespace Fgs.Crm.Application.Features.Customers.Queries.LookupCrmCustomers;

public sealed record LookupCrmCustomersQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<CrmCustomerLookupDto>>>;
