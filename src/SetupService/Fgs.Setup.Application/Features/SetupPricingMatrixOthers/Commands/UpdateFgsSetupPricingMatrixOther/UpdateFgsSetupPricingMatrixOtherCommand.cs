using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Commands.UpdateFgsSetupPricingMatrixOther;

public sealed record UpdateFgsSetupPricingMatrixOtherCommand(long Id, FgsSetupPricingMatrixOtherUpdateDto Dto) : IRequest<ApiResponse<FgsSetupPricingMatrixOtherDetailDto>>;
