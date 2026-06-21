using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupTaxDetails.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTaxDetails.Queries.GetFgsSetupTaxDetailById;

public sealed record GetFgsSetupTaxDetailByIdQuery(long Id)
    : IRequest<ApiResponse<FgsSetupTaxDetailDetailDto>>;
