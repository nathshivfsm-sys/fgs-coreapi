using Amazon;

namespace Fgs.Credentials.Aws;

public static class AwsClientCredentialHelper
{
    public static RegionEndpoint ResolveRegionEndpoint(string? region) =>
        RegionEndpoint.GetBySystemName(string.IsNullOrWhiteSpace(region) ? "us-east-1" : region);

    public static bool TryResolveExplicitCredentials(
        string? accessKeyId,
        string? secretAccessKey,
        out string resolvedAccessKeyId,
        out string resolvedSecretAccessKey)
    {
        resolvedAccessKeyId = accessKeyId
            ?? Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID")
            ?? string.Empty;
        resolvedSecretAccessKey = secretAccessKey
            ?? Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY")
            ?? string.Empty;

        return !string.IsNullOrWhiteSpace(resolvedAccessKeyId)
            && !string.IsNullOrWhiteSpace(resolvedSecretAccessKey);
    }

    public static bool HasExplicitCredentials(string? accessKeyId, string? secretAccessKey) =>
        !string.IsNullOrWhiteSpace(accessKeyId)
        && !string.IsNullOrWhiteSpace(secretAccessKey);
}
