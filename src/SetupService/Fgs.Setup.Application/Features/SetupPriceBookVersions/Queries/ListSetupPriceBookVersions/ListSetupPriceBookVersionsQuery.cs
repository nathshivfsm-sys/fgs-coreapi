using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPriceBookVersions.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPriceBookVersions.Queries.ListSetupPriceBookVersions;

public sealed record ListSetupPriceBookVersionsQuery(
    SetupListQuery Query,
    FgsSetupPriceBookVersionListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsSetupPriceBookVersionSummaryDto>>>;
