using Fgs.User.Domain.Entities;
using Fgs.User.Domain.Enums;

namespace Fgs.User.Tests.Domain;

public sealed class FgsInvitationTests
{
    [Fact]
    public void IsActive_WhenPendingAndNotExpired_ReturnsTrue()
    {
        var invitation = new FgsInvitation
        {
            Status = InvitationStatus.Pending,
            ExpiresAtUtc = DateTimeOffset.UtcNow.AddDays(1)
        };

        invitation.IsActive.Should().BeTrue();
    }

    [Fact]
    public void IsActive_WhenExpired_ReturnsFalse()
    {
        var invitation = new FgsInvitation
        {
            Status = InvitationStatus.Pending,
            ExpiresAtUtc = DateTimeOffset.UtcNow.AddMinutes(-1)
        };

        invitation.IsActive.Should().BeFalse();
    }

    [Fact]
    public void MarkAccepted_SetsStatusAndTimestamp()
    {
        var invitation = new FgsInvitation { Status = InvitationStatus.Pending };

        invitation.MarkAccepted();

        invitation.Status.Should().Be(InvitationStatus.Accepted);
        invitation.AcceptedAtUtc.Should().NotBeNull();
    }

    [Fact]
    public void MarkExpired_SetsStatusToExpired()
    {
        var invitation = new FgsInvitation { Status = InvitationStatus.Pending };

        invitation.MarkExpired();

        invitation.Status.Should().Be(InvitationStatus.Expired);
    }
}
