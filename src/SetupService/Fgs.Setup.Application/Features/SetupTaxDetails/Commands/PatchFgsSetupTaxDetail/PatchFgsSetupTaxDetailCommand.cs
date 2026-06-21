using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupTaxDetails.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTaxDetails.Commands.PatchFgsSetupTaxDetail;

public sealed record PatchFgsSetupTaxDetailCommand(long Id, FgsSetupTaxDetailPatchDto Dto)
    : IRequest<ApiResponse<FgsSetupTaxDetailDetailDto>>;
