namespace Fgs.Setup.Application.Common;

public static class CredentialAuditIdHelper
{
    /// <summary>
    /// Maps a global <c>GloCredential</c> integer id to a stable <see cref="Guid"/> for audit records.
    /// </summary>
    public static Guid FromGlobalCredentialId(int gloCredentialId) =>
        Guid.Parse($"00000000-0000-0000-0001-{gloCredentialId:D12}");
}
