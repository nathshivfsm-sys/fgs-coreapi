using Fgs.User.Application.Features.Signup;

namespace Fgs.User.Tests.Application;

public sealed class AddressCountryNormalizerTests
{
    [Theory]
    [InlineData("US", "TX", "US")]
    [InlineData("us", "NY", "US")]
    [InlineData("USA", "TX", "US")]
    [InlineData("United States", "TX", "US")]
    [InlineData("Canada", "ON", "CA")]
    [InlineData("CA", "ON", "CA")]
    [InlineData("United Kingdom", "ENG", "GB")]
    [InlineData("Australia", "NSW", "AU")]
    public void ResolveCountryCode_WithExplicitCountry_ReturnsNormalizedCode(
        string country,
        string state,
        string expected)
    {
        AddressCountryNormalizer.ResolveCountryCode(country, state).Should().Be(expected);
    }

    [Theory]
    [InlineData(null, "TX", "US")]
    [InlineData("", "CA", "US")]
    [InlineData("  ", "ON", "CA")]
    public void ResolveCountryCode_WhenCountryMissing_InfersFromState(string? country, string state, string expected)
    {
        AddressCountryNormalizer.ResolveCountryCode(country, state).Should().Be(expected);
    }

    [Fact]
    public void ResolveCountryCode_WhenCountryAndStateUnknown_ReturnsEmpty()
    {
        AddressCountryNormalizer.ResolveCountryCode(null, "ZZ").Should().BeEmpty();
    }

    [Fact]
    public void ResolveCountryCode_WhenLongUnknownCountryName_ReturnsEmpty()
    {
        AddressCountryNormalizer.ResolveCountryCode("Not A Real Country Name", "TX").Should().BeEmpty();
    }

    [Theory]
    [InlineData("XYZ")]
    [InlineData("ZZZ")]
    public void ResolveCountryCode_WhenShortUnknownCountryCode_ReturnsUppercased(string country)
    {
        AddressCountryNormalizer.ResolveCountryCode(country, "TX").Should().Be(country);
    }
}
