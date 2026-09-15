using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.GloLookups.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloMasterEntityTypes;

public sealed record LookupGloMasterEntityTypesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<GloMasterEntityTypeLookupDto>>>;
