namespace Fgs.User.Application.Features.ApiClients.Dtos;

public sealed record FgsApiClientSummaryDto(
    long Id,
    Guid ClientId,
    string ApplicationName,
    string? Description,
    string? ContactName,
    string? ContactEmail,
    int RateLimitPerMinute,
    bool IsActive);

public sealed record FgsApiClientDetailDto(
    long Id,
    Guid ClientId,
    string ApplicationName,
    string? Description,
    string? ContactName,
    string? ContactEmail,
    int RateLimitPerMinute,
    bool IsActive);

public sealed record FgsApiClientLookupDto(
    long Id,
    Guid ClientId,
    string ApplicationName);

public sealed record FgsApiClientCreateDto(
    string ApplicationName,
    string? Description,
    string? ContactName,
    string? ContactEmail,
    int RateLimitPerMinute = 60);

public sealed record FgsApiClientUpdateDto(
    string ApplicationName,
    string? Description,
    string? ContactName,
    string? ContactEmail,
    int RateLimitPerMinute);

public sealed record FgsApiClientPatchDto(
    string? ApplicationName,
    string? Description,
    string? ContactName,
    string? ContactEmail,
    int? RateLimitPerMinute,
    bool? IsActive);

public sealed record FgsApiClientListFilters(
    string? ApplicationName = null,
    string? ContactEmail = null);
