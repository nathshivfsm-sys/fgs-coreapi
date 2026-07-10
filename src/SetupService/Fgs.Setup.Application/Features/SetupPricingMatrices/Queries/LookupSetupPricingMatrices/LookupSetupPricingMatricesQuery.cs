using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrices.Queries.LookupSetupPricingMatrices;

public sealed record LookupSetupPricingMatricesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsSetupPricingMatrixLookupDto>>>;
