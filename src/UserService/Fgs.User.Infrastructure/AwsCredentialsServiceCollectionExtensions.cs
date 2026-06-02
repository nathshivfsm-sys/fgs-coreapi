using Amazon;
using Amazon.S3;
using Fgs.User.Infrastructure.Common.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fgs.User.Infrastructure;

public static class AwsCredentialsServiceCollectionExtensions
{
    public static IServiceCollection AddAwsCredentialsServices(
        this IServiceCollection services,
        IConfiguration configuration,
        IConfigurationBuilder configurationBuilder)
    {
        services.AddFgsCredentialConfigurationServices(configuration, configurationBuilder);
        services.AddSingleton<AmazonS3Client>(CreateS3Client);
        return services;
    }

    private static AmazonS3Client CreateS3Client(IServiceProvider sp)
    {
        var options = sp.GetRequiredService<IOptions<AwsCredentialsOptions>>().Value;
        var config = new AmazonS3Config
        {
            RegionEndpoint = ResolveRegionEndpoint(options.Region)
        };

        return HasExplicitCredentials(options)
            ? new AmazonS3Client(options.AccessKeyId!, options.SecretAccessKey!, config)
            : new AmazonS3Client(config);
    }

    private static bool HasExplicitCredentials(AwsCredentialsOptions options) =>
        !string.IsNullOrWhiteSpace(options.AccessKeyId)
        && !string.IsNullOrWhiteSpace(options.SecretAccessKey);

    private static RegionEndpoint ResolveRegionEndpoint(string? region) =>
        RegionEndpoint.GetBySystemName(string.IsNullOrWhiteSpace(region) ? "us-east-1" : region);
}
