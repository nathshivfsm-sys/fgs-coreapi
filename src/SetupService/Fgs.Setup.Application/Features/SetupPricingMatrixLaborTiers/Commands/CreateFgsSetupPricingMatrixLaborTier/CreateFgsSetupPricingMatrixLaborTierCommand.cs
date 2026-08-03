using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPricingMatrixLaborTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixLaborTiers.Commands.CreateFgsSetupPricingMatrixLaborTier;

public sealed record CreateFgsSetupPricingMatrixLaborTierCommand(FgsSetupPricingMatrixLaborTierCreateDto Dto) : IRequest<ApiResponse<FgsSetupPricingMatrixLaborTierDetailDto>>;
