using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupTaxes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTaxes.Queries.ListSetupTaxes;

public sealed record ListSetupTaxesQuery(
    SetupListQuery Query, FgsSetupTaxListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsSetupTaxSummaryDto>>>;
