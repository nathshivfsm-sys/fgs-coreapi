using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;

namespace Fgs.Security.Services;

/// <summary>
/// Microsoft Graph access tokens include a proprietary <c>nonce</c> in the JWT header that is
/// replaced after signing. The signed bytes contain Base64Url(SHA256(nonce)) instead of the
/// wire nonce, so standard signature validation fails unless the header is normalized first.
/// </summary>
public static class FgsEntraGraphAccessTokenNormalizer
{
    public static string? NormalizeIfRequired(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return token;
        }

        var parts = token.Split('.');
        if (parts.Length != 3)
        {
            return token;
        }

        if (!TryReadJsonObject(parts[0], out var header)
            || !header.TryGetProperty("nonce", out var nonceElement)
            || nonceElement.ValueKind != JsonValueKind.String)
        {
            return token;
        }

        if (!TryReadJsonObject(parts[1], out var payload)
            || !payload.TryGetProperty("aud", out var audienceElement)
            || audienceElement.ValueKind != JsonValueKind.String
            || !string.Equals(
                audienceElement.GetString(),
                FgsEntraTokenValidation.MicrosoftGraphAudience,
                StringComparison.OrdinalIgnoreCase))
        {
            return token;
        }

        var nonce = nonceElement.GetString();
        if (string.IsNullOrEmpty(nonce))
        {
            return token;
        }

        var hashedNonce = Base64UrlEncoder.Encode(
            SHA256.HashData(Encoding.UTF8.GetBytes(nonce)));

        header = ReplaceJsonStringProperty(header, "nonce", hashedNonce);
        var normalizedHeader = Base64UrlEncoder.Encode(
            Encoding.UTF8.GetBytes(header.GetRawText()));

        return $"{normalizedHeader}.{parts[1]}.{parts[2]}";
    }

    private static bool TryReadJsonObject(string base64UrlSegment, out JsonElement json)
    {
        json = default;
        try
        {
            var bytes = Base64UrlEncoder.DecodeBytes(base64UrlSegment);
            using var document = JsonDocument.Parse(bytes);
            json = document.RootElement.Clone();
            return json.ValueKind == JsonValueKind.Object;
        }
        catch
        {
            return false;
        }
    }

    private static JsonElement ReplaceJsonStringProperty(
        JsonElement source,
        string propertyName,
        string value)
    {
        using var stream = new MemoryStream();
        using (var writer = new Utf8JsonWriter(stream))
        {
            writer.WriteStartObject();
            foreach (var property in source.EnumerateObject())
            {
                if (string.Equals(property.Name, propertyName, StringComparison.Ordinal))
                {
                    writer.WriteString(propertyName, value);
                }
                else
                {
                    property.WriteTo(writer);
                }
            }

            writer.WriteEndObject();
        }

        using var document = JsonDocument.Parse(stream.ToArray());
        return document.RootElement.Clone();
    }
}
