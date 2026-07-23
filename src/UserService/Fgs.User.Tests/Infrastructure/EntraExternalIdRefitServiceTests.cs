using Fgs.Contracts.Clients;
using Fgs.User.Infrastructure.Common.Identity;
using Fgs.User.Infrastructure.Common.Options;
using Microsoft.Extensions.Options;
using Moq;

namespace Fgs.User.Tests.Infrastructure;

public sealed class EntraExternalIdRefitServiceTests
{
    [Fact]
    public void BuildAuthorizationUrl_IncludesStateAndClientId()
    {
        var invitationId = Guid.NewGuid();
        var service = CreateService(new EntraExternalIdOptions
        {
            TenantId = "tenant",
            ClientId = "client-id",
            Authority = "https://login.microsoftonline.com",
            Scopes = "openid profile email"
        });

        var url = service.BuildAuthorizationUrl(invitationId.ToString(), "https://localhost/callback");

        url.Should().Contain("client_id=client-id");
        url.Should().Contain($"state={invitationId}");
        url.Should().Contain("redirect_uri=");
    }

    [Fact]
    public void BuildAuthorizationUrl_WithCiamTenant_UsesTenantIdPathWithoutUserFlow()
    {
        var service = CreateService(new EntraExternalIdOptions
        {
            TenantId = "tenant-id",
            ClientId = "client-id",
            Authority = "https://example.ciamlogin.com",
            UserFlow = "SignUpSignIn",
            Scopes = "openid profile email"
        });

        var url = service.BuildAuthorizationUrl(
            Guid.NewGuid().ToString(),
            "https://localhost/callback",
            "admin@test.com");

        url.Should().StartWith("https://example.ciamlogin.com/tenant-id/oauth2/v2.0/authorize?");
        url.Should().NotContain("/SignUpSignIn/");
        url.Should().Contain("login_hint=admin%40test.com");
        url.Should().Contain("p=SignUpSignIn");
    }

    [Fact]
    public void BuildAuthorizationUrl_WithDisplayName_IncludesGivenAndFamilyName()
    {
        var service = CreateService(new EntraExternalIdOptions
        {
            TenantId = "tenant",
            ClientId = "client-id",
            Authority = "https://login.microsoftonline.com",
            Scopes = "openid profile email"
        });

        var url = service.BuildAuthorizationUrl(
            Guid.NewGuid().ToString(),
            "https://localhost/callback",
            "admin@test.com");

        url.Should().Contain("login_hint=admin%40test.com");
    }

    [Fact]
    public void BuildAuthorizationUrl_WithForceSignup_IncludesPromptCreate()
    {
        var service = CreateService(new EntraExternalIdOptions
        {
            TenantId = "tenant-id",
            ClientId = "client-id",
            Authority = "https://example.ciamlogin.com",
            UserFlow = "SignUpSignIn",
            Scopes = "openid profile email"
        });

        var url = service.BuildAuthorizationUrl(
            Guid.NewGuid().ToString(),
            "https://localhost/callback",
            "admin@test.com",
            forceSignup: true);

        url.Should().Contain("prompt=create");
        url.Should().Contain("login_hint=admin%40test.com");
        url.Should().Contain("p=SignUpSignIn");
    }

    [Fact]
    public void BuildAuthorizationUrl_WithoutForceSignup_OmitsPrompt()
    {
        var service = CreateService(new EntraExternalIdOptions
        {
            TenantId = "tenant",
            ClientId = "client-id",
            Authority = "https://login.microsoftonline.com",
            Scopes = "openid profile email"
        });

        var url = service.BuildAuthorizationUrl(
            Guid.NewGuid().ToString(),
            "https://localhost/callback",
            "admin@test.com");

        url.Should().NotContain("prompt=");
    }

    private static EntraExternalIdRefitService CreateService(EntraExternalIdOptions options) =>
        new(Options.Create(options), Mock.Of<IEntraOAuthClient>());
}
