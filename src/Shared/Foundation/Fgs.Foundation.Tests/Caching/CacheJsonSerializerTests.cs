using Fgs.Foundation.Caching;
using Fgs.Foundation.Paging;

namespace Fgs.Foundation.Tests.Caching;

public sealed class CacheJsonSerializerTests
{
    private sealed record SampleDto(string Name, SortDirection Direction);

    [Fact]
    public void SerializeAndDeserialize_RoundTripsDto()
    {
        var original = new SampleDto("vehicle", SortDirection.Desc);
        var bytes = CacheJsonSerializer.Serialize(original);
        var restored = CacheJsonSerializer.Deserialize<SampleDto>(bytes);

        restored.Should().NotBeNull();
        restored!.Name.Should().Be("vehicle");
        restored.Direction.Should().Be(SortDirection.Desc);
    }
}
