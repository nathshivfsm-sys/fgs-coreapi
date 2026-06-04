using Amazon.S3;
using Fgs.File.Application.Abstractions.Provisioning;
using Fgs.File.Infrastructure.Common.Options;
using Fgs.File.Infrastructure.Database;
using Fgs.File.Infrastructure.Storage;
using Fgs.Foundation.Extensions;
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
        IConfiguration configuration)
    {
        services.AddFgsFoundation();
        services.AddFgsEntraAuthentication(configuration);
        services.AddFgsRemoteClaimsEnrichment(configuration);
        services.Configure<AwsCredentialsOptions>(configuration.GetSection(AwsCredentialsOptions.SectionName));

        var connectionString = FgsFileConnectionString.ResolveRequired(configuration);
        services.AddDbContext<FgsFileDbContext>((_, options) =>
        {
            options.UseNpgsql(connectionString, npgsql =>
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory", FgsFileDbContext.MigrationHistorySchema));
        });

        services.AddSingleton<AmazonS3Client>();
        services.AddScoped<ITenantS3BucketProvisioner, TenantS3BucketProvisioner>();

        return services;
    }
}
