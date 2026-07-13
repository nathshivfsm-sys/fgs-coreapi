using Fgs.Contracts.Auth;
using Fgs.MultiTenancy;
using Fgs.Security.Abstractions;
using Fgs.Security.Authorization;
using Fgs.Security.Middleware;
using Fgs.Security.UserAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;

namespace Fgs.Security.Tests.Middleware;

public sealed class ActiveUserAuthorizationMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_WithActiveProfile_AllowsRequest()
    {
        var nextCalled = false;
        var tenantAccessor = new TenantContextAccessor();
        var profileStore = CreateProfileStore(CreateProfile());
        var middleware = CreateMiddleware(_ =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        });

        var context = CreateHttpContext(authenticated: true, tenantId: 1, companyId: 1);
        var userContext = CreateUserContext(authenticated: true, entraObjectId: "oid-1");

        await middleware.InvokeAsync(context, userContext, profileStore.Object, tenantAccessor);

        nextCalled.Should().BeTrue();
        context.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        tenantAccessor.Current.Should().NotBeNull();
        tenantAccessor.Current!.TenantId.Should().Be(1);
        tenantAccessor.Current.CompanyId.Should().Be(1);
    }

    [Fact]
    public async Task InvokeAsync_WithInactiveProfile_ReturnsForbiddenWithMessage()
    {
        var profileStore = CreateProfileStore(CreateProfile(isActive: false));
        var middleware = CreateMiddleware(_ => Task.CompletedTask);

        var context = CreateHttpContext(authenticated: true, tenantId: 1, companyId: 1);
        var userContext = CreateUserContext(authenticated: true, entraObjectId: "oid-1");

        await middleware.InvokeAsync(
            context,
            userContext,
            profileStore.Object,
            new TenantContextAccessor());

        context.Response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
        var body = await ReadResponseBody(context);
        body.Should().Contain(UserAuthorizationMessages.UserInactive);
    }

    [Fact]
    public async Task InvokeAsync_WithTenantMismatch_ReturnsForbiddenWithMessage()
    {
        var profileStore = CreateProfileStore(CreateProfile(tenantId: 1));
        var middleware = CreateMiddleware(_ => Task.CompletedTask);

        var context = CreateHttpContext(authenticated: true, tenantId: 99, companyId: 1);
        var userContext = CreateUserContext(authenticated: true, entraObjectId: "oid-1");

        await middleware.InvokeAsync(
            context,
            userContext,
            profileStore.Object,
            new TenantContextAccessor());

        context.Response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
        var body = await ReadResponseBody(context);
        body.Should().Contain(UserAuthorizationMessages.TenantMismatch);
    }

    [Fact]
    public async Task InvokeAsync_WithTenantAdmin_AllowsCrossCompanyAccess()
    {
        var nextCalled = false;
        var tenantAccessor = new TenantContextAccessor();
        var profileStore = CreateProfileStore(CreateProfile(companyId: 1, roles: ["TENANT_ADMIN"]));
        var middleware = CreateMiddleware(_ =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        });

        var context = CreateHttpContext(authenticated: true, tenantId: 1, companyId: 42);
        var userContext = CreateUserContext(authenticated: true, entraObjectId: "oid-1");

        await middleware.InvokeAsync(context, userContext, profileStore.Object, tenantAccessor);

        nextCalled.Should().BeTrue();
        tenantAccessor.Current!.CompanyId.Should().Be(42);
    }

    [Fact]
    public async Task InvokeAsync_WithUnauthenticatedRequest_SkipsValidation()
    {
        var nextCalled = false;
        var middleware = CreateMiddleware(_ =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        });

        var context = CreateHttpContext(authenticated: false, allowAnonymous: true);
        var userContext = CreateUserContext(authenticated: false);

        await middleware.InvokeAsync(
            context,
            userContext,
            Mock.Of<IUserAuthProfileStore>(),
            new TenantContextAccessor());

        nextCalled.Should().BeTrue();
    }

    [Fact]
    public async Task InvokeAsync_WithAuthenticatedAllowAnonymousEndpoint_EnforcesValidation()
    {
        var profileStore = CreateProfileStore(CreateProfile(tenantId: 1));
        var middleware = CreateMiddleware(_ => Task.CompletedTask);

        var context = CreateHttpContext(authenticated: true, allowAnonymous: true, tenantId: 99, companyId: 1);
        context.Request.Path = "/api/v1/tenant/99";
        context.Request.RouteValues["tenantId"] = "99";
        var userContext = CreateUserContext(authenticated: true, entraObjectId: "oid-1");

        await middleware.InvokeAsync(
            context,
            userContext,
            profileStore.Object,
            new TenantContextAccessor());

        context.Response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
        var body = await ReadResponseBody(context);
        body.Should().Contain(UserAuthorizationMessages.TenantMismatch);
    }

    [Fact]
    public async Task InvokeAsync_WithInternalServiceKey_SkipsValidation()
    {
        var nextCalled = false;
        var middleware = CreateMiddleware(
            _ =>
            {
                nextCalled = true;
                return Task.CompletedTask;
            },
            internalServiceKey: "secret-key");

        var context = CreateHttpContext(authenticated: true);
        context.Request.Headers["X-FGS-Internal-Service-Key"] = "secret-key";
        var userContext = CreateUserContext(authenticated: true, entraObjectId: "oid-1");

        await middleware.InvokeAsync(
            context,
            userContext,
            Mock.Of<IUserAuthProfileStore>(),
            new TenantContextAccessor());

        nextCalled.Should().BeTrue();
    }

    private static ActiveUserAuthorizationMiddleware CreateMiddleware(
        RequestDelegate next,
        string? internalServiceKey = null) =>
        new(
            next,
            Microsoft.Extensions.Options.Options.Create(new TenantScopeOptions()),
            Microsoft.Extensions.Options.Options.Create(new InternalServiceKeyOptions
            {
                InternalServiceKey = internalServiceKey ?? string.Empty
            }));

    private static Mock<IUserAuthProfileStore> CreateProfileStore(UserAuthProfileDto? profile)
    {
        var mock = new Mock<IUserAuthProfileStore>();
        mock
            .Setup(s => s.GetOrLoadAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(profile);
        return mock;
    }

    private static UserAuthProfileDto CreateProfile(
        long tenantId = 1,
        long companyId = 1,
        bool isActive = true,
        IReadOnlyList<string>? roles = null) =>
        new(
            Guid.NewGuid(),
            "user@test.com",
            "oid-1",
            tenantId,
            companyId,
            isActive,
            false,
            roles ?? ["TENANT_ADMIN"]);

    private static IFgsUserContext CreateUserContext(bool authenticated, string? entraObjectId = null)
    {
        var mock = new Mock<IFgsUserContext>();
        mock.SetupGet(c => c.IsAuthenticated).Returns(authenticated);
        mock.SetupGet(c => c.EntraObjectId).Returns(entraObjectId);
        return mock.Object;
    }

    private static DefaultHttpContext CreateHttpContext(
        bool authenticated,
        bool allowAnonymous = false,
        long? tenantId = null,
        long? companyId = null)
    {
        var context = new DefaultHttpContext
        {
            Request =
            {
                Path = "/api/v1/tenants"
            }
        };

        if (allowAnonymous)
        {
            var endpoint = new Endpoint(
                _ => Task.CompletedTask,
                new EndpointMetadataCollection(new AllowAnonymousAttribute()),
                "test");
            context.SetEndpoint(endpoint);
        }

        if (authenticated)
        {
            context.User = new System.Security.Claims.ClaimsPrincipal(
                new System.Security.Claims.ClaimsIdentity("Bearer"));
        }

        if (tenantId.HasValue)
        {
            context.Request.Headers["X-Tenant-Id"] = tenantId.Value.ToString();
        }

        if (companyId.HasValue)
        {
            context.Request.Headers["X-Company-Id"] = companyId.Value.ToString();
        }

        context.Response.Body = new MemoryStream();
        return context;
    }

    private static async Task<string> ReadResponseBody(DefaultHttpContext context)
    {
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(context.Response.Body);
        return await reader.ReadToEndAsync();
    }
}
