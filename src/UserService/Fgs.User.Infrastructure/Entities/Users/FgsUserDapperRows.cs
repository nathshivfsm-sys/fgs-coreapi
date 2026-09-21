using Fgs.User.Application.Features.Users.Dtos;

namespace Fgs.User.Infrastructure.Entities.Users;

internal sealed class FgsUserSummaryRow
{
    public Guid Id { get; set; }
    public string DisplayName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public long? RoleId { get; set; }
    public string? RoleName { get; set; }
    public string? InvitationStatus { get; set; }
    public bool IsActive { get; set; }

    public FgsUserSummaryDto ToDto() =>
        new(Id, DisplayName, Email, PhoneNumber, RoleId, RoleName, InvitationStatus, IsActive);
}

internal sealed class FgsUserDetailRow
{
    public Guid Id { get; set; }
    public string DisplayName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public long? RoleId { get; set; }
    public string? RoleName { get; set; }
    public string? InvitationStatus { get; set; }
    public bool IsActive { get; set; }
    public bool HasAcceptedInvitation { get; set; }

    public FgsUserDetailDto ToDto() =>
        new(Id, DisplayName, Email, PhoneNumber, RoleId, RoleName, InvitationStatus, IsActive, HasAcceptedInvitation);
}

internal sealed class FgsUserListSummaryRow
{
    public int TotalUsers { get; set; }
    public int PendingInvitation { get; set; }
    public int ActiveRegistered { get; set; }
    public int Inactive { get; set; }
    public int Admins { get; set; }

    public FgsUserListSummaryDto ToDto() =>
        new(TotalUsers, PendingInvitation, ActiveRegistered, Inactive, Admins);
}
