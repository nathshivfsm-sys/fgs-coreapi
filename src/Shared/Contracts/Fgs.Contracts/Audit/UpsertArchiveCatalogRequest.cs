namespace Fgs.Contracts.Audit;

/// <summary>
/// S2S request to create or update an archive catalog entry keyed by <see cref="ArchiveMonth"/>.
/// </summary>
public sealed record UpsertArchiveCatalogRequest(
    DateOnly ArchiveMonth,
    string StoragePath,
    long FileSize);
