using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.Vendors.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Vendors.Queries.ListVendors;

public sealed record ListVendorsQuery(
    SetupListQuery Query, FgsVendorListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsVendorSummaryDto>>>;
