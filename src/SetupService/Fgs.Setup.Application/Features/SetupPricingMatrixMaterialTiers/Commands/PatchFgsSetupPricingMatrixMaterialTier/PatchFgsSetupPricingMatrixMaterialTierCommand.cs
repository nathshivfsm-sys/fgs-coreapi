using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Commands.PatchFgsSetupPricingMatrixMaterialTier;

public sealed record PatchFgsSetupPricingMatrixMaterialTierCommand(long Id, FgsSetupPricingMatrixMaterialTierPatchDto Dto) : IRequest<ApiResponse<FgsSetupPricingMatrixMaterialTierDetailDto>>;
