using Fgs.User.Application.Features.TenantMenus;

namespace Fgs.User.Tests.Application;

public sealed class FgsTenantMenuTypeConverterTests
{
    private readonly FgsTenantMenuTypeConverter _converter = new();

    [Theory]
    [InlineData("Top Menu", FgsTenantMenuType.TopMenu)]
    [InlineData("Left Menu", FgsTenantMenuType.LeftMenu)]
    [InlineData("TopMenu", FgsTenantMenuType.TopMenu)]
    [InlineData("LeftMenu", FgsTenantMenuType.LeftMenu)]
    public void ConvertFrom_AcceptsWireAndMemberNames(string text, FgsTenantMenuType expected)
    {
        var converted = _converter.ConvertFrom(text);

        converted.Should().Be(expected);
        FgsTenantMenuTypeConverter.ToStoredValue(expected).Should().Be(
            expected == FgsTenantMenuType.TopMenu
                ? FgsTenantMenuTypeConverter.TopMenu
                : FgsTenantMenuTypeConverter.LeftMenu);
    }

    [Fact]
    public void CanConvertFrom_AcceptsString()
    {
        _converter.CanConvertFrom(typeof(string)).Should().BeTrue();
        _converter.CanConvertFrom(typeof(int)).Should().BeFalse();
    }

    [Fact]
    public void ConvertFrom_RejectsUnknownMenuType()
    {
        var act = () => _converter.ConvertFrom("PAGE");

        act.Should().Throw<FormatException>();
    }
}
