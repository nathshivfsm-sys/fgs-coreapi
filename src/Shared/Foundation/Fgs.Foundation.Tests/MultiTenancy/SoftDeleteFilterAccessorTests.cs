using Fgs.MultiTenancy;
using FluentAssertions;

namespace Fgs.Foundation.Tests.MultiTenancy;

public sealed class SoftDeleteFilterAccessorTests
{
    [Fact]
    public void IsEnabled_CanBeToggledAndIsSharedAcrossInstances()
    {
        var first = new SoftDeleteFilterAccessor();
        var second = new SoftDeleteFilterAccessor();

        first.IsEnabled = true;
        first.IsEnabled.Should().BeTrue();
        second.IsEnabled.Should().BeTrue();

        first.IsEnabled = false;
        second.IsEnabled.Should().BeFalse();

        second.IsEnabled = true;
        first.IsEnabled.Should().BeTrue();
    }
}
