using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupPostalCodes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPostalCodes.Queries.LookupSetupPostalCodes;

public sealed record LookupSetupPostalCodesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsSetupPostalCodeLookupDto>>>;
