namespace Fgs.User.Application.TenantProvisioning;

public enum TenantSeedTableOutcome
{
    Succeeded,
    Skipped,
    Failed
}

public sealed record TenantSeedTableResult(
    string SeedCode,
    TenantSeedTableOutcome Outcome,
    string? Message = null,
    int RowsInserted = 0);

public sealed record TenantDataSeedResult(
    int SucceededCount,
    int SkippedCount,
    int FailedCount,
    IReadOnlyList<TenantSeedTableResult> TableResults)
{
    public int TotalCount => SucceededCount + SkippedCount + FailedCount;

    public bool HasFailures => FailedCount > 0;

    public bool HasAnySuccess => SucceededCount > 0;
}
