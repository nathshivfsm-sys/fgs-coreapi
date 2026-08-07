namespace Fgs.Audit.Application.Features.ArchiveCatalogs.Dtos;

public sealed record ArchiveCatalogDto(
    long Id,
    DateOnly ArchiveMonth,
    string StoragePath,
    long FileSize,
    DateTime CreatedOn);
