using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.ResolutionCodes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.ResolutionCodes.Queries.ListResolutionCodes;

public sealed record ListResolutionCodesQuery(
    SetupListQuery Query, ResolutionCodeListFilters Filters)
    : IRequest<ApiResponse<PagedResult<ResolutionCodeSummaryDto>>>;
