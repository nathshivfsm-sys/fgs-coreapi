namespace Fgs.Foundation.CatalogCrud;

public sealed record CatalogEntityColumnDescriptor(
    string PropertyName,
    string ColumnName,
    Type ClrType,
    bool IsRequired,
    int? MaxLength,
    bool IsReadOnly,
    bool IsSearchable,
    bool IsSortable,
    string? Comment = null);

public sealed record CatalogEntityUniqueKeyDescriptor(
    string Name,
    IReadOnlyList<string> PropertyNames);

public sealed record CatalogEntityDescriptor(
    string Key,
    string EntityName,
    Type ClrType,
    Type SummaryDtoType,
    Type DetailDtoType,
    Type CreateDtoType,
    Type UpdateDtoType,
    Type PatchDtoType,
    string TableName,
    string Schema,
    CatalogEntityKeyType KeyType,
    CatalogEntityVariant Variant,
    string RoutePlural,
    string SwaggerTag,
    string? TableComment,
    bool SupportsSoftDelete,
    IReadOnlyList<CatalogEntityColumnDescriptor> Columns,
    IReadOnlyList<CatalogEntityUniqueKeyDescriptor> UniqueKeys,
    IReadOnlyList<string> SearchableColumns,
    IReadOnlyList<string> SortableColumns)
{
    public string QualifiedTableName => $"\"{Schema}\".\"{TableName}\"";

    public string IdColumnName => "Id";

    public IReadOnlyList<CatalogEntityColumnDescriptor> WritableColumns =>
        Columns.Where(c => !c.IsReadOnly).ToList();
}
