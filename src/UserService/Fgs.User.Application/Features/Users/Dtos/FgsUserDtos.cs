using Fgs.User.Domain.Enums;

namespace Fgs.User.Application.Features.Users.Dtos;

public sealed record FgsUserSummaryDto(
    Guid Id,
    string DisplayName,
    string Email,
    string? PhoneNumber,
    long? RoleId,
    string? RoleName,
    string? InvitationStatus,
    bool IsActive);

public sealed record FgsUserDetailDto(
    Guid Id,
    string DisplayName,
    string Email,
    string? PhoneNumber,
    long? RoleId,
    string? RoleName,
    string? InvitationStatus,
    bool IsActive,
    bool HasAcceptedInvitation);

public sealed record FgsUserInviteDto(
    string DisplayName,
    string Email,
    string? PhoneNumber,
    IReadOnlyList<long> RoleIds,
    AuthenticationMethod AuthenticationMethod = AuthenticationMethod.PasswordOrEmailOtp);

public sealed record FgsUserUpdateDto(
    string DisplayName,
    string? PhoneNumber,
    IReadOnlyList<long> RoleIds,
    bool IsActive);

public sealed record FgsUserPatchDto(
    string? DisplayName,
    string? PhoneNumber,
    IReadOnlyList<long>? RoleIds,
    bool? IsActive);

public sealed record FgsUserListFilters(
    string? Email = null,
    string? DisplayName = null,
    long? RoleId = null);
