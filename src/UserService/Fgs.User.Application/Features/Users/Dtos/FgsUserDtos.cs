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
    IReadOnlyList<long>? RoleIds = null);

/// <summary>
/// Company-scoped aggregate counts for Users UI cards/tab badges (not narrowed by list filters).
/// </summary>
public sealed record FgsUserListSummaryDto(
    int TotalUsers,
    int PendingInvitation,
    int ActiveRegistered,
    int Inactive,
    int Admins);

/// <summary>
/// List page plus company summary. <see cref="TotalCount"/> respects current list filters;
/// <see cref="Summary"/> is global for the current tenant/company (zeros when includeSummary is false).
/// </summary>
public sealed record FgsUserListResultDto(
    IReadOnlyList<FgsUserSummaryDto> Items,
    int Page,
    int PageSize,
    int TotalCount,
    FgsUserListSummaryDto Summary);
