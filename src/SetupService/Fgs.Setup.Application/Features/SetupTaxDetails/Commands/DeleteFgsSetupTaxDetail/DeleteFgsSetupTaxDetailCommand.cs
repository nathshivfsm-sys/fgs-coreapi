using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupTaxDetails.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTaxDetails.Commands.DeleteFgsSetupTaxDetail;

public sealed record DeleteFgsSetupTaxDetailCommand(long Id)
    : IRequest<ApiResponse<FgsSetupTaxDetailDetailDto>>;
