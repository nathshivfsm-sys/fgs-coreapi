using System.Text.Json;
using Fgs.Bff.Application.Features.Lookups;

namespace Fgs.Bff.Tests.Application;

public sealed class LookupItemMapperTests
{
    [Fact]
    public void Map_CountryCodePrimary_UsesCodeAsId()
    {
        using var doc = JsonDocument.Parse("""{"countryCode":"US","countryName":"United States","currencyCode":"USD"}""");
        var item = LookupItemMapper.Map(doc.RootElement);

        item.Id.Should().Be("US");
        item.Code.Should().Be("US");
        item.Name.Should().Be("United States");
        item.Extra.Should().ContainKey("currencyCode");
        item.Extra!["currencyCode"].Should().Be("USD");
    }

    [Fact]
    public void Map_IdAndName_MapsCoreFieldsAndExtra()
    {
        using var doc = JsonDocument.Parse("""{"id":42,"code":"HVAC","name":"HVAC Trade","displayOrder":3,"isActive":true}""");
        var item = LookupItemMapper.Map(doc.RootElement);

        item.Id.Should().Be("42");
        item.Code.Should().Be("HVAC");
        item.Name.Should().Be("HVAC Trade");
        item.DisplayOrder.Should().Be(3);
        item.Extra.Should().ContainKey("isActive");
        item.Extra!["isActive"].Should().Be(true);
    }
}
