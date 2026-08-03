using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Queries.LookupFgsSetupPricingMatrixOthers;

public sealed record LookupFgsSetupPricingMatrixOthersQuery(bool ActiveOnly = true, long? PricingMatrixId = null) : IRequest<ApiResponse<IReadOnlyList<FgsSetupPricingMatrixOtherLookupDto>>>;
