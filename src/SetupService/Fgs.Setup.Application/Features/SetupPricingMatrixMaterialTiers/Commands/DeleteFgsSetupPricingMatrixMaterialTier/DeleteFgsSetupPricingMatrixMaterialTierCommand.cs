using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Commands.DeleteFgsSetupPricingMatrixMaterialTier;

public sealed record DeleteFgsSetupPricingMatrixMaterialTierCommand(long Id) : IRequest<ApiResponse<FgsSetupPricingMatrixMaterialTierDetailDto>>;
