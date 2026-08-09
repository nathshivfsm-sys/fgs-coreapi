using Fgs.Credentials;
using Fgs.Credentials.Options;
using FluentAssertions;

namespace Fgs.Credentials.Tests;

public sealed class InternalServiceAuthorizationTests
{
    [Fact]
    public void IsAuthorized_Rejects_WhenNoKeysConfigured()
    {
        var options = new CredentialDistributionOptions();
        InternalServiceAuthorization.IsAuthorized("any", options).Should().BeFalse();
    }

    [Fact]
    public void IsAuthorized_Accepts_PrimaryKey()
    {
        var options = new CredentialDistributionOptions { InternalServiceKey = "primary" };
        InternalServiceAuthorization.IsAuthorized("primary", options).Should().BeTrue();
        InternalServiceAuthorization.IsAuthorized("other", options).Should().BeFalse();
    }

    [Fact]
    public void IsAuthorized_Accepts_AdditionalKey_DuringRotation()
    {
        var options = new CredentialDistributionOptions
        {
            InternalServiceKey = "new-key",
            AdditionalInternalServiceKeys = ["old-key", " "]
        };

        InternalServiceAuthorization.IsAuthorized("new-key", options).Should().BeTrue();
        InternalServiceAuthorization.IsAuthorized("old-key", options).Should().BeTrue();
        InternalServiceAuthorization.IsAuthorized("unknown", options).Should().BeFalse();
    }

    [Fact]
    public void IsAuthorized_Accepts_AdditionalOnly_WhenPrimaryEmpty()
    {
        var options = new CredentialDistributionOptions
        {
            InternalServiceKey = string.Empty,
            AdditionalInternalServiceKeys = ["rotating-key"]
        };

        InternalServiceAuthorization.IsAuthorized("rotating-key", options).Should().BeTrue();
    }
}
