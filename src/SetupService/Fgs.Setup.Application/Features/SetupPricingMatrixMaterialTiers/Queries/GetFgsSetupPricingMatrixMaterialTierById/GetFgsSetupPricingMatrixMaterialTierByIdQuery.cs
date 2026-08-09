using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Queries.GetFgsSetupPricingMatrixMaterialTierById;

public sealed record GetFgsSetupPricingMatrixMaterialTierByIdQuery(long Id) : IRequest<ApiResponse<FgsSetupPricingMatrixMaterialTierDetailDto>>;
