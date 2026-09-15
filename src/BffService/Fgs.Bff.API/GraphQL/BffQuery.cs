using Fgs.Bff.API.GraphQL.Lookups;
using Fgs.Bff.Application.Features.Lookups.Dtos;
using Fgs.Bff.Application.Features.Lookups.Queries.GetLookups;
using HotChocolate.Types;
using MediatR;

namespace Fgs.Bff.API.GraphQL;

/// <summary>
/// GraphQL root for BFF read aggregation. Mutations/composites stay on REST Controllers.
/// </summary>
public sealed class BffQuery
{
    /// <summary>Lightweight readiness signal for GraphQL clients.</summary>
    public BffServiceInfo Service() => new("fgs-bff-service", "1.0.6");

    /// <summary>
    /// Batch bag of owning-service lookups. Partial failures set <c>error</c> on that key only.
    /// </summary>
    public async Task<IReadOnlyList<LookupResultDto>> Lookups(
        IReadOnlyList<LookupRequestInput> requests,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var dtoRequests = requests.Select(r => r.ToDto()).ToList();
        var response = await mediator.Send(new GetLookupsQuery(dtoRequests), cancellationToken);
        return response.Data ?? [];
    }
}

public sealed record BffServiceInfo(string Name, string Version);
