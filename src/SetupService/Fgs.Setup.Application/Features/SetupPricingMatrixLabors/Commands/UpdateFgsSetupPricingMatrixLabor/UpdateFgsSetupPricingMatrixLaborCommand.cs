using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Commands.UpdateFgsSetupPricingMatrixLabor;

public sealed record UpdateFgsSetupPricingMatrixLaborCommand(long Id, FgsSetupPricingMatrixLaborUpdateDto Dto) : IRequest<ApiResponse<FgsSetupPricingMatrixLaborDetailDto>>;
