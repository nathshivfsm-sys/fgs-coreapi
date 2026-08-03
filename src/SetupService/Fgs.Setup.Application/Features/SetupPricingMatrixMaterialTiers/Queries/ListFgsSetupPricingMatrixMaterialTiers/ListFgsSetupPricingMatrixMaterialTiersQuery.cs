using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Queries.ListFgsSetupPricingMatrixMaterialTiers;

public sealed record ListFgsSetupPricingMatrixMaterialTiersQuery(SetupListQuery Query, FgsSetupPricingMatrixMaterialTierListFilters Filters) : IRequest<ApiResponse<PagedResult<FgsSetupPricingMatrixMaterialTierSummaryDto>>>;
