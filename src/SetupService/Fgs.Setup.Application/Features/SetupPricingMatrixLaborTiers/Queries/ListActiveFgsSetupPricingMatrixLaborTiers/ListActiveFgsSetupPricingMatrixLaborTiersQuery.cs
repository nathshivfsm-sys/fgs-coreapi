using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrixLaborTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixLaborTiers.Queries.ListActiveFgsSetupPricingMatrixLaborTiers;

public sealed record ListActiveFgsSetupPricingMatrixLaborTiersQuery(int Page = 1, int PageSize = 25, string? SortBy = null, SortDirection SortDirection = SortDirection.Asc, string? Search = null, FgsSetupPricingMatrixLaborTierListFilters? Filters = null) : IRequest<ApiResponse<PagedResult<FgsSetupPricingMatrixLaborTierSummaryDto>>>;
