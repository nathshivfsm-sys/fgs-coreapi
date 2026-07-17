namespace Fgs.User.Application.Features.RoleDataAccesses.Dtos;

public sealed record FgsRoleDataAccessSummaryDto(
    long Id,
    long FgsRoleId,
    long FgsDataAccessId,
    DateTimeOffset CreatedOn,
    string CreatedBy);

public sealed record FgsRoleDataAccessDetailDto(
    long Id,
    long FgsRoleId,
    long FgsDataAccessId,
    DateTimeOffset CreatedOn,
    string CreatedBy);

public sealed record FgsRoleDataAccessCreateDto(long FgsRoleId, long FgsDataAccessId);

public sealed record FgsRoleDataAccessListFilters(long? FgsRoleId = null, long? FgsDataAccessId = null);
