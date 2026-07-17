using Fgs.Contracts.Auth;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Microsoft.AspNetCore.Http;

namespace Fgs.Security.Tests.Authorization;

public sealed class UserAuthorizationEvaluatorTests
{
    private static readonly TenantScopeOptions DefaultOptions = new();

    [Fact]
    public void Evaluate_WithNullProfile_ReturnsProfileNotFound()
    {
        var result = UserAuthorizationEvaluator.Evaluate(
            CreateContext(),
            profile: null,
            DefaultOptions);

        result.Success.Should().BeFalse();
        result.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
        result.ErrorMessage.Should().Be(UserAuthorizationMessages.ProfileNotFound);
    }

    [Fact]
    public void Evaluate_WithInactiveProfile_ReturnsUserInactive()
    {
        var result = Evaluate(CreateProfile(isActive: false));

        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Be(UserAuthorizationMessages.UserInactive);
    }

    [Fact]
    public void Evaluate_WithDeletedProfile_ReturnsUserDeleted()
    {
        var result = Evaluate(CreateProfile(isDeleted: true));

        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Be(UserAuthorizationMessages.UserDeleted);
    }

    [Fact]
    public void Evaluate_OnSkipPath_ReturnsSuccessWithoutScope()
    {
        var context = CreateContext("/api/v1/auth/me");
        var result = UserAuthorizationEvaluator.Evaluate(context, CreateProfile(), DefaultOptions);

        result.Success.Should().BeTrue();
        result.ValidatedScope.Should().BeNull();
    }

    [Fact]
    public void Evaluate_WithoutTenantScope_ReturnsBadRequest()
    {
        var result = UserAuthorizationEvaluator.Evaluate(
            CreateContext(),
            CreateProfile(),
            DefaultOptions);

        result.Success.Should().BeFalse();
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.ErrorMessage.Should().Be(UserAuthorizationMessages.TenantScopeMissing);
    }

    [Fact]
    public void Evaluate_WithTenantMismatch_ReturnsForbidden()
    {
        var context = CreateContext(tenantId: 99, companyId: 1);
        var result = UserAuthorizationEvaluator.Evaluate(
            context,
            CreateProfile(tenantId: 1, companyId: 1, roles: ["FIELD_TECH"]),
            DefaultOptions);

        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Be(UserAuthorizationMessages.TenantMismatch);
    }

    [Fact]
    public void Evaluate_WithNonAdminCompanyMismatch_ReturnsForbidden()
    {
        var context = CreateContext(tenantId: 1, companyId: 99);
        var result = UserAuthorizationEvaluator.Evaluate(
            context,
            CreateProfile(tenantId: 1, companyId: 1, roles: ["FIELD_TECH"]),
            DefaultOptions);

        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Be(UserAuthorizationMessages.CompanyMismatch);
    }

    [Fact]
    public void Evaluate_WithNonAdminMatchingScope_ReturnsValidatedScope()
    {
        var context = CreateContext(tenantId: 1, companyId: 1);
        var result = UserAuthorizationEvaluator.Evaluate(
            context,
            CreateProfile(tenantId: 1, companyId: 1, roles: ["FIELD_TECH"]),
            DefaultOptions);

        result.Success.Should().BeTrue();
        result.ValidatedScope.Should().Be(new ValidatedUserScope(1, 1));
    }

    [Fact]
    public void Evaluate_WithNonAdminMissingCompanyHeader_UsesProfileCompany()
    {
        var context = CreateContext(tenantId: 1);
        var result = UserAuthorizationEvaluator.Evaluate(
            context,
            CreateProfile(tenantId: 1, companyId: 5, roles: ["FIELD_TECH"]),
            DefaultOptions);

        result.Success.Should().BeTrue();
        result.ValidatedScope.Should().Be(new ValidatedUserScope(1, 5));
    }

    [Fact]
    public void Evaluate_WithTenantAdminAndDifferentCompany_ReturnsValidatedScope()
    {
        var context = CreateContext(tenantId: 1, companyId: 42);
        var result = UserAuthorizationEvaluator.Evaluate(
            context,
            CreateProfile(tenantId: 1, companyId: 1, roles: [FgsRoleCodes.TenantAdmin]),
            DefaultOptions);

        result.Success.Should().BeTrue();
        result.ValidatedScope.Should().Be(new ValidatedUserScope(1, 42));
    }

    [Fact]
    public void Evaluate_WithTenantAdminMissingCompany_ReturnsBadRequest()
    {
        var context = CreateContext(tenantId: 1);
        var result = UserAuthorizationEvaluator.Evaluate(
            context,
            CreateProfile(tenantId: 1, companyId: 1, roles: [FgsRoleCodes.TenantAdmin]),
            DefaultOptions);

        result.Success.Should().BeFalse();
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.ErrorMessage.Should().Be(UserAuthorizationMessages.CompanyScopeMissing);
    }

    [Fact]
    public void Evaluate_WithConflictingRouteTenant_ReturnsRouteTenantMismatch()
    {
        var context = CreateContext(tenantId: 1, companyId: 1, routeTenantId: 2);
        var result = UserAuthorizationEvaluator.Evaluate(
            context,
            CreateProfile(tenantId: 1, companyId: 1, roles: ["FIELD_TECH"]),
            DefaultOptions);

        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Be(UserAuthorizationMessages.RouteTenantMismatch);
    }

    [Fact]
    public void Evaluate_UsesCachedProfileRoles_NotJwtClaims()
    {
        var context = CreateContext(tenantId: 1, companyId: 99);
        var profile = CreateProfile(tenantId: 1, companyId: 1, roles: [FgsRoleCodes.TenantAdmin]);

        var result = UserAuthorizationEvaluator.Evaluate(context, profile, DefaultOptions);

        result.Success.Should().BeTrue();
        result.ValidatedScope.Should().Be(new ValidatedUserScope(1, 99));
    }

    private static UserAuthorizationResult Evaluate(UserAuthProfileDto profile) =>
        UserAuthorizationEvaluator.Evaluate(CreateContext(tenantId: 1, companyId: 1), profile, DefaultOptions);

    private static UserAuthProfileDto CreateProfile(
        long tenantId = 1,
        long companyId = 1,
        bool isActive = true,
        bool isDeleted = false,
        IReadOnlyList<string>? roles = null) =>
        new(
            Guid.NewGuid(),
            "user@test.com",
            "oid-1",
            tenantId,
            companyId,
            isActive,
            isDeleted,
            roles ?? [FgsRoleCodes.TenantAdmin],
            [],
            [],
            []);

    private static DefaultHttpContext CreateContext(
        string path = "/api/v1/tenants",
        long? tenantId = null,
        long? companyId = null,
        long? routeTenantId = null,
        long? routeCompanyId = null)
    {
        var context = new DefaultHttpContext
        {
            Request =
            {
                Path = path
            }
        };

        if (tenantId.HasValue)
        {
            context.Request.Headers["X-Tenant-Id"] = tenantId.Value.ToString();
        }

        if (companyId.HasValue)
        {
            context.Request.Headers["X-Company-Id"] = companyId.Value.ToString();
        }

        if (routeTenantId.HasValue)
        {
            context.Request.RouteValues["tenantId"] = routeTenantId.Value.ToString();
        }

        if (routeCompanyId.HasValue)
        {
            context.Request.RouteValues["companyId"] = routeCompanyId.Value.ToString();
        }

        return context;
    }
}
