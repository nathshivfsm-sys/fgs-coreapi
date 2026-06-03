namespace Fgs.User.Domain.Enums;

/// <summary>
/// Determines whether a credential is platform-global or tenant/company scoped.
/// </summary>
public enum CredentialScope
{
    Global = 1,
    Tenant = 2
}
