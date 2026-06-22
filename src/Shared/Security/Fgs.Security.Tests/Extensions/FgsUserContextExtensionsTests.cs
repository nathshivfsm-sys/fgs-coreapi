using Fgs.Security.Abstractions;
using Fgs.Security.Extensions;
using FluentAssertions;

namespace Fgs.Security.Tests.Extensions;

public sealed class FgsUserContextExtensionsTests
{
    [Fact]
    public void ResolveAuditActor_WhenDisplayNamePresent_UsesDisplayName()
    {
        var context = new TestUserContext(DisplayName: "Jane Doe", Email: "jane@example.com");

        context.ResolveAuditActor().Should().Be("Jane Doe");
    }

    [Fact]
    public void ResolveAuditActor_WhenDisplayNameMissing_UsesUserId()
    {
        var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var context = new TestUserContext(UserId: userId, Email: "jane@example.com");

        context.ResolveAuditActor().Should().Be(userId.ToString());
    }

    [Fact]
    public void ResolveAuditActor_WhenOnlyEmailPresent_DoesNotUseEmail()
    {
        var context = new TestUserContext(Email: "jane@example.com");

        context.ResolveAuditActor().Should().Be("System");
    }

    private sealed record TestUserContext(
        Guid? UserId = null,
        string? Email = null,
        string? DisplayName = null,
        string? EntraObjectId = null,
        long? TenantId = null,
        long? CompanyId = null,
        bool IsAuthenticated = true,
        IReadOnlyList<string>? Roles = null) : IFgsUserContext
    {
        public IReadOnlyList<string> Roles { get; } = Roles ?? [];

        public bool IsInRole(string roleCode) =>
            Roles.Contains(roleCode, StringComparer.OrdinalIgnoreCase);
    }
}
