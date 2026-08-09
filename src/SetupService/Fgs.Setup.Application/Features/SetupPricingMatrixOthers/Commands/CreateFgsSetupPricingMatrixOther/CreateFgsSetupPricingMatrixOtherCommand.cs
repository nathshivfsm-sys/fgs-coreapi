using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Commands.CreateFgsSetupPricingMatrixOther;

public sealed record CreateFgsSetupPricingMatrixOtherCommand(FgsSetupPricingMatrixOtherCreateDto Dto) : IRequest<ApiResponse<FgsSetupPricingMatrixOtherDetailDto>>;
