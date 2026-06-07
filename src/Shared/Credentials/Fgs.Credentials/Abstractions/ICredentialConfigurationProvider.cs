namespace Fgs.Credentials.Abstractions;

public interface ICredentialConfigurationProvider
{
    IReadOnlyDictionary<string, string> Values { get; }

    string? GetValue(string key);

    string? GetConnectionString(string name);

    Task ReloadAsync(CancellationToken cancellationToken = default);
}
