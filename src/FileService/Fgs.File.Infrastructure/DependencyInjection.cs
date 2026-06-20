using Fgs.Contracts.Clients;
using Fgs.Credentials;
using Fgs.Credentials.Abstractions;
using Fgs.Credentials.Extensions;
using Fgs.File.Application.Abstractions.Provisioning;
using Fgs.File.Application.Abstractions.Storage;
using Fgs.File.Application.Common.Options;
using Fgs.File.Infrastructure.Common.Options;
using Fgs.File.Infrastructure.Database;
using Fgs.File.Infrastructure.Storage;
using Fgs.Foundation.Extensions;
using Fgs.Persistence.Extensions;
using Fgs.MultiTenancy;
using Fgs.Security.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.File.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsFileInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddFgsCredentialConsumer(
            configuration,
            configuration,
            options =>
            {
                options.ServiceName = "fgs-file-service";
                options.RequiredProviders = ["DATABASE", "AWS"];
            },
            typeof(AwsCredentialsOptions));

        services.AddFgsApiSecurity(configuration);
        services.Configure<AwsCredentialsOptions>(configuration.GetSection(AwsCredentialsOptions.SectionName));
        services.Configure<FileServiceOptions>(configuration.GetSection(FileServiceOptions.SectionName));

        services.AddFgsRefitClient<IUserTenantClient>(
            configuration,
            "UserService:BaseUrl",
            "http://user-service:5001");

        services.AddDbContext<FgsFileDbContext>((sp, options) =>
        {
            var connectionString = ConnectionStringResolver.ResolveRequired(
                sp.GetRequiredService<IConfiguration>(),
                ConnectionStringNames.FgsFile,
                "FGS_FILE_DB",
                sp.GetService<ICredentialConfigurationProvider>());
            options.UseFgsNpgsql(
                connectionString,
                "__EFMigrationsHistory",
                FgsFileDbContext.MigrationHistorySchema);
        });

        services.AddFgsPersistence<FgsFileDbContext>();

        services.AddAwsS3Services();
        services.AddSingleton<IS3ObjectKeyBuilder, S3ObjectKeyBuilder>();
        services.AddScoped<ITenantS3BucketProvisioner, TenantS3BucketProvisioner>();
        services.AddScoped<IS3ObjectStorageService, S3ObjectStorageService>();
        services.AddScoped<IImageVariantGenerator, ImageVariantGenerator>();
        services.AddSingleton<IFileContentUrlBuilder, FileContentUrlBuilder>();

        return services;
    }
}
