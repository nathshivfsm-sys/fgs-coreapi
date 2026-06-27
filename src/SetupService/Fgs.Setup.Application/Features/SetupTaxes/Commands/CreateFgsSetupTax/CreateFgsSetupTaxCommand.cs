using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupTaxes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTaxes.Commands.CreateFgsSetupTax;

public sealed record CreateFgsSetupTaxCommand(FgsSetupTaxCreateDto Dto)
    : IRequest<ApiResponse<FgsSetupTaxDetailDto>>;
