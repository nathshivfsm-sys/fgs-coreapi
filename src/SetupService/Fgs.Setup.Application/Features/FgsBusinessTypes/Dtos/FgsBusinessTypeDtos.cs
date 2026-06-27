namespace Fgs.Setup.Application.Features.FgsBusinessTypes.Dtos;

public sealed record FgsBusinessTypeSummaryDto(
    long Id,
    string Code,
    string Name,
    string? Description,
    short? DisplayOrder,
    bool IsActive);

public sealed record FgsBusinessTypeDetailDto(
    long Id,
    string Code,
    string Name,
    string? Description,
    short? DisplayOrder,
    bool IsActive);

public sealed record FgsBusinessTypeLookupDto(
    long Id,
    string Code,
    string Name,
    short? DisplayOrder);

public sealed record FgsBusinessTypeCreateDto(
    string Code,
    string Name,
    string? Description,
    short? DisplayOrder);

public sealed record FgsBusinessTypeUpdateDto(
    string Code,
    string Name,
    string? Description,
    short? DisplayOrder);

public sealed record FgsBusinessTypePatchDto(
    string? Code,
    string? Name,
    string? Description,
    short? DisplayOrder,
    bool? IsActive);

public sealed record FgsBusinessTypeListFilters(
    string? Code = null,
    string? Name = null);
