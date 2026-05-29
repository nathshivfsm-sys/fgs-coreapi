using System.Text.Json;
using Fgs.User.Application.Abstractions.Credentials;
using Fgs.User.Application.Features.Credentials.Payloads;

namespace Fgs.User.Infrastructure.Secrets;

public sealed class CredentialPayloadDeserializer : ICredentialPayloadDeserializer
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public T Deserialize<T>(string providerTypeCode, string secretJson) where T : class =>
        (T)Deserialize(providerTypeCode, secretJson, typeof(T));

    public object Deserialize(string providerTypeCode, string secretJson, Type payloadType)
    {
        var resolved = ResolvePayloadType(providerTypeCode) ?? payloadType;
        var result = JsonSerializer.Deserialize(secretJson, resolved, JsonOptions);
        return result ?? throw new InvalidOperationException("Failed to deserialize credential payload.");
    }

    private static Type? ResolvePayloadType(string providerTypeCode) =>
        providerTypeCode.ToUpperInvariant() switch
        {
            "STRIPE" or "PAYPAL" => typeof(StripeSecretPayload),
            "DATABASE" or "AWS" or "AZURE" => typeof(SqlDatabaseSecretPayload),
            _ => null
        };
}
