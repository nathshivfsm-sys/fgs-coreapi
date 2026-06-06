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
}
