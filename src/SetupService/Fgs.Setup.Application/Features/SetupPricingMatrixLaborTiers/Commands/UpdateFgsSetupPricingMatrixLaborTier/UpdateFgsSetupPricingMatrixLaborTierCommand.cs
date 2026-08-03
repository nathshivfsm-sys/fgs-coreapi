using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPricingMatrixLaborTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixLaborTiers.Commands.UpdateFgsSetupPricingMatrixLaborTier;

public sealed record UpdateFgsSetupPricingMatrixLaborTierCommand(long Id, FgsSetupPricingMatrixLaborTierUpdateDto Dto) : IRequest<ApiResponse<FgsSetupPricingMatrixLaborTierDetailDto>>;
