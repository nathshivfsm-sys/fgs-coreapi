using System.Security.Claims;
using Fgs.Security.Constants;
using Fgs.Security.Services;
using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Infrastructure.Common.Security;
using Moq;

namespace Fgs.User.Tests.Infrastructure;

public sealed class DbFgsClaimsEnricherTests
{
    [Fact]
    public async Task EnrichAsync_WhenProfileFound_AddsFgsClaims()
    {
        var profile = new FgsUserProfile(
            Guid.NewGuid(),
            "admin@test.com",
            "oid-123",
            10,
            1,
            ["TENANT_ADMIN"]);

        var resolver = new Mock<IFgsUserProfileResolver>();
        resolver
            .Setup(r => r.ResolveByEntraObjectIdAsync("oid-123", It.IsAny<CancellationToken>()))
            .ReturnsAsync(profile);

        var principal = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim("oid", "oid-123")
        ], authenticationType: "Bearer"));

        await new DbFgsClaimsEnricher(resolver.Object).EnrichAsync(principal);

        principal.FindFirst(JwtClaimTypes.TenantId)!.Value.Should().Be("10");
        principal.FindFirst(JwtClaimTypes.CompanyId)!.Value.Should().Be("1");
        principal.FindAll(ClaimTypes.Role).Select(c => c.Value).Should().Contain("TENANT_ADMIN");
        FgsClaimsEnrichment.IsEnriched(principal).Should().BeTrue();
    }

    [Fact]
    public async Task EnrichAsync_WhenTokenScopeClaimsMismatch_DoesNotEnrich()
    {
        var profile = new FgsUserProfile(
            Guid.NewGuid(),
            "admin@test.com",
            "oid-123",
            10,
            1,
            ["TENANT_ADMIN"]);

        var resolver = new Mock<IFgsUserProfileResolver>();
        resolver
            .Setup(r => r.ResolveByEntraObjectIdAsync("oid-123", It.IsAny<CancellationToken>()))
            .ReturnsAsync(profile);

        var principal = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim("oid", "oid-123"),
            new Claim(JwtClaimTypes.TenantId, "99"),
            new Claim(JwtClaimTypes.CompanyId, "1")
        ], authenticationType: "Bearer"));

        await new DbFgsClaimsEnricher(resolver.Object).EnrichAsync(principal);

        FgsClaimsEnrichment.IsEnriched(principal).Should().BeFalse();
    }

    [Fact]
    public async Task EnrichAsync_WhenMatchingTokenScopeClaims_Enriches()
    {
        var profile = new FgsUserProfile(
            Guid.NewGuid(),
            "admin@test.com",
            "oid-123",
            10,
            1,
            ["TENANT_ADMIN"]);

        var resolver = new Mock<IFgsUserProfileResolver>();
        resolver
            .Setup(r => r.ResolveByEntraObjectIdAsync("oid-123", It.IsAny<CancellationToken>()))
            .ReturnsAsync(profile);

        var principal = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim("oid", "oid-123"),
            new Claim(JwtClaimTypes.TenantId, "10"),
            new Claim(JwtClaimTypes.CompanyId, "1")
        ], authenticationType: "Bearer"));

        await new DbFgsClaimsEnricher(resolver.Object).EnrichAsync(principal);

        FgsClaimsEnrichment.IsEnriched(principal).Should().BeTrue();
    }

    [Fact]
    public async Task EnrichAsync_WhenAlreadyEnriched_SkipsResolver()
    {
        var resolver = new Mock<IFgsUserProfileResolver>();
        var principal = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim(FgsClaimsEnrichment.EnrichmentClaimType, bool.TrueString)
        ], authenticationType: "Bearer"));

        await new DbFgsClaimsEnricher(resolver.Object).EnrichAsync(principal);

        resolver.Verify(
            r => r.ResolveByEntraObjectIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
