using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Commands.CreateFgsSetupPricingMatrixLabor;

public sealed record CreateFgsSetupPricingMatrixLaborCommand(FgsSetupPricingMatrixLaborCreateDto Dto) : IRequest<ApiResponse<FgsSetupPricingMatrixLaborDetailDto>>;
