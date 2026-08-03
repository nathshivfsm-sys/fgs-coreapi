using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Commands.PatchFgsSetupPricingMatrixOther;

public sealed record PatchFgsSetupPricingMatrixOtherCommand(long Id, FgsSetupPricingMatrixOtherPatchDto Dto) : IRequest<ApiResponse<FgsSetupPricingMatrixOtherDetailDto>>;
