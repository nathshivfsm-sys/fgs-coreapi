using Fgs.Foundation.Hosting;
using Fgs.Security.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Security.Tests.Hosting;

public sealed class ActiveUserValidationHostOptionsTests
{
    [Fact]
    public void AddFgsActiveUserValidation_RegistersActiveUserAuthorizationMiddleware()
    {
        var services = new ServiceCollection();
        services.AddFgsActiveUserValidation(new ConfigurationBuilder().Build());
        var provider = services.BuildServiceProvider();

        var options = new FgsApiHostOptions();
        FgsApiHostExtensions.ApplyConfiguredHostOptions(provider, options);

        options.PostAuthenticationMiddleware.Should().NotBeNull();
    }
}
