using Amazon;
using Amazon.S3;
using Fgs.File.Infrastructure.Common.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fgs.File.Infrastructure;

public static class AwsCredentialsServiceCollectionExtensions
{
    public static IServiceCollection AddAwsS3Services(this IServiceCollection services)
    {
        services.AddSingleton<AmazonS3Client>(CreateS3Client);
        return services;
    }

    private static AmazonS3Client CreateS3Client(IServiceProvider sp)
    {
        // us-east-1 still defaults to SigV2 for presigned URLs unless this is set explicitly.
        AWSConfigsS3.UseSignatureVersion4 = true;

        var options = sp.GetRequiredService<IOptions<AwsCredentialsOptions>>().Value;
        var config = new AmazonS3Config
        {
            RegionEndpoint = ResolveRegionEndpoint(options.Region),
            SignatureVersion = "4",
            UseHttp = false
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
