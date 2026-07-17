using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.UniversalPricingServices.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalPricingServices.Queries.ListUniversalPricingServices;

public sealed record ListUniversalPricingServicesQuery(
    SetupListQuery Query, FgsUniversalPricingServiceListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsUniversalPricingServiceSummaryDto>>>;
