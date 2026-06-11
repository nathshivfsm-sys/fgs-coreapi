namespace Fgs.CatalogCrud.CodeGen;

internal sealed record EntityMetadata(
    string EntityName,
    string Key,
    string RoutePlural,
    string SwaggerTag,
    string? TableComment,
    Type ClrType,
    CatalogEntityVariant Variant,
    CatalogEntityKeyType KeyType,
    bool SupportsSoftDelete,
    IReadOnlyList<ColumnMetadata> Columns,
    IReadOnlyList<UniqueKeyMetadata> UniqueKeys);

internal sealed record ColumnMetadata(
    string PropertyName,
    string ColumnName,
    Type ClrType,
    bool IsRequired,
    int? MaxLength,
    bool IsReadOnly,
    bool IsSearchable,
    bool IsSortable,
    string? Comment);

internal sealed record UniqueKeyMetadata(string Name, IReadOnlyList<string> PropertyNames);

internal enum CatalogEntityVariant
{
    StandardLong,
    StandardGuid,
    ScopedManualAudit,
    NullableTenantScope,
    HardDeleteScoped
}

internal enum CatalogEntityKeyType
{
    Long,
    Guid
}
