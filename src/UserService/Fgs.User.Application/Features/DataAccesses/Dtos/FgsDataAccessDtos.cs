namespace Fgs.User.Application.Features.DataAccesses.Dtos;

public sealed record FgsDataAccessSummaryDto(
    long Id,
    string DataAccessCode,
    string Name,
    string? Description,
    bool IsBuiltIn,
    short DisplayOrder,
    bool IsActive);

public sealed record FgsDataAccessDetailDto(
    long Id,
    string DataAccessCode,
    string Name,
    string? Description,
    bool IsBuiltIn,
    short DisplayOrder,
    bool IsActive);

public sealed record FgsDataAccessLookupDto(
    long Id,
    string DataAccessCode,
    string Name,
    bool IsBuiltIn,
    short DisplayOrder);

public sealed record FgsDataAccessCreateDto(
    string DataAccessCode,
    string Name,
    string? Description,
    short DisplayOrder = 1);

public sealed record FgsDataAccessUpdateDto(
    string DataAccessCode,
    string Name,
    string? Description,
    short DisplayOrder);

public sealed record FgsDataAccessPatchDto(
    string? DataAccessCode,
    string? Name,
    string? Description,
    short? DisplayOrder,
    bool? IsActive);

public sealed record FgsDataAccessListFilters(
    string? DataAccessCode = null,
    string? Name = null,
    bool? IsBuiltIn = null);
