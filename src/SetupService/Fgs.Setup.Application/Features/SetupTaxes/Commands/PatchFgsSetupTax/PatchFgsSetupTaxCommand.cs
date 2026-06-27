using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupTaxes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTaxes.Commands.PatchFgsSetupTax;

public sealed record PatchFgsSetupTaxCommand(long Id, FgsSetupTaxPatchDto Dto)
    : IRequest<ApiResponse<FgsSetupTaxDetailDto>>;
