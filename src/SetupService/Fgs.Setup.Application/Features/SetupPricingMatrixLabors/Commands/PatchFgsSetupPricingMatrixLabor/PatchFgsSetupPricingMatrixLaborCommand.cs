using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Commands.PatchFgsSetupPricingMatrixLabor;

public sealed record PatchFgsSetupPricingMatrixLaborCommand(long Id, FgsSetupPricingMatrixLaborPatchDto Dto) : IRequest<ApiResponse<FgsSetupPricingMatrixLaborDetailDto>>;
