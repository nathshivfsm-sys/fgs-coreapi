namespace Fgs.Notification.Infrastructure.Credentials;

public sealed class CredentialConfigurationHolder
{
    private IReadOnlyDictionary<string, string> _values =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

    public IReadOnlyDictionary<string, string> Values => _values;

    public string? GetValue(string key) =>
        _values.TryGetValue(key, out var value) ? value : null;

    internal void ReplaceValues(IReadOnlyDictionary<string, string> values) =>
        _values = values;
}
