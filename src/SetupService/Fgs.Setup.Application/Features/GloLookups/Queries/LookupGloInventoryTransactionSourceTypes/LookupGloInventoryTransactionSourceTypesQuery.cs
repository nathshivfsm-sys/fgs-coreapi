using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.GloLookups.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloInventoryTransactionSourceTypes;

public sealed record LookupGloInventoryTransactionSourceTypesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<GloInventoryTransactionSourceTypeLookupDto>>>;
