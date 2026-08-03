using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Queries.ListActiveFgsSetupPricingMatrixMaterialTiers;

public sealed record ListActiveFgsSetupPricingMatrixMaterialTiersQuery(int Page = 1, int PageSize = 25, string? SortBy = null, SortDirection SortDirection = SortDirection.Asc, string? Search = null, FgsSetupPricingMatrixMaterialTierListFilters? Filters = null) : IRequest<ApiResponse<PagedResult<FgsSetupPricingMatrixMaterialTierSummaryDto>>>;
