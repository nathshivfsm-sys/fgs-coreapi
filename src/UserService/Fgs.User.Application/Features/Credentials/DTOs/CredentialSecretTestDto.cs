using System.Text.Json;

namespace Fgs.User.Application.Features.Credentials.DTOs;

/// <summary>
/// Development-only test payload containing decrypted secret values. Never expose in production APIs.
/// </summary>
public sealed class CredentialSecretTestDto
{
    public Guid SecretId { get; init; }

    public string ProviderTypeCode { get; init; } = null!;

    public int VersionNo { get; init; }

    public JsonElement SecretPayload { get; init; }

    public string? SqlConnectionString { get; init; }
}
