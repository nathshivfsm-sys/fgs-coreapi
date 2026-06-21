using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupTaxDetails.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTaxDetails.Queries.ListSetupTaxDetails;

public sealed record ListSetupTaxDetailsQuery(
    SetupListQuery Query, FgsSetupTaxDetailListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsSetupTaxDetailSummaryDto>>>;
