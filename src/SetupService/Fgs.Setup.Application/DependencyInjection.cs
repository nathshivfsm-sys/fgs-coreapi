using System.Reflection;
using Fgs.Foundation.CatalogCrud;
using Fgs.Foundation.Extensions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Setup.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsSetupApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        services.AddFgsFoundation();
        services.AddCatalogCrudApplication();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        services.AddValidatorsFromAssembly(assembly);
        services.AddScoped<Features.Credentials.Services.CredentialMutationService>();
        return services;
    }
}
