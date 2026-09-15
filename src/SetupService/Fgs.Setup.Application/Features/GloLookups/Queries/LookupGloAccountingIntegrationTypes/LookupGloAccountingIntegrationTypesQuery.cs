using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.GloLookups.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloAccountingIntegrationTypes;

public sealed record LookupGloAccountingIntegrationTypesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<GloAccountingIntegrationTypeLookupDto>>>;
