using System.Text.Json.Serialization;

namespace Fgs.User.Application.Features.Auth.Commands.EntraApiConnector;

public sealed class EntraApiConnectorRequestDto
{
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("objectId")]
    public string? ObjectId { get; set; }
}
