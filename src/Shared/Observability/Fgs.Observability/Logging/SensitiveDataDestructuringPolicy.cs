using System.Collections;
using System.Reflection;
using Serilog.Core;
using Serilog.Events;

namespace Fgs.Observability.Logging;

/// <summary>
/// Redacts common secret/PII property names from structured log payloads.
/// </summary>
public sealed class SensitiveDataDestructuringPolicy : IDestructuringPolicy
{
    private static readonly HashSet<string> SensitiveNames = new(StringComparer.OrdinalIgnoreCase)
    {
        "password",
        "passwd",
        "pwd",
        "secret",
        "apikey",
        "api_key",
        "access_token",
        "refresh_token",
        "id_token",
        "authorization",
        "connectionstring",
        "connection_string",
        "jwt",
        "bearer",
        "clientsecret",
        "client_secret",
        "privatekey",
        "private_key",
        "ssn",
        "creditcard",
        "cardnumber"
    };

    public bool TryDestructure(
        object value,
        ILogEventPropertyValueFactory propertyValueFactory,
        out LogEventPropertyValue result)
    {
        if (value is IDictionary dictionary)
        {
            var props = new List<LogEventProperty>();
            foreach (DictionaryEntry entry in dictionary)
            {
                var key = entry.Key?.ToString() ?? string.Empty;
                props.Add(new LogEventProperty(
                    key,
                    IsSensitive(key)
                        ? new ScalarValue("***REDACTED***")
                        : propertyValueFactory.CreatePropertyValue(entry.Value, destructureObjects: true)));
            }

            result = new StructureValue(props);
            return true;
        }

        var type = value.GetType();
        if (type.IsPrimitive
            || value is string
            || value is decimal
            || value is DateTime
            || value is DateTimeOffset
            || value is Guid
            || value is Enum)
        {
            result = new ScalarValue(null);
            return false;
        }

        var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => p.CanRead && p.GetIndexParameters().Length == 0)
            .ToArray();

        if (properties.Length == 0)
        {
            result = new ScalarValue(null);
            return false;
        }

        var structure = new List<LogEventProperty>(properties.Length);
        foreach (var property in properties)
        {
            object? propertyValue;
            try
            {
                propertyValue = property.GetValue(value);
            }
            catch
            {
                propertyValue = null;
            }

            structure.Add(new LogEventProperty(
                property.Name,
                IsSensitive(property.Name)
                    ? new ScalarValue("***REDACTED***")
                    : propertyValueFactory.CreatePropertyValue(propertyValue, destructureObjects: true)));
        }

        result = new StructureValue(structure, type.Name);
        return true;
    }

    private static bool IsSensitive(string name)
    {
        var normalized = name.Replace("-", string.Empty, StringComparison.Ordinal)
            .Replace("_", string.Empty, StringComparison.Ordinal);
        return SensitiveNames.Contains(name) || SensitiveNames.Contains(normalized);
    }
}
