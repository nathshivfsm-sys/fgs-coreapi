using Amazon;
using Amazon.SecretsManager;
using Fgs.User.Infrastructure.Common.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fgs.User.Infrastructure;

public static class AwsCredentialsServiceCollectionExtensions
{
    public static IServiceCollection AddAwsCredentialsServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMemoryCache();

        var section = configuration.GetSection(AwsCredentialsOptions.SectionName);
        services.Configure<AwsCredentialsOptions>(section);

        services.AddSingleton<IAmazonSecretsManager>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<AwsCredentialsOptions>>().Value;
            var regionEndpoint = RegionEndpoint.GetBySystemName(
                string.IsNullOrWhiteSpace(options.Region) ? "us-east-1" : options.Region);

            var config = new AmazonSecretsManagerConfig
            {
                RegionEndpoint = regionEndpoint
            };

            if (HasExplicitCredentials(options))
            {
                return new AmazonSecretsManagerClient(
                    options.AccessKeyId!,
                    options.SecretAccessKey!,
                    config);
            }

            // Production: ECS/EC2/EKS IAM role, or ~/.aws/credentials when EnableLocalProfileFallback is true.
            return new AmazonSecretsManagerClient(config);
        });

        return services;
    }

    private static bool HasExplicitCredentials(AwsCredentialsOptions options) =>
        !string.IsNullOrWhiteSpace(options.AccessKeyId)
        && !string.IsNullOrWhiteSpace(options.SecretAccessKey);
}
