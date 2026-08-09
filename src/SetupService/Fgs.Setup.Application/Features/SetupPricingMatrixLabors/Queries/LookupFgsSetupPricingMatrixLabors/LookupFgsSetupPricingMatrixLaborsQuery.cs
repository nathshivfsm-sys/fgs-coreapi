using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Queries.LookupFgsSetupPricingMatrixLabors;

public sealed record LookupFgsSetupPricingMatrixLaborsQuery(bool ActiveOnly = true, long? PricingMatrixId = null) : IRequest<ApiResponse<IReadOnlyList<FgsSetupPricingMatrixLaborLookupDto>>>;
