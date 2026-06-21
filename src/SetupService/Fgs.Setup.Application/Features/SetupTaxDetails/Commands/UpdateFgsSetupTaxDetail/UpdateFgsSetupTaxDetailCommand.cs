using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupTaxDetails.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTaxDetails.Commands.UpdateFgsSetupTaxDetail;

public sealed record UpdateFgsSetupTaxDetailCommand(long Id, FgsSetupTaxDetailUpdateDto Dto)
    : IRequest<ApiResponse<FgsSetupTaxDetailDetailDto>>;
