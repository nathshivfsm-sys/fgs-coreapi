using System.Text;

namespace Fgs.CatalogCrud.CodeGen;

internal static class CodeGenerator
{
    public static void GenerateAll(IReadOnlyList<EntityMetadata> entities, CodeGenOptions options)
    {
        var catalogPath = Path.Combine(options.ApplicationPath, "Common", "Catalog");
        Directory.CreateDirectory(Path.Combine(options.ApplicationPath, "Features", "Generated", "Descriptors"));
        Directory.CreateDirectory(Path.Combine(options.ApplicationPath, "Features", "Generated", "Dtos"));
        Directory.CreateDirectory(Path.Combine(options.ApplicationPath, "Features", "Generated", "Validators"));
        Directory.CreateDirectory(Path.Combine(options.ApiPath, "Controllers", "Generated"));
        Directory.CreateDirectory(catalogPath);

        foreach (var entity in entities)
        {
            WriteFile(Path.Combine(options.ApplicationPath, "Features", "Generated", "Dtos", $"{entity.EntityName}Dtos.cs"), GenerateDtos(entity, options));
            WriteFile(Path.Combine(options.ApplicationPath, "Features", "Generated", "Descriptors", $"{entity.EntityName}Descriptor.cs"), GenerateDescriptor(entity, options));
            WriteFile(Path.Combine(options.ApplicationPath, "Features", "Generated", "Validators", $"{entity.EntityName}Validators.cs"), GenerateValidators(entity, options));
            WriteFile(Path.Combine(options.ApiPath, "Controllers", "Generated", $"{ToControllerName(entity, options)}Controller.cs"), GenerateController(entity, options));
        }

        WriteFile(Path.Combine(catalogPath, "EntityKeys.cs"), GenerateKeys(entities, options));
        WriteFile(Path.Combine(catalogPath, "EntityRegistryRegistration.Generated.cs"), GenerateRegistry(entities, options));
        WriteFile(Path.Combine(catalogPath, $"{options.Service}CatalogEntityRegistration.Generated.cs"), GenerateRegistrationExtension(entities, options));
    }

    private static string GenerateDtos(EntityMetadata entity, CodeGenOptions options)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"namespace {options.ApplicationNamespace}.Features.Generated.Dtos;");
        sb.AppendLine();
        sb.AppendLine($"/// <summary>{EscapeXml(entity.TableComment ?? entity.EntityName)}</summary>");

        GenerateDtoRecord(sb, $"{entity.EntityName}SummaryDto", entity.Columns.Where(c => c.PropertyName is not "CreatedBy" and not "UpdatedBy").ToList());
        GenerateDtoRecord(sb, $"{entity.EntityName}DetailDto", entity.Columns);
        GenerateDtoRecord(sb, $"{entity.EntityName}CreateDto", entity.Columns.Where(c => !c.IsReadOnly && c.PropertyName != "IsActive").ToList(), requiredOnly: true);
        GenerateDtoRecord(sb, $"{entity.EntityName}UpdateDto", entity.Columns.Where(c => !c.IsReadOnly && c.PropertyName != "IsActive").ToList());
        GenerateDtoRecord(sb, $"{entity.EntityName}PatchDto", entity.Columns.Where(c => !c.IsReadOnly && c.PropertyName != "IsActive").ToList(), patch: true);

        return sb.ToString();
    }

    private static void GenerateDtoRecord(
        StringBuilder sb,
        string dtoName,
        IReadOnlyList<ColumnMetadata> columns,
        bool requiredOnly = false,
        bool patch = false)
    {
        sb.AppendLine($"public sealed record {dtoName}(");
        var properties = columns.ToList();
        for (var index = 0; index < properties.Count; index++)
        {
            var column = properties[index];
            var typeName = ToDtoPropertyType(column, patch, requiredOnly);

            sb.AppendLine($"    /// <summary>{EscapeXml(column.Comment ?? column.PropertyName)}</summary>");
            sb.Append($"    {typeName} {column.PropertyName}");
            sb.Append(index == properties.Count - 1 ? ")" : ",");
            sb.AppendLine();
        }

        sb.AppendLine(";");
        sb.AppendLine();
    }

    private static string GenerateDescriptor(EntityMetadata entity, CodeGenOptions options)
    {
        var domainNs = options.EntityNamespace;
        var sb = new StringBuilder();
        sb.AppendLine("using Fgs.Foundation.CatalogCrud;");
        sb.AppendLine($"using {options.ApplicationNamespace}.Common.Catalog;");
        sb.AppendLine($"using {options.ApplicationNamespace}.Features.Generated.Dtos;");
        sb.AppendLine();
        sb.AppendLine($"namespace {options.ApplicationNamespace}.Features.Generated.Descriptors;");
        sb.AppendLine();
        sb.AppendLine($"public static class {entity.EntityName}Descriptor");
        sb.AppendLine("{");
        sb.AppendLine("    public static CatalogEntityDescriptor Create() => new(");
        sb.AppendLine($"        Key: EntityKeys.{ToKeyName(entity.Key)},");
        sb.AppendLine($"        EntityName: \"{entity.EntityName}\",");
        sb.AppendLine($"        ClrType: typeof({domainNs}.{entity.EntityName}),");
        sb.AppendLine($"        SummaryDtoType: typeof({entity.EntityName}SummaryDto),");
        sb.AppendLine($"        DetailDtoType: typeof({entity.EntityName}DetailDto),");
        sb.AppendLine($"        CreateDtoType: typeof({entity.EntityName}CreateDto),");
        sb.AppendLine($"        UpdateDtoType: typeof({entity.EntityName}UpdateDto),");
        sb.AppendLine($"        PatchDtoType: typeof({entity.EntityName}PatchDto),");
        sb.AppendLine($"        TableName: \"{entity.EntityName}\",");
        sb.AppendLine($"        Schema: \"{options.DefaultSchema}\",");
        sb.AppendLine($"        KeyType: CatalogEntityKeyType.{entity.KeyType},");
        sb.AppendLine($"        Variant: CatalogEntityVariant.{entity.Variant},");
        sb.AppendLine($"        RoutePlural: \"{entity.RoutePlural}\",");
        sb.AppendLine($"        SwaggerTag: \"{entity.SwaggerTag}\",");
        sb.AppendLine($"        TableComment: \"{EscapeString(entity.TableComment ?? entity.EntityName)}\",");
        sb.AppendLine($"        SupportsSoftDelete: {(entity.SupportsSoftDelete ? "true" : "false")},");
        sb.AppendLine("        Columns:");
        sb.AppendLine("        [");
        foreach (var column in entity.Columns)
        {
            sb.AppendLine("            new CatalogEntityColumnDescriptor(");
            sb.AppendLine($"                \"{column.PropertyName}\", \"{column.ColumnName}\", {ToTypeofExpression(column.ClrType)}, {column.IsRequired.ToString().ToLowerInvariant()}, {FormatNullableInt(column.MaxLength)}, {column.IsReadOnly.ToString().ToLowerInvariant()}, {column.IsSearchable.ToString().ToLowerInvariant()}, {column.IsSortable.ToString().ToLowerInvariant()}, \"{EscapeString(column.Comment ?? column.PropertyName)}\"),");
        }

        sb.AppendLine("        ],");
        sb.AppendLine("        UniqueKeys:");
        sb.AppendLine("        [");
        foreach (var uniqueKey in entity.UniqueKeys)
        {
            var props = string.Join(", ", uniqueKey.PropertyNames.Select(property => $"\"{property}\""));
            sb.AppendLine($"            new CatalogEntityUniqueKeyDescriptor(\"{uniqueKey.Name}\", [{props}]),");
        }

        sb.AppendLine("        ],");
        var searchable = string.Join(", ", entity.Columns.Where(c => c.IsSearchable).Select(c => $"\"{c.PropertyName}\""));
        var sortable = string.Join(", ", entity.Columns.Where(c => c.IsSortable).Select(c => $"\"{c.PropertyName}\""));
        sb.AppendLine($"        SearchableColumns: [{searchable}],");
        sb.AppendLine($"        SortableColumns: [{sortable}]);");
        sb.AppendLine("}");

        return sb.ToString();
    }

    private static string GenerateValidators(EntityMetadata entity, CodeGenOptions options)
    {
        var sb = new StringBuilder();
        sb.AppendLine("using FluentValidation;");
        sb.AppendLine("using Fgs.Foundation.CatalogCrud.Abstractions;");
        sb.AppendLine("using Fgs.Foundation.CatalogCrud.Commands;");
        sb.AppendLine("using Fgs.Foundation.CatalogCrud.Validation;");
        sb.AppendLine($"using {options.ApplicationNamespace}.Features.Generated.Descriptors;");
        sb.AppendLine($"using {options.ApplicationNamespace}.Features.Generated.Dtos;");
        sb.AppendLine();
        sb.AppendLine($"namespace {options.ApplicationNamespace}.Features.Generated.Validators;");
        sb.AppendLine();
        sb.AppendLine($"public sealed class Create{entity.EntityName}CommandValidator : AbstractValidator<CreateCatalogEntityCommand<{entity.EntityName}CreateDto, {entity.EntityName}DetailDto>>");
        sb.AppendLine("{");
        sb.AppendLine($"    public Create{entity.EntityName}CommandValidator(IEntityReadRepository readRepository)");
        sb.AppendLine("    {");
        sb.AppendLine($"        var descriptor = {entity.EntityName}Descriptor.Create();");
        sb.AppendLine("        RuleFor(x => x.EntityKey).Equal(descriptor.Key);");
        sb.AppendLine("        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);");
        sb.AppendLine("    }");
        sb.AppendLine("}");
        sb.AppendLine();
        sb.AppendLine($"public sealed class Update{entity.EntityName}CommandValidator : AbstractValidator<UpdateCatalogEntityCommand<{entity.EntityName}UpdateDto, {entity.EntityName}DetailDto>>");
        sb.AppendLine("{");
        sb.AppendLine($"    public Update{entity.EntityName}CommandValidator(IEntityReadRepository readRepository)");
        sb.AppendLine("    {");
        sb.AppendLine($"        var descriptor = {entity.EntityName}Descriptor.Create();");
        sb.AppendLine("        RuleFor(x => x.EntityKey).Equal(descriptor.Key);");
        sb.AppendLine("        RuleFor(x => x.Id).NotEmpty();");
        sb.AppendLine("        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);");
        sb.AppendLine("    }");
        sb.AppendLine("}");
        sb.AppendLine();
        sb.AppendLine($"public sealed class Patch{entity.EntityName}CommandValidator : AbstractValidator<PatchCatalogEntityCommand<{entity.EntityName}PatchDto, {entity.EntityName}DetailDto>>");
        sb.AppendLine("{");
        sb.AppendLine("    public Patch" + entity.EntityName + "CommandValidator()");
        sb.AppendLine("    {");
        sb.AppendLine($"        var descriptor = {entity.EntityName}Descriptor.Create();");
        sb.AppendLine("        RuleFor(x => x.EntityKey).Equal(descriptor.Key);");
        sb.AppendLine("        RuleFor(x => x.Id).NotEmpty();");
        sb.AppendLine("        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);");
        sb.AppendLine("    }");
        sb.AppendLine("}");

        return sb.ToString();
    }

    private static string GenerateController(EntityMetadata entity, CodeGenOptions options)
    {
        var controllerName = ToControllerName(entity, options);
        var idConstraint = entity.KeyType == CatalogEntityKeyType.Guid ? "{id:guid}" : "{id:long}";
        var filterParams = entity.Columns
            .Where(column => column.IsSearchable && !ReadOnlyFilter(column.PropertyName))
            .Take(3)
            .Select(column => ($"{ToCamelCase(column.PropertyName)}", column.PropertyName, column.Comment ?? column.PropertyName))
            .ToList();

        var sb = new StringBuilder();
        sb.AppendLine("using Asp.Versioning;");
        sb.AppendLine("using Fgs.Contracts.Api;");
        sb.AppendLine("using Fgs.Foundation.Api;");
        sb.AppendLine("using Fgs.Foundation.CatalogCrud;");
        sb.AppendLine("using Fgs.Foundation.CatalogCrud.Commands;");
        sb.AppendLine("using Fgs.Foundation.CatalogCrud.Queries;");
        sb.AppendLine($"using {options.ApplicationNamespace}.Common.Catalog;");
        sb.AppendLine($"using {options.ApplicationNamespace}.Features.Generated.Dtos;");
        sb.AppendLine("using Microsoft.AspNetCore.Authorization;");
        sb.AppendLine("using Microsoft.AspNetCore.Mvc;");
        sb.AppendLine();
        sb.AppendLine($"namespace {options.ApiNamespace}.Controllers.Generated;");
        sb.AppendLine();
        sb.AppendLine($"/// <summary>{EscapeXml(entity.TableComment ?? $"Manage {entity.EntityName} catalog records.")}</summary>");
        sb.AppendLine("[Authorize]");
        sb.AppendLine("[ApiVersion(FgsApiVersions.V1)]");
        sb.AppendLine($"[FgsVersionedRoute(\"{entity.RoutePlural}\")]");
        sb.AppendLine($"[Tags(\"{entity.SwaggerTag}\")]");
        sb.AppendLine($"public sealed class {controllerName}Controller : CatalogCrudControllerBase");
        sb.AppendLine("{");
        sb.AppendLine($"    public {controllerName}Controller(MediatR.IMediator mediator) : base(mediator) {{ }}");
        sb.AppendLine();
        sb.AppendLine("    /// <summary>Gets a record by identifier.</summary>");
        sb.AppendLine($"    /// <param name=\"id\">The {entity.EntityName} identifier.</param>");
        sb.AppendLine($"    [HttpGet(\"{idConstraint}\")]");
        sb.AppendLine($"    [ProducesResponseType(typeof(ApiResponse<{entity.EntityName}DetailDto>), StatusCodes.Status200OK)]");
        sb.AppendLine("    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]");
        sb.AppendLine($"    public async Task<IActionResult> Get({(entity.KeyType == CatalogEntityKeyType.Guid ? "Guid" : "long")} id, CancellationToken cancellationToken) =>");
        sb.AppendLine($"        FromApiResponse(await Mediator.Send(new GetCatalogEntityQuery<{entity.EntityName}DetailDto>(EntityKeys.{ToKeyName(entity.Key)}, id.ToString()), cancellationToken));");
        sb.AppendLine();
        sb.AppendLine("    /// <summary>Lists records with pagination, sorting, and search.</summary>");
        sb.AppendLine("    /// <param name=\"page\">Page number (1-based).</param>");
        sb.AppendLine("    /// <param name=\"pageSize\">Number of records per page.</param>");
        sb.AppendLine("    /// <param name=\"sortBy\">Property name to sort by.</param>");
        sb.AppendLine("    /// <param name=\"sortDirection\">Sort direction.</param>");
        sb.AppendLine("    /// <param name=\"search\">Free-text search across searchable fields.</param>");
        sb.AppendLine("    /// <param name=\"isActive\">Filter by active status when supported.</param>");
        foreach (var filter in filterParams)
        {
            sb.AppendLine($"    /// <param name=\"{filter.Item1}\">Filter by {filter.Item3}.</param>");
        }

        sb.AppendLine("    [HttpGet]");
        sb.AppendLine($"    [ProducesResponseType(typeof(ApiResponse<PagedResult<{entity.EntityName}SummaryDto>>), StatusCodes.Status200OK)]");
        sb.AppendLine("    public async Task<IActionResult> List(");
        sb.AppendLine("        [FromQuery] int page = 1,");
        sb.AppendLine("        [FromQuery] int pageSize = 25,");
        sb.AppendLine("        [FromQuery] string? sortBy = null,");
        sb.AppendLine("        [FromQuery] SortDirection sortDirection = SortDirection.Asc,");
        sb.AppendLine("        [FromQuery] string? search = null,");
        sb.AppendLine("        [FromQuery] bool? isActive = true,");
        foreach (var filter in filterParams)
        {
            sb.AppendLine($"        [FromQuery] string? {filter.Item1} = null,");
        }

        sb.AppendLine("        CancellationToken cancellationToken = default)");
        sb.AppendLine("    {");
        if (filterParams.Count == 0)
        {
            sb.AppendLine($"        var response = await Mediator.Send(new ListCatalogEntitiesQuery<{entity.EntityName}SummaryDto>(EntityKeys.{ToKeyName(entity.Key)}, new PagedQuery(page, pageSize, sortBy, sortDirection, search, isActive)), cancellationToken);");
        }
        else
        {
            sb.AppendLine($"        var filters = new {entity.EntityName}ListFilters({string.Join(", ", filterParams.Select(filter => filter.Item1))});");
            sb.AppendLine($"        var response = await Mediator.Send(new ListCatalogEntitiesQuery<{entity.EntityName}SummaryDto>(EntityKeys.{ToKeyName(entity.Key)}, new PagedQuery(page, pageSize, sortBy, sortDirection, search, isActive), filters), cancellationToken);");
        }

        sb.AppendLine("        return FromApiResponse(response);");
        sb.AppendLine("    }");
        sb.AppendLine();
        sb.AppendLine("    /// <summary>Creates a new record.</summary>");
        sb.AppendLine("    [HttpPost]");
        sb.AppendLine($"    [ProducesResponseType(typeof(ApiResponse<{entity.EntityName}DetailDto>), StatusCodes.Status201Created)]");
        sb.AppendLine("    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]");
        sb.AppendLine("    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]");
        sb.AppendLine($"    public async Task<IActionResult> Create([FromBody] {entity.EntityName}CreateDto request, CancellationToken cancellationToken)");
        sb.AppendLine("    {");
        sb.AppendLine($"        var response = await Mediator.Send(new CreateCatalogEntityCommand<{entity.EntityName}CreateDto, {entity.EntityName}DetailDto>(EntityKeys.{ToKeyName(entity.Key)}, request), cancellationToken);");
        sb.AppendLine("        return StatusCode(response.StatusCode, response);");
        sb.AppendLine("    }");
        sb.AppendLine();
        sb.AppendLine("    /// <summary>Replaces an existing record.</summary>");
        sb.AppendLine($"    [HttpPut(\"{idConstraint}\")]");
        sb.AppendLine($"    [ProducesResponseType(typeof(ApiResponse<{entity.EntityName}DetailDto>), StatusCodes.Status200OK)]");
        sb.AppendLine("    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]");
        sb.AppendLine($"    public async Task<IActionResult> Update({(entity.KeyType == CatalogEntityKeyType.Guid ? "Guid" : "long")} id, [FromBody] {entity.EntityName}UpdateDto request, CancellationToken cancellationToken) =>");
        sb.AppendLine($"        FromApiResponse(await Mediator.Send(new UpdateCatalogEntityCommand<{entity.EntityName}UpdateDto, {entity.EntityName}DetailDto>(EntityKeys.{ToKeyName(entity.Key)}, id.ToString(), request), cancellationToken));");
        sb.AppendLine();
        sb.AppendLine("    /// <summary>Partially updates an existing record.</summary>");
        sb.AppendLine($"    [HttpPatch(\"{idConstraint}\")]");
        sb.AppendLine($"    [ProducesResponseType(typeof(ApiResponse<{entity.EntityName}DetailDto>), StatusCodes.Status200OK)]");
        sb.AppendLine("    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]");
        sb.AppendLine($"    public async Task<IActionResult> Patch({(entity.KeyType == CatalogEntityKeyType.Guid ? "Guid" : "long")} id, [FromBody] {entity.EntityName}PatchDto request, CancellationToken cancellationToken) =>");
        sb.AppendLine($"        FromApiResponse(await Mediator.Send(new PatchCatalogEntityCommand<{entity.EntityName}PatchDto, {entity.EntityName}DetailDto>(EntityKeys.{ToKeyName(entity.Key)}, id.ToString(), request), cancellationToken));");
        sb.AppendLine();
        sb.AppendLine("    /// <summary>Deletes a record (soft delete when supported).</summary>");
        sb.AppendLine($"    [HttpDelete(\"{idConstraint}\")]");
        sb.AppendLine("    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]");
        sb.AppendLine("    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]");
        sb.AppendLine($"    public async Task<IActionResult> Delete({(entity.KeyType == CatalogEntityKeyType.Guid ? "Guid" : "long")} id, CancellationToken cancellationToken) =>");
        sb.AppendLine($"        FromApiResponse(await Mediator.Send(new DeleteCatalogEntityCommand(EntityKeys.{ToKeyName(entity.Key)}, id.ToString()), cancellationToken));");
        sb.AppendLine("}");

        if (filterParams.Count > 0)
        {
            sb.AppendLine();
            sb.AppendLine($"internal sealed record {entity.EntityName}ListFilters({string.Join(", ", filterParams.Select(filter => $"string? {ToPascalCase(filter.Item1)}"))});");
        }

        return sb.ToString();
    }

    private static string GenerateKeys(IReadOnlyList<EntityMetadata> entities, CodeGenOptions options)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"namespace {options.ApplicationNamespace}.Common.Catalog;");
        sb.AppendLine();
        sb.AppendLine("public static class EntityKeys");
        sb.AppendLine("{");
        foreach (var entity in entities)
        {
            sb.AppendLine($"    public const string {ToKeyName(entity.Key)} = \"{entity.Key}\";");
        }

        sb.AppendLine("}");
        return sb.ToString();
    }

    private static string GenerateRegistry(IReadOnlyList<EntityMetadata> entities, CodeGenOptions options)
    {
        var sb = new StringBuilder();
        sb.AppendLine("using Fgs.Foundation.CatalogCrud;");
        sb.AppendLine("using Fgs.Foundation.CatalogCrud.Abstractions;");
        sb.AppendLine($"using {options.ApplicationNamespace}.Features.Generated.Descriptors;");
        sb.AppendLine();
        sb.AppendLine($"namespace {options.ApplicationNamespace}.Common.Catalog;");
        sb.AppendLine();
        sb.AppendLine("public static class EntityRegistryRegistration");
        sb.AppendLine("{");
        sb.AppendLine("    public static void RegisterAll(IEntityRegistry registry)");
        sb.AppendLine("    {");
        foreach (var entity in entities)
        {
            sb.AppendLine($"        registry.Register({entity.EntityName}Descriptor.Create());");
        }

        sb.AppendLine("    }");
        sb.AppendLine("}");

        return sb.ToString();
    }

    private static string GenerateRegistrationExtension(IReadOnlyList<EntityMetadata> entities, CodeGenOptions options)
    {
        var sb = new StringBuilder();
        sb.AppendLine("using Fgs.Foundation.CatalogCrud;");
        sb.AppendLine("using Fgs.Foundation.CatalogCrud.Abstractions;");
        sb.AppendLine("using Microsoft.Extensions.DependencyInjection;");
        sb.AppendLine();
        sb.AppendLine($"namespace {options.ApplicationNamespace}.Common.Catalog;");
        sb.AppendLine();
        sb.AppendLine($"public static class {options.Service}CatalogEntityRegistration");
        sb.AppendLine("{");
        sb.AppendLine($"    public static IServiceCollection Add{options.Service}CatalogEntities(this IServiceCollection services)");
        sb.AppendLine("    {");
        sb.AppendLine("        services.AddSingleton<IEntityRegistry>(sp =>");
        sb.AppendLine("        {");
        sb.AppendLine("            var registry = new EntityRegistry();");
        sb.AppendLine("            EntityRegistryRegistration.RegisterAll(registry);");
        sb.AppendLine("            return registry;");
        sb.AppendLine("        });");
        sb.AppendLine("        return services;");
        sb.AppendLine("    }");
        sb.AppendLine("}");

        return sb.ToString();
    }

    private static void WriteFile(string path, string content)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, content.ReplaceLineEndings("\n"));
        Console.WriteLine($"Generated {path}");
    }

    private static string ToControllerName(EntityMetadata entity, CodeGenOptions options) =>
        entity.EntityName.StartsWith(options.EntityNamePrefix, StringComparison.Ordinal)
            ? entity.EntityName[options.EntityNamePrefix.Length..]
            : entity.EntityName;

    private static string ToKeyName(string key) => key;

    private static bool ReadOnlyFilter(string propertyName) =>
        propertyName is "TenantId" or "CompanyId" or "CreatedOn" or "CreatedBy" or "UpdatedOn" or "UpdatedBy";

    private static string ToDtoPropertyType(ColumnMetadata column, bool patch, bool requiredOnly)
    {
        if (patch)
        {
            return MakeNullable(ToDtoTypeName(column.ClrType, false));
        }

        if (Nullable.GetUnderlyingType(column.ClrType) is not null)
        {
            return ToDtoTypeName(column.ClrType, false);
        }

        if (column.ClrType == typeof(string) && (!requiredOnly || !column.IsRequired))
        {
            return "string?";
        }

        return ToDtoTypeName(column.ClrType, false);
    }

    private static string ToDtoTypeName(Type clrType, bool patch) =>
        clrType switch
        {
            _ when clrType == typeof(string) => "string",
            _ when clrType == typeof(long) => "long",
            _ when clrType == typeof(int) => "int",
            _ when clrType == typeof(short) => "short",
            _ when clrType == typeof(bool) => "bool",
            _ when clrType == typeof(decimal) => "decimal",
            _ when clrType == typeof(double) => "double",
            _ when clrType == typeof(float) => "float",
            _ when clrType == typeof(Guid) => "Guid",
            _ when clrType == typeof(DateTimeOffset) => "DateTimeOffset",
            _ when clrType == typeof(DateTime) => "DateTime",
            _ when clrType == typeof(DateOnly) => "DateOnly",
            _ when Nullable.GetUnderlyingType(clrType) is Type underlying => MakeNullable(ToDtoTypeName(underlying, false)),
            _ => clrType.Name
        };

    private static string MakeNullable(string typeName) =>
        typeName.EndsWith('?') ? typeName : typeName + "?";

    private static string ToTypeofExpression(Type clrType)
    {
        var underlying = Nullable.GetUnderlyingType(clrType);
        if (underlying is not null)
        {
            return $"typeof({ToCSharpName(underlying)}?)";
        }

        return $"typeof({ToCSharpName(clrType)})";
    }

    private static string ToCSharpName(Type type) =>
        type switch
        {
            _ when type == typeof(string) => "string",
            _ when type == typeof(long) => "long",
            _ when type == typeof(int) => "int",
            _ when type == typeof(short) => "short",
            _ when type == typeof(bool) => "bool",
            _ when type == typeof(decimal) => "decimal",
            _ when type == typeof(DateTimeOffset) => "DateTimeOffset",
            _ when type == typeof(DateTime) => "DateTime",
            _ when type == typeof(DateOnly) => "DateOnly",
            _ when type == typeof(Guid) => "Guid",
            _ => type.FullName!.Replace('+', '.')
        };

    private static string FormatNullableInt(int? value) => value.HasValue ? value.Value.ToString() : "null";

    private static string EscapeXml(string value) =>
        value.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");

    private static string EscapeString(string value) =>
        value.Replace("\\", "\\\\").Replace("\"", "\\\"");

    private static string ToCamelCase(string value) =>
        char.ToLowerInvariant(value[0]) + value[1..];

    private static string ToPascalCase(string value) =>
        char.ToUpperInvariant(value[0]) + value[1..];
}
