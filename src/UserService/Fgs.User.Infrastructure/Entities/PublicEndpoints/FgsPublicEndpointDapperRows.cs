using Fgs.User.Application.Features.PublicEndpoints.Dtos;

namespace Fgs.User.Infrastructure.Entities.PublicEndpoints;

internal sealed class FgsPublicEndpointSummaryRow
{
    public long Id { get; set; }

    public string EndpointType { get; set; } = null!;

    public string EnvironmentCode { get; set; } = null!;

    public string BaseUrl { get; set; } = null!;

    public string? DisplayName { get; set; }

    public bool IsActive { get; set; }

    public FgsPublicEndpointSummaryDto ToDto() =>
        new(Id, EndpointType, EnvironmentCode, BaseUrl, DisplayName, IsActive);
}

internal sealed class FgsPublicEndpointDetailRow
{
    public long Id { get; set; }

    public string EndpointType { get; set; } = null!;

    public string EnvironmentCode { get; set; } = null!;

    public string BaseUrl { get; set; } = null!;

    public string? DisplayName { get; set; }

    public bool IsActive { get; set; }

    public FgsPublicEndpointDetailDto ToDto() =>
        new(Id, EndpointType, EnvironmentCode, BaseUrl, DisplayName, IsActive);
}

internal sealed class FgsPublicEndpointLookupRow
{
    public long Id { get; set; }

    public string EndpointType { get; set; } = null!;

    public string EnvironmentCode { get; set; } = null!;

    public string BaseUrl { get; set; } = null!;

    public string? DisplayName { get; set; }

    public FgsPublicEndpointLookupDto ToDto() =>
        new(Id, EndpointType, EnvironmentCode, BaseUrl, DisplayName);
}
