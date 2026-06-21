using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupTaxes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTaxes.Commands.DeleteFgsSetupTax;

public sealed record DeleteFgsSetupTaxCommand(long Id)
    : IRequest<ApiResponse<FgsSetupTaxDetailDto>>;
