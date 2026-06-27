using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupTaxes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTaxes.Queries.GetFgsSetupTaxById;

public sealed record GetFgsSetupTaxByIdQuery(long Id)
    : IRequest<ApiResponse<FgsSetupTaxDetailDto>>;
