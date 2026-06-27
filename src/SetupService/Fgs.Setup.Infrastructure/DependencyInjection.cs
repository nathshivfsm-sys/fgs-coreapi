using Fgs.Contracts.Clients;
using Fgs.Credentials.Extensions;
using Fgs.Credentials.Options;
using Fgs.Foundation.Caching.Options;
using Fgs.Setup.Application.Abstractions.Credentials;
using Fgs.Setup.Application.Abstractions.GLBreaks;
using Fgs.Setup.Application.Abstractions.Locations;
using Fgs.Setup.Application.Abstractions.Persistence;
using Fgs.Setup.Application.Abstractions.TechTrades;
using Fgs.Setup.Application.Abstractions.TitlesOfCourtesy;
using Fgs.Setup.Infrastructure.Audit;
using Fgs.Messaging.Abstractions;
using Fgs.Messaging.Options;
using Fgs.Setup.Application.Abstractions.Provisioning;
using Fgs.Setup.Application.Abstractions.Time;
using Fgs.Setup.Infrastructure.Common.Options;
using Fgs.Setup.Infrastructure.Common.Time;
using Fgs.Setup.Infrastructure.Database;
using Fgs.Setup.Infrastructure.Database.Read;
using Fgs.Setup.Infrastructure.Messaging;
using Fgs.Setup.Application.Abstractions.Tenants;
using Fgs.Setup.Infrastructure.Provisioning;
using Fgs.Setup.Infrastructure.Tenants;
using Fgs.Setup.Infrastructure.GLBreaks;
using Fgs.Setup.Infrastructure.TechTrades;
using Fgs.Setup.Infrastructure.TitlesOfCourtesy;
using Fgs.Setup.Application.Abstractions.CommunicationTemplates;
using Fgs.Setup.Application.Abstractions.SalesActivityOutcomes;
using Fgs.Setup.Application.Abstractions.SalesActivityTypes;
using Fgs.Setup.Application.Abstractions.SalesDispositionReasons;
using Fgs.Setup.Application.Abstractions.SalesPipelineStatuses;
using Fgs.Setup.Application.Abstractions.SetupDescriptions;
using Fgs.Setup.Application.Abstractions.SetupLaborRateTypes;
using Fgs.Setup.Application.Abstractions.SetupPaymentMethods;
using Fgs.Setup.Application.Abstractions.SetupPaymentTerms;
using Fgs.Setup.Application.Abstractions.SetupPostalCodes;
using Fgs.Setup.Application.Abstractions.SetupTaxAuthorities;
using Fgs.Setup.Application.Abstractions.SetupTaxes;
using Fgs.Setup.Application.Abstractions.SetupTechSkillLevels;
using Fgs.Setup.Application.Abstractions.SetupTimeSlots;
using Fgs.Setup.Application.Abstractions.SetupZones;
using Fgs.Setup.Application.Abstractions.Tags;
using Fgs.Setup.Application.Abstractions.VehicleMaintenances;
using Fgs.Setup.Application.Abstractions.Vehicles;
using Fgs.Setup.Application.Abstractions.Vendors;
using Fgs.Setup.Application.Abstractions.Warehouses;
using Fgs.Setup.Infrastructure.CommunicationTemplates;
using Fgs.Setup.Infrastructure.SalesActivityOutcomes;
using Fgs.Setup.Infrastructure.SalesActivityTypes;
using Fgs.Setup.Infrastructure.SalesDispositionReasons;
using Fgs.Setup.Infrastructure.SalesPipelineStatuses;
using Fgs.Setup.Infrastructure.SetupDescriptions;
using Fgs.Setup.Infrastructure.SetupLaborRateTypes;
using Fgs.Setup.Infrastructure.SetupPaymentMethods;
using Fgs.Setup.Infrastructure.SetupPaymentTerms;
using Fgs.Setup.Infrastructure.SetupPostalCodes;
using Fgs.Setup.Infrastructure.SetupTaxAuthorities;
using Fgs.Setup.Infrastructure.SetupTaxes;
using Fgs.Setup.Infrastructure.SetupTechSkillLevels;
using Fgs.Setup.Infrastructure.SetupTimeSlots;
using Fgs.Setup.Infrastructure.SetupZones;
using Fgs.Setup.Infrastructure.Tags;
using Fgs.Setup.Infrastructure.VehicleMaintenances;
using Fgs.Setup.Infrastructure.Vehicles;
using Fgs.Setup.Infrastructure.Vendors;
using Fgs.Setup.Infrastructure.Warehouses;
using Fgs.Setup.Application.Abstractions.BillingCategories;
using Fgs.Setup.Application.Abstractions.FgsBusinessTypes;
using Fgs.Setup.Application.Abstractions.JobTypeCategories;
using Fgs.Setup.Application.Abstractions.JobTypeSubCategories;
using Fgs.Setup.Application.Abstractions.JobTypes;
using Fgs.Setup.Application.Abstractions.LeadDisqualificationReasons;
using Fgs.Setup.Application.Abstractions.LeadSources;
using Fgs.Setup.Application.Abstractions.LeadStatuses;
using Fgs.Setup.Application.Abstractions.ResolutionCodes;
using Fgs.Setup.Infrastructure.BillingCategories;
using Fgs.Setup.Infrastructure.FgsBusinessTypes;
using Fgs.Setup.Infrastructure.JobTypeCategories;
using Fgs.Setup.Infrastructure.JobTypeSubCategories;
using Fgs.Setup.Infrastructure.JobTypes;
using Fgs.Setup.Infrastructure.LeadDisqualificationReasons;
using Fgs.Setup.Infrastructure.LeadSources;
using Fgs.Setup.Infrastructure.LeadStatuses;
using Fgs.Setup.Infrastructure.ResolutionCodes;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Foundation.Extensions;
using Fgs.Persistence.Extensions;
using Fgs.Security.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fgs.Setup.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsSetupInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddFgsApiSecurity(configuration);

        services.Configure<CredentialConsumerOptions>(options => options.ServiceName = "fgs-setup-service");

        services.Configure<TenantProvisioningOptions>(configuration.GetSection(TenantProvisioningOptions.SectionName));
        services.Configure<OutboxOptions>(configuration.GetSection(OutboxOptions.SectionName));

        var connectionString = FgsSetupConnectionString.ResolveRequired(configuration);
        services.AddDbContext<FgsSetupDbContext>((_, options) =>
        {
            options.UseFgsNpgsql(
                connectionString,
                "__EFMigrationsHistory",
                FgsSetupDbContext.MigrationHistorySchema);
            options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
        });

        services.AddFgsPersistence<FgsSetupDbContext>();

        var auditServiceEnabled = configuration.GetValue("AuditService:Enabled", true);
        if (auditServiceEnabled)
        {
            services.AddFgsRefitClient<IAuditClient>(
                configuration,
                "AuditService:BaseUrl",
                "http://audit-service:5003");
            services.AddScoped<ICredentialAuditRecorder, RefitCredentialAuditRecorder>();
        }
        else
        {
            services.AddSingleton<ICredentialAuditRecorder, NoOpCredentialAuditRecorder>();
        }

        services.AddFgsInternalServiceRefitClient<IUserTenantClient>(
            configuration,
            "UserService:BaseUrl",
            "http://user-service:5001");

        services.AddFgsInternalServiceRefitClient<IFileTenantClient>(
            configuration,
            "FileService:BaseUrl",
            "http://file-service:5005");

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<ISetupReadConnectionFactory, FgsSetupReadConnectionFactory>();
        services.AddScoped<ITechTradeReadRepository, TechTradeReadRepository>();
        services.AddScoped<ITitleOfCourtesyReadRepository, TitleOfCourtesyReadRepository>();
        services.AddScoped<SetupEntityAuditHelper>();
        services.AddScoped<ISetupLocationWriteService, SetupLocationWriteService>();
        services.AddScoped<ITechTradeWriteService, TechTradeWriteService>();
        services.AddScoped<ITitleOfCourtesyWriteService, TitleOfCourtesyWriteService>();
        services.AddScoped<IFgsSalesPipelineStatusReadRepository, FgsSalesPipelineStatusReadRepository>();
        services.AddScoped<IFgsSalesPipelineStatusWriteService, FgsSalesPipelineStatusWriteService>();
        services.AddScoped<IFgsSalesActivityTypeReadRepository, FgsSalesActivityTypeReadRepository>();
        services.AddScoped<IFgsSalesActivityTypeWriteService, FgsSalesActivityTypeWriteService>();
        services.AddScoped<IFgsSalesDispositionReasonReadRepository, FgsSalesDispositionReasonReadRepository>();
        services.AddScoped<IFgsSalesDispositionReasonWriteService, FgsSalesDispositionReasonWriteService>();
        services.AddScoped<IFgsSalesActivityOutcomeReadRepository, FgsSalesActivityOutcomeReadRepository>();
        services.AddScoped<IFgsSalesActivityOutcomeWriteService, FgsSalesActivityOutcomeWriteService>();
        services.AddScoped<IFgsSetupZoneReadRepository, FgsSetupZoneReadRepository>();
        services.AddScoped<IFgsSetupZoneWriteService, FgsSetupZoneWriteService>();
        services.AddScoped<IFgsSetupTechSkillLevelReadRepository, FgsSetupTechSkillLevelReadRepository>();
        services.AddScoped<IFgsSetupTechSkillLevelWriteService, FgsSetupTechSkillLevelWriteService>();
        services.AddScoped<IFgsSetupLaborRateTypeReadRepository, FgsSetupLaborRateTypeReadRepository>();
        services.AddScoped<IFgsSetupLaborRateTypeWriteService, FgsSetupLaborRateTypeWriteService>();
        services.AddScoped<IFgsSetupTaxReadRepository, FgsSetupTaxReadRepository>();
        services.AddScoped<IFgsSetupTaxWriteService, FgsSetupTaxWriteService>();
        services.AddScoped<IFgsSetupTaxAuthorityReadRepository, FgsSetupTaxAuthorityReadRepository>();
        services.AddScoped<IFgsSetupTaxAuthorityWriteService, FgsSetupTaxAuthorityWriteService>();
        services.AddScoped<IFgsSetupPostalCodeReadRepository, FgsSetupPostalCodeReadRepository>();
        services.AddScoped<IFgsSetupPostalCodeWriteService, FgsSetupPostalCodeWriteService>();
        services.AddScoped<IFgsSetupPaymentMethodReadRepository, FgsSetupPaymentMethodReadRepository>();
        services.AddScoped<IFgsSetupPaymentMethodWriteService, FgsSetupPaymentMethodWriteService>();
        services.AddScoped<IFgsSetupPaymentTermReadRepository, FgsSetupPaymentTermReadRepository>();
        services.AddScoped<IFgsSetupPaymentTermWriteService, FgsSetupPaymentTermWriteService>();
        services.AddScoped<IFgsSetupDescriptionReadRepository, FgsSetupDescriptionReadRepository>();
        services.AddScoped<IFgsSetupDescriptionWriteService, FgsSetupDescriptionWriteService>();
        services.AddScoped<IFgsSetupTimeSlotReadRepository, FgsSetupTimeSlotReadRepository>();
        services.AddScoped<IFgsSetupTimeSlotWriteService, FgsSetupTimeSlotWriteService>();
        services.AddScoped<IFgsSetupCommunicationTemplateReadRepository, FgsSetupCommunicationTemplateReadRepository>();
        services.AddScoped<IFgsSetupCommunicationTemplateWriteService, FgsSetupCommunicationTemplateWriteService>();
        services.AddScoped<IFgsTagReadRepository, FgsTagReadRepository>();
        services.AddScoped<IFgsTagWriteService, FgsTagWriteService>();
        services.AddScoped<IFgsWarehouseReadRepository, FgsWarehouseReadRepository>();
        services.AddScoped<IFgsWarehouseWriteService, FgsWarehouseWriteService>();
        services.AddScoped<IFgsVendorReadRepository, FgsVendorReadRepository>();
        services.AddScoped<IFgsVendorWriteService, FgsVendorWriteService>();
        services.AddScoped<IFgsVehicleReadRepository, FgsVehicleReadRepository>();
        services.AddScoped<IFgsVehicleWriteService, FgsVehicleWriteService>();
        services.AddScoped<IFgsVehicleMaintenanceReadRepository, FgsVehicleMaintenanceReadRepository>();
        services.AddScoped<IFgsVehicleMaintenanceWriteService, FgsVehicleMaintenanceWriteService>();
        services.AddScoped<IFgsBusinessTypeReadRepository, FgsBusinessTypeReadRepository>();
        services.AddScoped<IFgsBusinessTypeWriteService, FgsBusinessTypeWriteService>();
        services.AddScoped<IBillingCategoryReadRepository, BillingCategoryReadRepository>();
        services.AddScoped<IBillingCategoryWriteService, BillingCategoryWriteService>();
        services.AddScoped<IJobTypeCategoryReadRepository, JobTypeCategoryReadRepository>();
        services.AddScoped<IJobTypeCategoryWriteService, JobTypeCategoryWriteService>();
        services.AddScoped<IJobTypeSubCategoryReadRepository, JobTypeSubCategoryReadRepository>();
        services.AddScoped<IJobTypeSubCategoryWriteService, JobTypeSubCategoryWriteService>();
        services.AddScoped<IJobTypeReadRepository, JobTypeReadRepository>();
        services.AddScoped<IJobTypeWriteService, JobTypeWriteService>();
        services.AddScoped<ILeadDisqualificationReasonReadRepository, LeadDisqualificationReasonReadRepository>();
        services.AddScoped<ILeadDisqualificationReasonWriteService, LeadDisqualificationReasonWriteService>();
        services.AddScoped<ILeadSourceReadRepository, LeadSourceReadRepository>();
        services.AddScoped<ILeadSourceWriteService, LeadSourceWriteService>();
        services.AddScoped<ILeadStatusReadRepository, LeadStatusReadRepository>();
        services.AddScoped<ILeadStatusWriteService, LeadStatusWriteService>();
        services.AddScoped<IResolutionCodeReadRepository, ResolutionCodeReadRepository>();
        services.AddScoped<IResolutionCodeWriteService, ResolutionCodeWriteService>();
        services.AddScoped<IGLBreakReadRepository, GLBreakReadRepository>();
        services.AddScoped<IGLBreakWriteService, GLBreakWriteService>();
        services.AddSingleton<ITenantSeedDatabaseConnectionFactory>(sp =>
            new TenantSeedDatabaseConnectionFactory(
                connectionString,
                sp.GetRequiredService<IOptions<TenantProvisioningOptions>>()));
        services.AddScoped<ITenantDataSeedingEngine, TenantDataSeedingEngine>();
        services.AddScoped<ITenantProvisioningOrchestrator, TenantProvisioningOrchestrator>();
        services.AddScoped<ICompanyBusinessTypeService, CompanyBusinessTypeService>();
        services.AddScoped<IOutboxWriter, OutboxWriter>();
        CredentialServiceCollectionExtensions.AddFgsCredentialConfigurationServices(
            services,
            configuration,
            configuration,
            registerCredentialStoreDbContext: false);
        CredentialServiceCollectionExtensions.RegisterCredentialOptionsChangeSource<RedisCacheOptions>(services);

        return services;
    }
}
