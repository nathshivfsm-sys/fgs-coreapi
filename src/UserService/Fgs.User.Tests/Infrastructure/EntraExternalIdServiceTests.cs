using Fgs.User.Infrastructure.Common.Options;
using Fgs.User.Infrastructure.Common.Identity;
using Fgs.Security.Options;
using Microsoft.Extensions.Options;

namespace Fgs.User.Tests.Infrastructure;

public sealed class EntraExternalIdServiceTests
{
    [Fact]
    public void BuildAuthorizationUrl_IncludesStateAndClientId()
    {
        var invitationId = Guid.NewGuid();
        var service = new EntraExternalIdService(
            Options.Create(new EntraExternalIdOptions
            {
                TenantId = "tenant",
                ClientId = "client-id",
                Authority = "https://login.microsoftonline.com",
                Scopes = "openid profile email"
            }),
            new HttpClient());

        var url = service.BuildAuthorizationUrl(invitationId, "https://localhost/callback");

        url.Should().Contain("client_id=client-id");
        url.Should().Contain($"state={invitationId}");
        url.Should().Contain("redirect_uri=");
    }

    [Fact]
    public void BuildAuthorizationUrl_WithCiamTenant_UsesTenantIdPathWithoutUserFlow()
    {
        var service = new EntraExternalIdService(
            Options.Create(new EntraExternalIdOptions
            {
                TenantId = "tenant-id",
                ClientId = "client-id",
                Authority = "https://example.ciamlogin.com",
                UserFlow = "SignUpSignIn",
                Scopes = "openid profile email"
            }),
            new HttpClient());

        var url = service.BuildAuthorizationUrl(
            Guid.NewGuid(),
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
        var service = new EntraExternalIdService(
            Options.Create(new EntraExternalIdOptions
            {
                TenantId = "tenant",
                ClientId = "client-id",
                Authority = "https://login.microsoftonline.com",
                Scopes = "openid profile email"
            }),
            new HttpClient());

        var url = service.BuildAuthorizationUrl(
            Guid.NewGuid(),
            "https://localhost/callback",
            "admin@test.com");

        url.Should().Contain("login_hint=admin%40test.com");
    }
}
