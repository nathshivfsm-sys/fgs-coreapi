using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPricingMatrixLaborTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixLaborTiers.Commands.PatchFgsSetupPricingMatrixLaborTier;

public sealed record PatchFgsSetupPricingMatrixLaborTierCommand(long Id, FgsSetupPricingMatrixLaborTierPatchDto Dto) : IRequest<ApiResponse<FgsSetupPricingMatrixLaborTierDetailDto>>;
