using Fgs.Security.Abstractions;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Moq;

namespace Fgs.Security.Tests.Authorization;

public sealed class AuthenticatedUserTenantScopeGuardTests
{
    [Fact]
    public void DenyCrossTenantAccess_WithUnauthenticatedUser_ReturnsNull()
    {
        var userContext = CreateUserContext(authenticated: false);

        var result = AuthenticatedUserTenantScopeGuard.DenyCrossTenantAccess<object>(
            userContext,
            requestedTenantId: 99);

        result.Should().BeNull();
    }

    [Fact]
    public void DenyCrossTenantAccess_WithMatchingTenant_ReturnsNull()
    {
        var userContext = CreateUserContext(authenticated: true, tenantId: 1);

        var result = AuthenticatedUserTenantScopeGuard.DenyCrossTenantAccess<object>(
            userContext,
            requestedTenantId: 1);

        result.Should().BeNull();
    }

    [Fact]
    public void DenyCrossTenantAccess_WithMismatchedTenant_ReturnsForbidden()
    {
        var userContext = CreateUserContext(authenticated: true, tenantId: 1);

        var result = AuthenticatedUserTenantScopeGuard.DenyCrossTenantAccess<object>(
            userContext,
            requestedTenantId: 99);

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(403);
        result.Errors.Should().Contain(UserAuthorizationMessages.TenantMismatch);
    }

    [Fact]
    public void DenyCrossTenantCompanyAccess_WithTenantAdmin_AllowsCrossCompany()
    {
        var userContext = CreateUserContext(
            authenticated: true,
            tenantId: 1,
            companyId: 1,
            roles: [FgsRoleCodes.TenantAdmin]);

        var result = AuthenticatedUserTenantScopeGuard.DenyCrossTenantCompanyAccess<object>(
            userContext,
            requestedTenantId: 1,
            requestedCompanyId: 42);

        result.Should().BeNull();
    }

    [Fact]
    public void DenyCrossTenantCompanyAccess_WithCompanyMismatch_ReturnsForbidden()
    {
        var userContext = CreateUserContext(
            authenticated: true,
            tenantId: 1,
            companyId: 1,
            roles: ["USER"]);

        var result = AuthenticatedUserTenantScopeGuard.DenyCrossTenantCompanyAccess<object>(
            userContext,
            requestedTenantId: 1,
            requestedCompanyId: 42);

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(403);
        result.Errors.Should().Contain(UserAuthorizationMessages.CompanyMismatch);
    }

    private static IFgsUserContext CreateUserContext(
        bool authenticated,
        long? tenantId = null,
        long? companyId = null,
        IReadOnlyList<string>? roles = null)
    {
        var mock = new Mock<IFgsUserContext>();
        mock.SetupGet(c => c.IsAuthenticated).Returns(authenticated);
        mock.SetupGet(c => c.TenantId).Returns(tenantId);
        mock.SetupGet(c => c.CompanyId).Returns(companyId);
        mock.SetupGet(c => c.Roles).Returns(roles ?? []);
        mock.Setup(c => c.IsInRole(It.IsAny<string>()))
            .Returns<string>(role => (roles ?? []).Contains(role, StringComparer.OrdinalIgnoreCase));
        return mock.Object;
    }
}
