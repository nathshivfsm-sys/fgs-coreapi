using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.GloLookups.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloInventoryTransactionTypes;

public sealed record LookupGloInventoryTransactionTypesQuery(int? SourceTypeId = null, bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<GloInventoryTransactionTypeLookupDto>>>;
