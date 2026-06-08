using Fgs.Credentials.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Fgs.Credentials;

public static class ConnectionStringResolver
{
    public static string? Resolve(
        IConfiguration configuration,
        string connectionStringName,
        string? environmentVariable = null,
        ICredentialConfigurationProvider? credentialProvider = null)
    {
        static string? NullIfWhiteSpace(string? value) =>
            string.IsNullOrWhiteSpace(value) ? null : value;

        return NullIfWhiteSpace(credentialProvider?.GetConnectionString(connectionStringName))
            ?? NullIfWhiteSpace(configuration.GetConnectionString(connectionStringName))
            ?? (environmentVariable is null
                ? null
                : NullIfWhiteSpace(Environment.GetEnvironmentVariable(environmentVariable)));
    }

    public static string ResolveRequired(
        IConfiguration configuration,
        string connectionStringName,
        string? environmentVariable = null,
        ICredentialConfigurationProvider? credentialProvider = null) =>
        Resolve(configuration, connectionStringName, environmentVariable, credentialProvider)
        ?? throw new InvalidOperationException(
            $"PostgreSQL connection string '{connectionStringName}' is required. "
            + "Load it from Setup credential storage, set ConnectionStrings, or provide the environment override.");
}
