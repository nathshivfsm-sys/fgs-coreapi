using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrixLaborTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixLaborTiers.Queries.LookupFgsSetupPricingMatrixLaborTiers;

public sealed record LookupFgsSetupPricingMatrixLaborTiersQuery(bool ActiveOnly = true, long? PricingMatrixLaborId = null) : IRequest<ApiResponse<IReadOnlyList<FgsSetupPricingMatrixLaborTierLookupDto>>>;
