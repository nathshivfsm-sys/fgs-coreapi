using Fgs.Asset.Application.Abstractions.AssetAttributeOptions;
using Fgs.Asset.Application.Abstractions.AssetAttributes;
using Fgs.Asset.Application.Abstractions.AssetAttributeValues;
using Fgs.Asset.Application.Abstractions.AssetManufacturers;
using Fgs.Asset.Application.Abstractions.AssetModels;
using Fgs.Asset.Application.Abstractions.Assets;
using Fgs.Asset.Application.Abstractions.AssetStatuses;
using Fgs.Asset.Application.Abstractions.AssetTypes;
using Fgs.Asset.Application.Abstractions.AssetWarranties;
using Fgs.Asset.Application.Abstractions.Persistence;
using Fgs.Asset.Infrastructure.AssetAttributeOptions;
using Fgs.Asset.Infrastructure.AssetAttributes;
using Fgs.Asset.Infrastructure.AssetAttributeValues;
using Fgs.Asset.Infrastructure.AssetManufacturers;
using Fgs.Asset.Infrastructure.AssetModels;
using Fgs.Asset.Infrastructure.Assets;
using Fgs.Asset.Infrastructure.AssetStatuses;
using Fgs.Asset.Infrastructure.AssetTypes;
using Fgs.Asset.Infrastructure.AssetWarranties;
using Fgs.Asset.Infrastructure.Common;
using Fgs.Asset.Infrastructure.Database;
using Fgs.Asset.Infrastructure.Database.Read;
using Fgs.Credentials.Abstractions;
using Fgs.Credentials.Extensions;
using Fgs.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Asset.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsAssetInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddFgsStandardInfrastructure(configuration, "fgs-asset-service", "DATABASE");
        services.AddFgsDbContext<FgsAssetDbContext>((sp, options) =>
        {
            var appConfiguration = sp.GetRequiredService<IConfiguration>();
            var credentialProvider = sp.GetService<ICredentialConfigurationProvider>();
            var connectionString = FgsAssetConnectionString.ResolveRequired(appConfiguration, credentialProvider);
            options.UseFgsNpgsql(connectionString, "__EFMigrationsHistory", FgsAssetDbContext.MigrationHistorySchema);
        });
        services.AddFgsPersistence<FgsAssetDbContext>();
        services.AddFgsDbContextReadyCheck<FgsAssetDbContext>();
        services.AddSingleton<IAssetReadConnectionFactory, FgsAssetReadConnectionFactory>();
        services.AddScoped<AssetEntityAuditHelper>();
        services.AddScoped<IFgsAssetTypeReadRepository, FgsAssetTypeReadRepository>();
        services.AddScoped<IFgsAssetTypeWriteService, FgsAssetTypeWriteService>();
        services.AddScoped<IFgsAssetManufacturerReadRepository, FgsAssetManufacturerReadRepository>();
        services.AddScoped<IFgsAssetManufacturerWriteService, FgsAssetManufacturerWriteService>();
        services.AddScoped<IFgsAssetStatusReadRepository, FgsAssetStatusReadRepository>();
        services.AddScoped<IFgsAssetStatusWriteService, FgsAssetStatusWriteService>();
        services.AddScoped<IFgsAssetModelReadRepository, FgsAssetModelReadRepository>();
        services.AddScoped<IFgsAssetModelWriteService, FgsAssetModelWriteService>();
        services.AddScoped<IFgsAssetAttributeReadRepository, FgsAssetAttributeReadRepository>();
        services.AddScoped<IFgsAssetAttributeWriteService, FgsAssetAttributeWriteService>();
        services.AddScoped<IFgsAssetAttributeOptionReadRepository, FgsAssetAttributeOptionReadRepository>();
        services.AddScoped<IFgsAssetAttributeOptionWriteService, FgsAssetAttributeOptionWriteService>();
        services.AddScoped<IFgsAssetReadRepository, FgsAssetReadRepository>();
        services.AddScoped<IFgsAssetWriteService, FgsAssetWriteService>();
        services.AddScoped<IFgsAssetWarrantyReadRepository, FgsAssetWarrantyReadRepository>();
        services.AddScoped<IFgsAssetWarrantyWriteService, FgsAssetWarrantyWriteService>();
        services.AddScoped<IFgsAssetAttributeValueReadRepository, FgsAssetAttributeValueReadRepository>();
        services.AddScoped<IFgsAssetAttributeValueWriteService, FgsAssetAttributeValueWriteService>();
        return services;
    }
}
