namespace Fgs.Bff.API.GraphQL;

/// <summary>
/// GraphQL root for BFF read aggregation. Mutations/composites stay on REST Controllers.
/// </summary>
public sealed class BffQuery
{
    /// <summary>Lightweight readiness signal for GraphQL clients.</summary>
    public BffServiceInfo Service() => new("fgs-bff-service", "1.0.0");
}

public sealed record BffServiceInfo(string Name, string Version);
