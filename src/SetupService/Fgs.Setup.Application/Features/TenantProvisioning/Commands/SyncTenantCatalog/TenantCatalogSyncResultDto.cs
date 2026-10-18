namespace Fgs.Setup.Application.Features.TenantProvisioning.Commands.SyncTenantCatalog;

public sealed record TenantCatalogSyncTableDto(
    string SeedCode,
    string Outcome,
    int RowsInserted,
    string? Message);

public sealed record TenantCatalogSyncResultDto(
    int SucceededCount,
    int SkippedCount,
    int FailedCount,
    IReadOnlyList<TenantCatalogSyncTableDto> Tables);
