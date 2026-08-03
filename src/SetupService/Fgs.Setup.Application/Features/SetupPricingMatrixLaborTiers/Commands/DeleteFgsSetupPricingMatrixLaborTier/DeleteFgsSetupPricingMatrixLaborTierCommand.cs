using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPricingMatrixLaborTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixLaborTiers.Commands.DeleteFgsSetupPricingMatrixLaborTier;

public sealed record DeleteFgsSetupPricingMatrixLaborTierCommand(long Id) : IRequest<ApiResponse<FgsSetupPricingMatrixLaborTierDetailDto>>;
