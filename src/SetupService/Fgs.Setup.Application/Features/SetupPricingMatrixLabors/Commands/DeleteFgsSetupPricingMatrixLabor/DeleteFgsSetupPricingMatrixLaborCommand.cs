using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Commands.DeleteFgsSetupPricingMatrixLabor;

public sealed record DeleteFgsSetupPricingMatrixLaborCommand(long Id) : IRequest<ApiResponse<FgsSetupPricingMatrixLaborDetailDto>>;
