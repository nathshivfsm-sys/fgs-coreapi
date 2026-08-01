using Fgs.User.Application.Features.Users.Dtos;

namespace Fgs.User.Infrastructure.Entities.Users;

internal sealed class FgsUserSummaryRow
{
    public Guid Id { get; set; }
    public string DisplayName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public long? RoleId { get; set; }
    public string? RoleName { get; set; }
    public string? InvitationStatus { get; set; }
    public bool IsActive { get; set; }

    public FgsUserSummaryDto ToDto() =>
        new(Id, DisplayName, Email, RoleId, RoleName, InvitationStatus, IsActive);
}

internal sealed class FgsUserDetailRow
{
    public Guid Id { get; set; }
    public string DisplayName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public long? RoleId { get; set; }
    public string? RoleName { get; set; }
    public string? InvitationStatus { get; set; }
    public bool IsActive { get; set; }
    public bool HasAcceptedInvitation { get; set; }

    public FgsUserDetailDto ToDto() =>
        new(Id, DisplayName, Email, RoleId, RoleName, InvitationStatus, IsActive, HasAcceptedInvitation);
}
