using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Queries.LookupFgsSetupPricingMatrixMaterialTiers;

public sealed record LookupFgsSetupPricingMatrixMaterialTiersQuery(bool ActiveOnly = true, long? PricingMatrixId = null) : IRequest<ApiResponse<IReadOnlyList<FgsSetupPricingMatrixMaterialTierLookupDto>>>;
