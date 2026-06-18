using System.Security.Claims;
using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Security.Services;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Fgs.Security.Tests.Services;

public sealed class RemoteFgsClaimsEnricherTests
{
    [Fact]
    public async Task EnrichAsync_WhenProfileFound_AddsFgsClaims()
    {
        var claimsClient = new Mock<IFgsClaimsClient>();
        claimsClient
            .Setup(c => c.GetMeAsync(
                "Bearer token-123",
                10L,
                1L,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApiResponse<FgsAuthMeDto>.Ok(new FgsAuthMeDto(
                Guid.NewGuid(),
                "admin@test.com",
                "oid-123",
                ["TENANT_ADMIN"])));

        var httpContext = CreateHttpContext("Bearer token-123", "10", "1");
        var enricher = new RemoteFgsClaimsEnricher(
            claimsClient.Object,
            CreateAccessor(httpContext));

        var principal = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim("oid", "oid-123")
        ], authenticationType: "Bearer"));

        await enricher.EnrichAsync(principal);

        principal.FindAll(ClaimTypes.Role).Select(c => c.Value).Should().Contain("TENANT_ADMIN");
        FgsClaimsEnrichment.IsEnriched(principal).Should().BeTrue();
        claimsClient.Verify(
            c => c.GetMeAsync("Bearer token-123", 10L, 1L, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task EnrichAsync_WhenClaimsClientFails_DoesNotEnrich()
    {
        var claimsClient = new Mock<IFgsClaimsClient>();
        claimsClient
            .Setup(c => c.GetMeAsync(
                It.IsAny<string>(),
                It.IsAny<long?>(),
                It.IsAny<long?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApiResponse<FgsAuthMeDto>.Fail(["Unauthorized."], ApiStatusCodes.Unauthorized));

        var httpContext = CreateHttpContext("Bearer token-123", "10", "1");
        var enricher = new RemoteFgsClaimsEnricher(
            claimsClient.Object,
            CreateAccessor(httpContext));

        var principal = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim("oid", "oid-123")
        ], authenticationType: "Bearer"));

        await enricher.EnrichAsync(principal);

        FgsClaimsEnrichment.IsEnriched(principal).Should().BeFalse();
    }

    [Fact]
    public async Task EnrichAsync_WhenAlreadyEnriched_SkipsClaimsClient()
    {
        var claimsClient = new Mock<IFgsClaimsClient>();
        var httpContext = CreateHttpContext("Bearer token-123", "10", "1");
        var enricher = new RemoteFgsClaimsEnricher(
            claimsClient.Object,
            CreateAccessor(httpContext));

        var principal = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim(FgsClaimsEnrichment.EnrichmentClaimType, bool.TrueString)
        ], authenticationType: "Bearer"));

        await enricher.EnrichAsync(principal);

        claimsClient.Verify(
            c => c.GetMeAsync(
                It.IsAny<string>(),
                It.IsAny<long?>(),
                It.IsAny<long?>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    private static DefaultHttpContext CreateHttpContext(string authorization, string tenantId, string companyId)
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers.Authorization = authorization;
        httpContext.Request.Headers["X-Tenant-Id"] = tenantId;
        httpContext.Request.Headers["X-Company-Id"] = companyId;
        return httpContext;
    }

    private static IHttpContextAccessor CreateAccessor(HttpContext httpContext)
    {
        var accessor = new Mock<IHttpContextAccessor>();
        accessor.Setup(a => a.HttpContext).Returns(httpContext);
        return accessor.Object;
    }
}
