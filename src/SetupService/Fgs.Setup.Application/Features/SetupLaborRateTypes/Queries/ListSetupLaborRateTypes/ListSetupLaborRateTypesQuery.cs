using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupLaborRateTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupLaborRateTypes.Queries.ListSetupLaborRateTypes;

public sealed record ListSetupLaborRateTypesQuery(
    SetupListQuery Query, FgsSetupLaborRateTypeListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsSetupLaborRateTypeSummaryDto>>>;
