using Fgs.Foundation.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fgs.Foundation.Tests.Hosting;

public sealed class FgsApiHostExtensionsTests
{
    private sealed class TestHostOptionsConfigurator : IConfigureOptions<FgsApiHostOptions>
    {
        public void Configure(FgsApiHostOptions options) =>
            options.PostAuthenticationMiddleware = _ => { };
    }

    [Fact]
    public void ApplyConfiguredHostOptions_AppliesRegisteredConfigurators()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IConfigureOptions<FgsApiHostOptions>, TestHostOptionsConfigurator>();
        var provider = services.BuildServiceProvider();

        var options = new FgsApiHostOptions();
        options.PostAuthenticationMiddleware.Should().BeNull();

        FgsApiHostExtensions.ApplyConfiguredHostOptions(provider, options);

        options.PostAuthenticationMiddleware.Should().NotBeNull();
    }
}
