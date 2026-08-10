using Fgs.Messaging.Consumer;

namespace Fgs.Messaging.Tests;

public sealed class ConsumerRetryPolicyTests
{
    [Fact]
    public void GetRetryCount_ReturnsZeroWhenHeaderMissing()
    {
        ConsumerRetryPolicy.GetRetryCount(null).Should().Be(0);
        ConsumerRetryPolicy.GetRetryCount(new Dictionary<string, object?>()).Should().Be(0);
    }

    [Fact]
    public void GetRetryCount_ReadsIntegerHeader()
    {
        var headers = new Dictionary<string, object?> { ["x-retry-count"] = 3 };
        ConsumerRetryPolicy.GetRetryCount(headers).Should().Be(3);
    }

    [Fact]
    public void GetRetryCount_ReadsByteArrayHeader()
    {
        var headers = new Dictionary<string, object?> { ["x-retry-count"] = "2"u8.ToArray() };
        ConsumerRetryPolicy.GetRetryCount(headers).Should().Be(2);
    }

    [Fact]
    public void BuildRetryHeaders_PreservesOriginalHeadersAndSetsRetryCount()
    {
        var original = new Dictionary<string, object?>
        {
            ["correlation_id"] = "corr-1",
            ["tenant_id"] = "10",
            ["x-retry-count"] = 1
        };

        var headers = ConsumerRetryPolicy.BuildRetryHeaders(original, nextRetryCount: 2);

        headers["correlation_id"].Should().Be("corr-1");
        headers["tenant_id"].Should().Be("10");
        headers["x-retry-count"].Should().Be(2);
        original["x-retry-count"].Should().Be(1);
    }

    [Fact]
    public void BuildRetryHeaders_WhenOriginalNull_CreatesRetryCountOnly()
    {
        var headers = ConsumerRetryPolicy.BuildRetryHeaders(null, nextRetryCount: 1);

        headers.Should().ContainSingle();
        headers["x-retry-count"].Should().Be(1);
    }
}
