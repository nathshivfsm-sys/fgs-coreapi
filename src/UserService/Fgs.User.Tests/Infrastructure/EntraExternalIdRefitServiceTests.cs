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

    [Fact]
    public void BuildAuthorizationUrl_WithUserFlowOverride_UsesOverrideForP()
    {
        var service = CreateService(new EntraExternalIdOptions
        {
            TenantId = "tenant",
            ClientId = "client-id",
            Authority = "https://example.ciamlogin.com",
            UserFlow = "Fgs_SignUpSignIn",
            PasswordUserFlow = "Fgs_SignUpSignIn_Pwd",
            Scopes = "openid profile email"
        });

        var url = service.BuildAuthorizationUrl(
            Guid.NewGuid().ToString(),
            "https://localhost/callback",
            "admin@test.com",
            forceSignup: true,
            userFlow: "Fgs_SignUpSignIn_Pwd");

        url.Should().Contain("p=Fgs_SignUpSignIn_Pwd");
        url.Should().Contain("prompt=create");
    }

    [Fact]
    public void BuildLoginAuthorizationUrl_WithUserFlowOverride_UsesOverrideForP()
    {
        var service = CreateService(new EntraExternalIdOptions
        {
            TenantId = "tenant",
            ClientId = "client-id",
            Authority = "https://example.ciamlogin.com",
            UserFlow = "Fgs_SignUpSignIn",
            Scopes = "openid profile email"
        });

        var url = service.BuildLoginAuthorizationUrl(
            Guid.NewGuid().ToString(),
            "https://localhost:3000/auth/callback",
            "challenge",
            "admin@test.com",
            userFlow: "Fgs_SignUpSignIn_Pwd");

        url.Should().Contain("p=Fgs_SignUpSignIn_Pwd");
        url.Should().Contain("code_challenge=challenge");
        url.Should().NotContain("prompt=");
    }

    [Fact]
    public void ParseUserClaims_WhenEmailOnlyOnIdToken_UsesIdToken()
    {
        var accessToken = CreateJwt("""{"oid":"user-oid","aud":"api"}""");
        var idToken = CreateJwt("""{"oid":"user-oid","email":"admin@test.com","name":"Admin"}""");

        var (oid, email, name) = EntraExternalIdRefitService.ParseUserClaims(accessToken, idToken);

        oid.Should().Be("user-oid");
        email.Should().Be("admin@test.com");
        name.Should().Be("Admin");
    }

    [Fact]
    public void ParseUserClaims_WhenEmailsArrayOnIdToken_UsesFirstEmail()
    {
        var accessToken = CreateJwt("""{"sub":"user-sub"}""");
        var idToken = CreateJwt("""{"emails":["first@test.com","second@test.com"]}""");

        var (oid, email, _) = EntraExternalIdRefitService.ParseUserClaims(accessToken, idToken);

        oid.Should().Be("user-sub");
        email.Should().Be("first@test.com");
    }

    [Fact]
    public void ParseUserClaims_WhenEmailMissingEverywhere_Throws()
    {
        var accessToken = CreateJwt("""{"oid":"user-oid"}""");
        var idToken = CreateJwt("""{"oid":"user-oid","name":"Admin"}""");

        var act = () => EntraExternalIdRefitService.ParseUserClaims(accessToken, idToken);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*oid, email*");
    }

    private static EntraExternalIdRefitService CreateService(EntraExternalIdOptions options) =>
        new(Options.Create(options), Mock.Of<IEntraOAuthClient>());

    private static string CreateJwt(string payloadJson)
    {
        static string B64(string value) =>
            Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(value))
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');

        return $"{B64("{}")}.{B64(payloadJson)}.sig";
    }
}
