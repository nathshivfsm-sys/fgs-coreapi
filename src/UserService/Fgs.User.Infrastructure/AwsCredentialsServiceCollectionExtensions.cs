using Amazon;
using Amazon.S3;
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
            var config = new AmazonSecretsManagerConfig
            {
                RegionEndpoint = ResolveRegionEndpoint(options.Region)
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

        services.AddSingleton<AmazonS3Client>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<AwsCredentialsOptions>>().Value;
            var config = new AmazonS3Config
            {
                RegionEndpoint = ResolveRegionEndpoint(options.Region)
            };

            if (HasExplicitCredentials(options))
            {
                return new AmazonS3Client(options.AccessKeyId!, options.SecretAccessKey!, config);
            }

            return new AmazonS3Client(config);
        });

        return services;
    }

    private static bool HasExplicitCredentials(AwsCredentialsOptions options) =>
        !string.IsNullOrWhiteSpace(options.AccessKeyId)
        && !string.IsNullOrWhiteSpace(options.SecretAccessKey);

    private static RegionEndpoint ResolveRegionEndpoint(string? region) =>
        RegionEndpoint.GetBySystemName(string.IsNullOrWhiteSpace(region) ? "us-east-1" : region);
}
