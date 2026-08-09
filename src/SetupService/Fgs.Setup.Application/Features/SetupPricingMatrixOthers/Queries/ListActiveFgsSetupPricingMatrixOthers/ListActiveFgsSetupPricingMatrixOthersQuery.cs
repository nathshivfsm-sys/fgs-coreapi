using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Queries.ListActiveFgsSetupPricingMatrixOthers;

public sealed record ListActiveFgsSetupPricingMatrixOthersQuery(int Page = 1, int PageSize = 25, string? SortBy = null, SortDirection SortDirection = SortDirection.Asc, string? Search = null, FgsSetupPricingMatrixOtherListFilters? Filters = null) : IRequest<ApiResponse<PagedResult<FgsSetupPricingMatrixOtherSummaryDto>>>;
