using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Commands.DeleteFgsSetupPricingMatrixOther;

public sealed record DeleteFgsSetupPricingMatrixOtherCommand(long Id) : IRequest<ApiResponse<FgsSetupPricingMatrixOtherDetailDto>>;
