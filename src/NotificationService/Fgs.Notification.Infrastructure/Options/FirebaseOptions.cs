namespace Fgs.Notification.Infrastructure.Options;

public sealed class FirebaseOptions
{
    public const string SectionName = "Firebase";

    /// <summary>
    /// Absolute path to a Firebase service-account JSON file. When empty, push sends soft-fail.
    /// </summary>
    public string CredentialPath { get; set; } = string.Empty;

    /// <summary>
    /// Inline service-account JSON. Used when <see cref="CredentialPath"/> is empty.
    /// </summary>
    public string CredentialJson { get; set; } = string.Empty;
}
