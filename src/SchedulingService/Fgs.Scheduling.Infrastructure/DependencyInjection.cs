using Fgs.Credentials;
using Fgs.Credentials.Abstractions;
using Fgs.Credentials.Extensions;
using Fgs.Persistence.Extensions;
using Fgs.Scheduling.Application.Abstractions.Appointments;
using Fgs.Scheduling.Infrastructure.Common;
using Fgs.Scheduling.Infrastructure.Database;
using Fgs.Scheduling.Infrastructure.Persistence.Appointments;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Scheduling.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsSchedulingInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddFgsStandardInfrastructure(configuration, "fgs-scheduling-service", "DATABASE");

        services.AddFgsDbContext<FgsSchedulingDbContext>((sp, options) =>
        {
            var connectionString = ConnectionStringResolver.ResolveRequired(
                sp.GetRequiredService<IConfiguration>(),
                ConnectionStringNames.FgsDispatch,
                "FGS_DISPATCH_DB",
                sp.GetService<ICredentialConfigurationProvider>());
            options.UseFgsNpgsql(
                connectionString,
                "__EFMigrationsHistory",
                FgsSchedulingDbContext.MigrationHistorySchema);
        });

        services.AddFgsPersistence<FgsSchedulingDbContext>();
        services.AddFgsDbContextReadyCheck<FgsSchedulingDbContext>();

        services.AddScoped<SchedulingEntityAuditHelper>();
        services.AddScoped<IFgsAppointmentReadRepository, FgsAppointmentReadRepository>();
        services.AddScoped<IFgsAppointmentWriteService, FgsAppointmentWriteService>();

        return services;
    }
}
