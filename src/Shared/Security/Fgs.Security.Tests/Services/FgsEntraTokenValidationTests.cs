using System.Security.Claims;
using Fgs.Security.Options;
using Fgs.Security.Services;
using FluentAssertions;

namespace Fgs.Security.Tests.Services;

public sealed class FgsEntraTokenValidationTests
{
    private const string ClientId = "3c788340-59a5-4864-b1b4-4f9adeffcb37";

    [Fact]
    public void BuildValidIssuers_IncludesCiamAndStsWindowsNetFormats()
    {
        var options = new EntraExternalIdAuthOptions
        {
            TenantId = "f9417a96-cb71-4919-8332-7087f1ad0455",
            Authority = "https://fsdemoapp.ciamlogin.com",
            ClientId = ClientId
        };

        var issuers = FgsEntraTokenValidation.BuildValidIssuers(options);

        issuers.Should().Contain("https://fsdemoapp.ciamlogin.com/f9417a96-cb71-4919-8332-7087f1ad0455/v2.0");
        issuers.Should().Contain("https://f9417a96-cb71-4919-8332-7087f1ad0455.ciamlogin.com/f9417a96-cb71-4919-8332-7087f1ad0455/v2.0");
        issuers.Should().Contain("https://sts.windows.net/f9417a96-cb71-4919-8332-7087f1ad0455/");
        issuers.Should().Contain("https://login.microsoftonline.com/f9417a96-cb71-4919-8332-7087f1ad0455/v2.0");
    }

    [Fact]
    public void ValidateGraphAudienceAppId_AllowsMatchingAppIdForGraphAudience()
    {
        var principal = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim("aud", FgsEntraTokenValidation.MicrosoftGraphAudience),
            new Claim("appid", ClientId)
        ]));

        FgsEntraTokenValidation.ValidateGraphAudienceAppId(principal, ClientId).Should().BeTrue();
    }

    [Fact]
    public void ValidateGraphAudienceAppId_RejectsMismatchedAppIdForGraphAudience()
    {
        var principal = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim("aud", FgsEntraTokenValidation.MicrosoftGraphAudience),
            new Claim("appid", "00000000-0000-0000-0000-000000000000")
        ]));

        FgsEntraTokenValidation.ValidateGraphAudienceAppId(principal, ClientId).Should().BeFalse();
    }
}
