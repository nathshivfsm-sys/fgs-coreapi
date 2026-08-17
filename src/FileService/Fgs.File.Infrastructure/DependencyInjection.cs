using Fgs.Contracts.Clients;
using Fgs.Credentials;
using Fgs.Credentials.Abstractions;
using Fgs.Credentials.Extensions;
using Fgs.File.Application.Abstractions.Persistence;
using Fgs.File.Application.Abstractions.Storage;
using Fgs.File.Application.Common.Options;
using Fgs.File.Application.Features.Attachments;
using Fgs.File.Application.Abstractions.Provisioning;
using Fgs.File.Infrastructure.Database;
using Fgs.File.Infrastructure.Persistence;
using Fgs.File.Infrastructure.Storage;
using Fgs.Foundation.Extensions;
using Fgs.Persistence.Extensions;
using Fgs.MultiTenancy;
using Fgs.Security.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Fgs.Credentials.Options;

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
                options.RequiredProviders = ["DATABASE", "AWS", "ENTRA_EXTERNAL_ID"];
            },
            typeof(AwsCredentialsOptions));

        services.AddFgsApiSecurity(configuration);
        services.AddFgsUserAuthProfileClient(configuration);
        services.Configure<AwsCredentialsOptions>(configuration.GetSection(AwsCredentialsOptions.SectionName));
        services.Configure<FileServiceOptions>(configuration.GetSection(FileServiceOptions.SectionName));
        services.Configure<AttachmentValidationOptions>(configuration.GetSection(AttachmentValidationOptions.SectionName));

        services.AddFgsInternalServiceRefitClient<IUserTenantClient>(
            configuration,
            "UserService:BaseUrl",
            "http://user-service:5001");

        services.AddFgsDbContext<FgsFileDbContext>((sp, options) =>
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
        services.AddFgsDbContextReadyCheck<FgsFileDbContext>();

        services.AddAwsS3Services();
        services.AddSingleton<IS3ObjectKeyBuilder, S3ObjectKeyBuilder>();
        services.AddScoped<ITenantS3BucketProvisioner, TenantS3BucketProvisioner>();
        services.AddScoped<IFileStorageService, S3FileStorageService>();
        services.AddScoped<IImageVariantGenerator, ImageVariantGenerator>();
        services.AddScoped<IThumbnailGenerator, AttachmentThumbnailGenerator>();
        services.AddScoped<IAttachmentReadRepository, AttachmentReadRepository>();

        return services;
    }
}
