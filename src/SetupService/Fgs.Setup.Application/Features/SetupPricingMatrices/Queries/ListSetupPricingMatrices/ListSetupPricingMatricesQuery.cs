using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrices.Queries.ListSetupPricingMatrices;

public sealed record ListSetupPricingMatricesQuery(
    SetupListQuery Query,
    FgsSetupPricingMatrixListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsSetupPricingMatrixSummaryDto>>>;
