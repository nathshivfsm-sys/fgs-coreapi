namespace Fgs.Contracts.Health;

public sealed record ServiceHealthDto(string Service, string Status, string ApiVersion);
