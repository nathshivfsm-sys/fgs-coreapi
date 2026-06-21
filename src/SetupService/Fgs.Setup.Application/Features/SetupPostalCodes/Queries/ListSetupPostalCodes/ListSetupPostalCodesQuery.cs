using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPostalCodes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPostalCodes.Queries.ListSetupPostalCodes;

public sealed record ListSetupPostalCodesQuery(
    SetupListQuery Query, FgsSetupPostalCodeListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsSetupPostalCodeSummaryDto>>>;
