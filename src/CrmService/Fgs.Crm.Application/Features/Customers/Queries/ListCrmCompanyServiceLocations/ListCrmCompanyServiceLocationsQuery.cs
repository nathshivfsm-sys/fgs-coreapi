using Fgs.Contracts.Api;
using Fgs.Crm.Application.Common.CrmCrud;
using Fgs.Crm.Application.Features.Customers.Dtos;
using MediatR;

namespace Fgs.Crm.Application.Features.Customers.Queries.ListCrmCompanyServiceLocations;

public sealed record ListCrmCompanyServiceLocationsQuery(
    CrmListQuery Query,
    CrmCompanyServiceLocationListFilters Filters,
    bool IncludeSummary = true)
    : IRequest<ApiResponse<CrmCompanyServiceLocationListResultDto>>;
