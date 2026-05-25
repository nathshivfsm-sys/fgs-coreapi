using Amazon;
using Amazon.S3;
using Fgs.User.Infrastructure.Common.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fgs.User.Infrastructure;

public static class AwsS3ServiceCollectionExtensions
{
    public static IServiceCollection AddAwsS3Client(this IServiceCollection services)
    {
        services.AddSingleton<AmazonS3Client>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<AwsS3Options>>().Value;
            var config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(options.Region)
            };

            return new AmazonS3Client(options.AccessKeyId, options.SecretAccessKey, config);
        });

        return services;
    }
}
