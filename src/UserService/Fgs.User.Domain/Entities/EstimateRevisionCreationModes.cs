namespace Fgs.User.Domain.Entities;

/// <summary>
/// Allowed <see cref="FgsTenantServiceSetup.EstimateRevisionCreationMode"/> values
/// (see CK_FgsTenantServiceSetup_EstimateRevisionCreationMode).
/// </summary>
public static class EstimateRevisionCreationModes
{
    /// <summary>User manually creates a revision.</summary>
    public const string OnDemand = "OnDemand";

    /// <summary>Automatically creates a revision when a signed estimate is changed.</summary>
    public const string OnPostSignatureChange = "OnPostSignatureChange";
}
