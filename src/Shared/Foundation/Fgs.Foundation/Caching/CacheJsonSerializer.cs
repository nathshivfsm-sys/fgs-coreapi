using System.Text.Json;
using Fgs.Foundation.Api;

namespace Fgs.Foundation.Caching;

internal static class CacheJsonSerializer
{
    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions().ConfigureFgsApi();

    public static byte[] Serialize<T>(T value) where T : class =>
        JsonSerializer.SerializeToUtf8Bytes(value, Options);

    public static string Serialize(object value) =>
        JsonSerializer.Serialize(value, Options);

    public static T? Deserialize<T>(byte[] data) where T : class =>
        JsonSerializer.Deserialize<T>(data, Options);
}
