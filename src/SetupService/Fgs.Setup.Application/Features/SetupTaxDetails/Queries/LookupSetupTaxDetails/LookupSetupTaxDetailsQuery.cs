using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupTaxDetails.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTaxDetails.Queries.LookupSetupTaxDetails;

public sealed record LookupSetupTaxDetailsQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsSetupTaxDetailLookupDto>>>;
