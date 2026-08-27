using Fgs.User.Application.Common;
using Microsoft.Extensions.Configuration;

namespace Fgs.User.Tests.Application;

public sealed class ApplicationPublicUrlResolverTests
{
    [Fact]
    public void Resolve_PrefersPublicBaseUrlOverCredentialRedirectUri()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Application:PublicBaseUrl"] = "http://100.54.14.213",
                ["Application:PublicServicePath"] = "user-service",
                ["EntraExternalId:RedirectUri"] = "https://localhost:8443/api/v1/auth/entra/callback",
                ["Invitation:InviteBaseUrl"] = "https://localhost:8443/api/v1/invite/start",
                ["Application:DashboardUrl"] = "https://localhost:8443/api/v1/dashboard"
            })
            .Build();

        ApplicationPublicUrlResolver.ResolveEntraCallbackRedirect(configuration)
            .Should().Be("http://100.54.14.213/user-service/api/v1/auth/entra/callback");
        ApplicationPublicUrlResolver.ResolveInviteBaseUrl(configuration)
            .Should().Be("http://100.54.14.213/user-service/api/v1/invite/start");
        ApplicationPublicUrlResolver.ResolveDashboardUrl(configuration)
            .Should().Be("http://100.54.14.213/user-service/api/v1/dashboard");
        ApplicationPublicUrlResolver.ResolveLoginRedirect(configuration)
            .Should().Be("http://100.54.14.213/user-service/api/v1/auth/entra/callback");
    }

    [Fact]
    public void Resolve_OmitsServicePathWhenNotConfigured()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Application:PublicBaseUrl"] = "https://developer.fsm.com"
            })
            .Build();

        ApplicationPublicUrlResolver.ResolveInviteBaseUrl(configuration)
            .Should().Be("https://developer.fsm.com/api/v1/invite/start");
    }

    [Fact]
    public void Resolve_FallsBackToConfiguredUrlsWhenPublicBaseMissing()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["EntraExternalId:RedirectUri"] = "https://localhost:8443/api/v1/auth/entra/callback",
                ["Invitation:InviteBaseUrl"] = "https://localhost:8443/api/v1/invite/start"
            })
            .Build();

        ApplicationPublicUrlResolver.ResolveEntraCallbackRedirect(configuration)
            .Should().Be("https://localhost:8443/api/v1/auth/entra/callback");
        ApplicationPublicUrlResolver.ResolveInviteBaseUrl(configuration)
            .Should().Be("https://localhost:8443/api/v1/invite/start");
    }
}
