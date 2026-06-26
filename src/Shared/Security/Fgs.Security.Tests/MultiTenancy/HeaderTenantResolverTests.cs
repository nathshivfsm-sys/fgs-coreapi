using Fgs.MultiTenancy;
using Microsoft.AspNetCore.Http;

namespace Fgs.Security.Tests.MultiTenancy;

public sealed class HeaderTenantResolverTests
{
    private readonly HeaderTenantResolver _resolver = new();

    [Fact]
    public void TryResolve_WhenHeadersPresent_ReturnsResolvedContext()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["X-Tenant-Id"] = "10";
        httpContext.Request.Headers["X-Company-Id"] = "1";

        var resolved = _resolver.TryResolve(httpContext, out var tenantContext);

        resolved.Should().BeTrue();
        tenantContext.TenantId.Should().Be(10);
        tenantContext.CompanyId.Should().Be(1);
    }

    [Fact]
    public void TryResolve_WhenHeadersMissing_ReturnsFalse()
    {
        var httpContext = new DefaultHttpContext();

        var resolved = _resolver.TryResolve(httpContext, out _);

        resolved.Should().BeFalse();
    }

    [Fact]
    public void TryResolve_WhenOnlyTenantHeaderPresent_ReturnsFalse()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["X-Tenant-Id"] = "10";

        var resolved = _resolver.TryResolve(httpContext, out _);

        resolved.Should().BeFalse();
    }
}
