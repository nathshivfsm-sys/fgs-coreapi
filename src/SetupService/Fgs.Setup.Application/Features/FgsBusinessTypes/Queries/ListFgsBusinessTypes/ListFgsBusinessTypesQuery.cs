using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.FgsBusinessTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.FgsBusinessTypes.Queries.ListFgsBusinessTypes;

public sealed record ListFgsBusinessTypesQuery(
    SetupListQuery Query, FgsBusinessTypeListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsBusinessTypeSummaryDto>>>;
