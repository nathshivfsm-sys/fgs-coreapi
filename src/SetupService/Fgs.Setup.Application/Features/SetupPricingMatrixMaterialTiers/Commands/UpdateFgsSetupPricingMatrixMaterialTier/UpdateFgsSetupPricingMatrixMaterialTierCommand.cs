using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Commands.UpdateFgsSetupPricingMatrixMaterialTier;

public sealed record UpdateFgsSetupPricingMatrixMaterialTierCommand(long Id, FgsSetupPricingMatrixMaterialTierUpdateDto Dto) : IRequest<ApiResponse<FgsSetupPricingMatrixMaterialTierDetailDto>>;
