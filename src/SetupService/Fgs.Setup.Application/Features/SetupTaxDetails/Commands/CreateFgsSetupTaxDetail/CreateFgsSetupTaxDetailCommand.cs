using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupTaxDetails.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTaxDetails.Commands.CreateFgsSetupTaxDetail;

public sealed record CreateFgsSetupTaxDetailCommand(FgsSetupTaxDetailCreateDto Dto)
    : IRequest<ApiResponse<FgsSetupTaxDetailDetailDto>>;
