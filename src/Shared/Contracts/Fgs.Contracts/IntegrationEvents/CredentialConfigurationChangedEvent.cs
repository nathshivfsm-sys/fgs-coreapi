namespace Fgs.Contracts.IntegrationEvents;

/// <summary>
/// Published when global or tenant credentials are created, updated, deleted, or rotated.
/// Downstream services (e.g. Platform) reload decrypted configuration from the credential store.
/// </summary>
public sealed record CredentialConfigurationChangedEvent(
    DateTimeOffset OccurredAtUtc,
    string Source = "Fgs.User");
