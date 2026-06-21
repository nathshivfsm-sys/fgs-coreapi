using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupTaxes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTaxes.Commands.UpdateFgsSetupTax;

public sealed record UpdateFgsSetupTaxCommand(long Id, FgsSetupTaxUpdateDto Dto)
    : IRequest<ApiResponse<FgsSetupTaxDetailDto>>;
