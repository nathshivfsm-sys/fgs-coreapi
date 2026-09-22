namespace Fgs.Contracts.Clients;

/// <summary>
/// S2S body to enable or disable a user's application login access.
/// </summary>
public sealed record SetUserAccessRequest(bool IsActive);
