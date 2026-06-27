namespace Fgs.User.Application.Features.Auth.Commands.EntraApiConnector;

public sealed record EntraApiConnectorResponseDto(
    string Version,
    string Action,
    string? TenantId = null,
    string? CompanyId = null,
    string? UserMessage = null);
