using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupTaxes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTaxes.Queries.LookupSetupTaxes;

public sealed record LookupSetupTaxesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsSetupTaxLookupDto>>>;
