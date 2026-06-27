using Fgs.Foundation.Caching;
using Fgs.Foundation.Paging;

namespace Fgs.Foundation.Tests.Caching;

public sealed class CacheKeysTests
{
    [Fact]
    public void Build_ProducesExpectedFormat()
    {
        var key = CacheKeys.Build(10, 20, "vehicles", "42");

        key.Should().Be("tenant:10:company:20:vehicles:42");
    }

    [Fact]
    public void EntityPrefix_EndsWithColon()
    {
        var prefix = CacheKeys.EntityPrefix(1, 2, "vehicles");

        prefix.Should().Be("tenant:1:company:2:vehicles:");
    }

    [Fact]
    public void LookupSegment_IncludesActiveOnlyFlag()
    {
        CacheKeys.LookupSegment(true).Should().Be("lookup:activeOnly=true");
        CacheKeys.LookupSegment(false).Should().Be("lookup:activeOnly=false");
    }

    [Fact]
    public void ListActiveSegment_IsDeterministic()
    {
        var first = CacheKeys.ListActiveSegment(1, 25, "Name", SortDirection.Asc.ToString(), "search", "filters");
        var second = CacheKeys.ListActiveSegment(1, 25, "Name", SortDirection.Asc.ToString(), "search", "filters");

        first.Should().Be(second);
        first.Should().StartWith("list-active:");
    }

    [Fact]
    public void ListActiveSegment_ChangesWhenInputsChange()
    {
        var first = CacheKeys.ListActiveSegment(1, 25, "Name", SortDirection.Asc.ToString(), null, null);
        var second = CacheKeys.ListActiveSegment(2, 25, "Name", SortDirection.Asc.ToString(), null, null);

        first.Should().NotBe(second);
    }

    [Fact]
    public void Fingerprint_IsDeterministicForSameObject()
    {
        var filters = new { Code = "A", Active = true };

        CacheKeys.Fingerprint(filters).Should().Be(CacheKeys.Fingerprint(filters));
    }
}
