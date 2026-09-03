using Fgs.User.Application.Common;
using Microsoft.Extensions.Configuration;

namespace Fgs.User.Tests.Application;

public sealed class ApplicationPublicUrlResolverTests
{
    [Fact]
    public void ResolveUiAuthCallback_PrefersExplicitUiUrlOverEntraKeys()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Application:PublicBaseUrl"] = "http://100.54.14.213",
                ["Application:PublicServicePath"] = "user-service",
                ["Application:UiAuthCallbackUrl"] = "https://app.example.com/auth/callback",
                ["EntraExternalId:LoginRedirectUri"] = "https://ignored.example/login",
                ["EntraExternalId:RedirectUri"] = "https://ignored.example/redirect",
                ["Invitation:InviteBaseUrl"] = "https://localhost:8443/api/v1/invite/start"
            })
            .Build();

        ApplicationPublicUrlResolver.ResolveUiAuthCallbackUrl(configuration)
            .Should().Be("https://app.example.com/auth/callback");
        ApplicationPublicUrlResolver.ResolveLoginRedirect(configuration)
            .Should().Be("https://app.example.com/auth/callback");
        ApplicationPublicUrlResolver.ResolveInviteBaseUrl(configuration)
            .Should().Be("http://100.54.14.213/user-service/api/v1/invite/start");
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
    public void Resolve_FallsBackToLoginRedirectUriWhenUiAuthMissing()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["EntraExternalId:LoginRedirectUri"] = "https://app.local/auth/callback",
                ["Invitation:InviteBaseUrl"] = "https://localhost:8443/api/v1/invite/start"
            })
            .Build();

        ApplicationPublicUrlResolver.ResolveLoginRedirect(configuration)
            .Should().Be("https://app.local/auth/callback");
        ApplicationPublicUrlResolver.ResolveInviteBaseUrl(configuration)
            .Should().Be("https://localhost:8443/api/v1/invite/start");
    }

    [Fact]
    public void ResolveUiAuthCallback_DoesNotUsePublicBaseApiPath()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Application:PublicBaseUrl"] = "https://dev-api.fieldwhizey.com",
                ["Application:PublicServicePath"] = "user-service"
            })
            .Build();

        ApplicationPublicUrlResolver.ResolveUiAuthCallbackUrl(configuration)
            .Should().Be(ApplicationUrlDefaults.UiAuthCallback);
        ApplicationPublicUrlResolver.ResolveUiAuthCallbackUrl(configuration)
            .Should().NotContain("/api/v1/auth/entra/callback");
    }
}
