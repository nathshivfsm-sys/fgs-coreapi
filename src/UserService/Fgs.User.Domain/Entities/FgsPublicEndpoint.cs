namespace Fgs.User.Domain.Entities;

/// <summary>
/// Public service endpoint available to a tenant/company for client discovery after sign-in.
/// </summary>
public class FgsPublicEndpoint : FgsTenantCompanySetupEntityBase<long>
{
    public string EndpointType { get; set; } = null!;

    public string EnvironmentCode { get; set; } = null!;

    public string BaseUrl { get; set; } = null!;

    public string? DisplayName { get; set; }
}
