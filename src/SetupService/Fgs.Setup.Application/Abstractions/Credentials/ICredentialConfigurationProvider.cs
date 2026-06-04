namespace Fgs.Setup.Application.Abstractions.Credentials;

/// <summary>
/// Provides decrypted credential configuration loaded at startup and refreshed after mutations.
/// </summary>
public interface ICredentialConfigurationProvider
{
    IReadOnlyDictionary<string, string> Values { get; }

    string? GetValue(string key);

    Task ReloadAsync(CancellationToken cancellationToken = default);
}
