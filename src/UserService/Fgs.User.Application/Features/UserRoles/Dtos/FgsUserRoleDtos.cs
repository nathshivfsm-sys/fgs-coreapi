namespace Fgs.User.Application.Features.UserRoles.Dtos;

public sealed record FgsUserRoleSummaryDto(
    long Id,
    Guid UserId,
    long FgsRoleId,
    DateTimeOffset CreatedOn,
    string CreatedBy);

public sealed record FgsUserRoleDetailDto(
    long Id,
    Guid UserId,
    long FgsRoleId,
    DateTimeOffset CreatedOn,
    string CreatedBy);

public sealed record FgsUserRoleCreateDto(Guid UserId, long FgsRoleId);

public sealed record FgsUserRoleListFilters(Guid? UserId = null, long? FgsRoleId = null);
