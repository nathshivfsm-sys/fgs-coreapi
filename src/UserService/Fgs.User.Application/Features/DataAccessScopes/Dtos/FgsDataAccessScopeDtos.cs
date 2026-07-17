namespace Fgs.User.Application.Features.DataAccessScopes.Dtos;

public sealed record FgsDataAccessScopeSummaryDto(
    long Id,
    long FgsDataAccessId,
    string ScopeType,
    string Operator,
    string? ScopeValue,
    short DisplayOrder);

public sealed record FgsDataAccessScopeDetailDto(
    long Id,
    long FgsDataAccessId,
    string ScopeType,
    string Operator,
    string? ScopeValue,
    short DisplayOrder);

public sealed record FgsDataAccessScopeCreateDto(
    long FgsDataAccessId,
    string ScopeType,
    string Operator,
    string? ScopeValue = null,
    short DisplayOrder = 1);

public sealed record FgsDataAccessScopeUpdateDto(
    string ScopeType,
    string Operator,
    string? ScopeValue,
    short DisplayOrder);

public sealed record FgsDataAccessScopePatchDto(
    string? ScopeType,
    string? Operator,
    string? ScopeValue,
    short? DisplayOrder);

public sealed record FgsDataAccessScopeListFilters(
    long? FgsDataAccessId = null,
    string? ScopeType = null);
