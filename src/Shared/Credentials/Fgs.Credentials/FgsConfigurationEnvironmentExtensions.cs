using Microsoft.Extensions.Configuration;

namespace Fgs.Credentials;

public static class FgsConfigurationEnvironmentExtensions
{
    public static void ApplyFgsAwsCredentialEnvironmentVariables(this ConfigurationManager configuration)
    {
        Apply(configuration, "KMS_KEY_ARN", "AwsCredentials:KmsKeyArn");
        Apply(configuration, "AWS_ACCESS_KEY_ID", "AwsCredentials:AccessKeyId");
        Apply(configuration, "AWS_SECRET_ACCESS_KEY", "AwsCredentials:SecretAccessKey");
        Apply(configuration, "CREDENTIAL_DISTRIBUTION_KEY", "CredentialDistribution:InternalServiceKey");
    }

    public static void ApplyFgsKmsEnvironmentVariable(this ConfigurationManager configuration) =>
        Apply(configuration, "KMS_KEY_ARN", "AwsCredentials:KmsKeyArn");

    private static void Apply(ConfigurationManager configuration, string environmentVariable, string configurationKey)
    {
        var value = Environment.GetEnvironmentVariable(environmentVariable);
        if (!string.IsNullOrWhiteSpace(value))
        {
            configuration[configurationKey] = value;
        }
    }
}
