using System.Text.Json;
using System.Text.Json.Serialization;
using Fgs.User.Application.Signup.Json;

namespace Fgs.User.API.Json;

internal static class JsonSerializerOptionsExtensions
{
    public static JsonSerializerOptions ConfigureFgsApi(this JsonSerializerOptions options)
    {
        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        options.Converters.Add(new CompanySizeJsonConverter());
        options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        return options;
    }
}
