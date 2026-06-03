using System.Text.Json;
using System.Text.Json.Serialization;

namespace Fgs.Foundation.Api;

public static class JsonSerializerOptionsExtensions
{
    public static JsonSerializerOptions ConfigureFgsApi(this JsonSerializerOptions options)
    {
        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        return options;
    }
}
