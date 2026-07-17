namespace Fgs.User.Application.Features.PublicEndpoints.Dtos;

public sealed record FgsPublicEndpointSummaryDto(
    long Id,
    string EndpointType,
    string EnvironmentCode,
    string BaseUrl,
    string? DisplayName,
    bool IsActive);

public sealed record FgsPublicEndpointDetailDto(
    long Id,
    string EndpointType,
    string EnvironmentCode,
    string BaseUrl,
    string? DisplayName,
    bool IsActive);

public sealed record FgsPublicEndpointLookupDto(
    long Id,
    string EndpointType,
    string EnvironmentCode,
    string BaseUrl,
    string? DisplayName);

public sealed record FgsPublicEndpointCreateDto(
    string EndpointType,
    string EnvironmentCode,
    string BaseUrl,
    string? DisplayName);

public sealed record FgsPublicEndpointUpdateDto(
    string EndpointType,
    string EnvironmentCode,
    string BaseUrl,
    string? DisplayName);

public sealed record FgsPublicEndpointPatchDto(
    string? EndpointType,
    string? EnvironmentCode,
    string? BaseUrl,
    string? DisplayName,
    bool? IsActive);

public sealed record FgsPublicEndpointListFilters(
    string? EndpointType = null,
    string? EnvironmentCode = null);
