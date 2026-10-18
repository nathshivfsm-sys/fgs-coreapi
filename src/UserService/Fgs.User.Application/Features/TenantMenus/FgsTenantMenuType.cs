using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Fgs.User.Application.Features.TenantMenus;

[TypeConverter(typeof(FgsTenantMenuTypeConverter))]
public enum FgsTenantMenuType
{
    [JsonStringEnumMemberName(FgsTenantMenuTypeConverter.TopMenu)]
    TopMenu,

    [JsonStringEnumMemberName(FgsTenantMenuTypeConverter.LeftMenu)]
    LeftMenu
}

public sealed class FgsTenantMenuTypeConverter : TypeConverter
{
    public const string TopMenu = "Top Menu";

    public const string LeftMenu = "Left Menu";

    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) =>
        sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is not string text)
        {
            return base.ConvertFrom(context, culture, value);
        }

        if (TryParse(text, out var menuType))
        {
            return menuType;
        }

        throw new FormatException($"'{text}' is not a valid menu type.");
    }

    public static string ToStoredValue(FgsTenantMenuType menuType)
    {
        var field = typeof(FgsTenantMenuType).GetField(menuType.ToString());
        var stored = field?.GetCustomAttribute<JsonStringEnumMemberNameAttribute>()?.Name;
        return stored ?? throw new ArgumentOutOfRangeException(nameof(menuType), menuType, null);
    }

    public static bool TryParse(string? text, out FgsTenantMenuType menuType)
    {
        foreach (var field in typeof(FgsTenantMenuType).GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            var wireName = field.GetCustomAttribute<JsonStringEnumMemberNameAttribute>()?.Name;
            if (string.Equals(wireName, text, StringComparison.Ordinal)
                || string.Equals(field.Name, text, StringComparison.Ordinal))
            {
                menuType = (FgsTenantMenuType)field.GetValue(null)!;
                return true;
            }
        }

        menuType = default;
        return false;
    }
}
