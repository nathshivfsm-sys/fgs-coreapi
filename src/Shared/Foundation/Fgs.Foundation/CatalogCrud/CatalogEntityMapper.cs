using System.Collections;
using System.Reflection;
using System.Text.Json;

namespace Fgs.Foundation.CatalogCrud;

public static class CatalogEntityMapper
{
    public static object MapRowToDto(IDictionary<string, object?> row, Type dtoType)
    {
        var constructor = dtoType.GetConstructors().FirstOrDefault()
            ?? throw new InvalidOperationException($"DTO type '{dtoType.Name}' has no constructor.");

        var args = constructor.GetParameters()
            .Select(parameter =>
            {
                var columnName = parameter.Name!;
                if (!row.TryGetValue(columnName, out var value) &&
                    !row.TryGetValue(columnName.ToLowerInvariant(), out value))
                {
                    return GetDefaultValue(parameter.ParameterType);
                }

                return ConvertValue(value, parameter.ParameterType);
            })
            .ToArray();

        return constructor.Invoke(args);
    }

    private static object? GetDefaultValue(Type type) =>
        Nullable.GetUnderlyingType(type) is not null ? null : Activator.CreateInstance(type);

    public static object MapCreateDto(object createDto, CatalogEntityDescriptor descriptor)
    {
        var entity = Activator.CreateInstance(descriptor.ClrType)
            ?? throw new InvalidOperationException($"Unable to create entity '{descriptor.EntityName}'.");

        CopyWritableProperties(createDto, entity, descriptor, includeNulls: false);
        return entity;
    }

    public static void MapUpdateDto(object updateDto, object entity, CatalogEntityDescriptor descriptor) =>
        CopyWritableProperties(updateDto, entity, descriptor, includeNulls: true);

    public static void MapPatchDto(object patchDto, object entity, CatalogEntityDescriptor descriptor)
    {
        foreach (var property in patchDto.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!property.CanRead)
            {
                continue;
            }

            var column = descriptor.WritableColumns.FirstOrDefault(c =>
                string.Equals(c.PropertyName, property.Name, StringComparison.Ordinal));
            if (column is null)
            {
                continue;
            }

            var value = property.GetValue(patchDto);
            if (value is null)
            {
                continue;
            }

            var entityProperty = descriptor.ClrType.GetProperty(property.Name);
            entityProperty?.SetValue(entity, ConvertValue(value, entityProperty.PropertyType));
        }
    }

    public static object ToDetailDto(object entity, CatalogEntityDescriptor descriptor) =>
        CopyToDto(entity, descriptor.DetailDtoType, descriptor);

    public static object ToSummaryDto(object entity, CatalogEntityDescriptor descriptor) =>
        CopyToDto(entity, descriptor.SummaryDtoType, descriptor);

    public static object? ParseId(string id, CatalogEntityKeyType keyType) =>
        keyType switch
        {
            CatalogEntityKeyType.Long => long.Parse(id),
            CatalogEntityKeyType.Guid => Guid.Parse(id),
            _ => throw new ArgumentOutOfRangeException(nameof(keyType))
        };

    public static Dictionary<string, object?> ExtractFilters(object? filterSource)
    {
        if (filterSource is null)
        {
            return new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
        }

        var filters = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
        foreach (var property in filterSource.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var value = property.GetValue(filterSource);
            if (value is not null)
            {
                filters[property.Name] = value;
            }
        }

        return filters;
    }

    private static object CopyToDto(object entity, Type dtoType, CatalogEntityDescriptor descriptor)
    {
        var constructor = dtoType.GetConstructors().FirstOrDefault()
            ?? throw new InvalidOperationException($"DTO type '{dtoType.Name}' has no constructor.");

        var args = constructor.GetParameters()
            .Select(parameter =>
            {
                var sourceProperty = descriptor.ClrType.GetProperty(parameter.Name!)
                    ?? dtoType.GetProperty(parameter.Name!);
                var value = sourceProperty?.GetValue(entity);
                return ConvertValue(value, parameter.ParameterType);
            })
            .ToArray();

        return constructor.Invoke(args);
    }

    private static void CopyWritableProperties(
        object source,
        object target,
        CatalogEntityDescriptor descriptor,
        bool includeNulls)
    {
        foreach (var column in descriptor.WritableColumns)
        {
            var sourceProperty = source.GetType().GetProperty(column.PropertyName);
            var targetProperty = descriptor.ClrType.GetProperty(column.PropertyName);
            if (sourceProperty is null || targetProperty is null || !targetProperty.CanWrite)
            {
                continue;
            }

            var value = sourceProperty.GetValue(source);
            if (value is null && !includeNulls)
            {
                continue;
            }

            targetProperty.SetValue(target, ConvertValue(value, targetProperty.PropertyType));
        }
    }

    private static object? ConvertValue(object? value, Type targetType)
    {
        if (value is null)
        {
            return null;
        }

        var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

        if (underlyingType.IsInstanceOfType(value))
        {
            return value;
        }

        if (value is JsonElement jsonElement)
        {
            return jsonElement.Deserialize(underlyingType);
        }

        if (underlyingType == typeof(Guid) && value is string guidString)
        {
            return Guid.Parse(guidString);
        }

        if (underlyingType.IsEnum)
        {
            return Enum.Parse(underlyingType, value.ToString()!, ignoreCase: true);
        }

        if (value is IConvertible)
        {
            return Convert.ChangeType(value, underlyingType);
        }

        return value;
    }
}
