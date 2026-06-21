using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupLaborRateTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupLaborRateTypes.Queries.LookupSetupLaborRateTypes;

public sealed record LookupSetupLaborRateTypesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsSetupLaborRateTypeLookupDto>>>;
