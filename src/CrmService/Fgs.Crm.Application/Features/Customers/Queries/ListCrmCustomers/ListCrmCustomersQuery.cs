using Fgs.Contracts.Api;
using Fgs.Crm.Application.Common.CrmCrud;
using Fgs.Crm.Application.Features.Customers.Dtos;
using Fgs.Foundation.Paging;
using MediatR;

namespace Fgs.Crm.Application.Features.Customers.Queries.ListCrmCustomers;

public sealed record ListCrmCustomersQuery(
    CrmListQuery Query,
    CrmCustomerListFilters Filters)
    : IRequest<ApiResponse<PagedResult<CrmCustomerSummaryDto>>>;
