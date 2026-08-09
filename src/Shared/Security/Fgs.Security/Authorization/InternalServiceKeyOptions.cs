namespace Fgs.Security.Authorization;

public sealed class InternalServiceKeyOptions
{
    public const string SectionName = "CredentialDistribution";

    public string InternalServiceKey { get; set; } = string.Empty;

    public string[] AdditionalInternalServiceKeys { get; set; } = [];
}
