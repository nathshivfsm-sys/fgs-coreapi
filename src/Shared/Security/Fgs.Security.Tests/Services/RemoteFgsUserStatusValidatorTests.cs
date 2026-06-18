using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Security.Services;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Fgs.Security.Tests.Services;

public sealed class RemoteFgsUserStatusValidatorTests
{
    [Fact]
    public async Task IsActiveAsync_WhenValidateSucceeds_ReturnsTrue()
    {
        var claimsClient = new Mock<IFgsClaimsClient>();
        claimsClient
            .Setup(c => c.ValidateUserAsync(
                "Bearer token-123",
                10L,
                1L,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApiResponse<object>.Ok(new { }));

        var validator = new RemoteFgsUserStatusValidator(
            claimsClient.Object,
            CreateAccessor(CreateHttpContext("Bearer token-123", "10", "1")));

        var result = await validator.IsActiveAsync();

        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsActiveAsync_WhenValidateFails_ReturnsFalse()
    {
        var claimsClient = new Mock<IFgsClaimsClient>();
        claimsClient
            .Setup(c => c.ValidateUserAsync(
                It.IsAny<string>(),
                It.IsAny<long?>(),
                It.IsAny<long?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApiResponse<object>.Fail(["Unauthorized."], ApiStatusCodes.Unauthorized));

        var validator = new RemoteFgsUserStatusValidator(
            claimsClient.Object,
            CreateAccessor(CreateHttpContext("Bearer token-123", "10", "1")));

        var result = await validator.IsActiveAsync();

        result.Should().BeFalse();
    }

    [Fact]
    public async Task IsActiveAsync_WhenBearerTokenMissing_ReturnsFalse()
    {
        var claimsClient = new Mock<IFgsClaimsClient>();
        var validator = new RemoteFgsUserStatusValidator(
            claimsClient.Object,
            CreateAccessor(new DefaultHttpContext()));

        var result = await validator.IsActiveAsync();

        result.Should().BeFalse();
        claimsClient.Verify(
            c => c.ValidateUserAsync(
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
