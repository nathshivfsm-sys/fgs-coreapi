namespace Fgs.User.Domain.Entities;

/// <summary>
/// Master list of supported credential providers and integrations available within the FSM platform.
/// </summary>
public class GloCredentialProviderType
{
    public int Id { get; set; }

    /// <summary>System unique code used by application logic and integration services.</summary>
    public string ProviderCode { get; set; } = null!;

    /// <summary>User friendly provider name displayed in setup screens.</summary>
    public string ProviderName { get; set; } = null!;

    /// <summary>JSON schema used by the UI to dynamically render provider configuration fields.</summary>
    public string ConfigurationSchema { get; set; } = null!;

    /// <summary>Indicates whether the provider can be selected for new credential configurations.</summary>
    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? UpdatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public ICollection<GloCredential> Credentials { get; set; } = [];

    public ICollection<FgsCredential> TenantCredentials { get; set; } = [];
}
