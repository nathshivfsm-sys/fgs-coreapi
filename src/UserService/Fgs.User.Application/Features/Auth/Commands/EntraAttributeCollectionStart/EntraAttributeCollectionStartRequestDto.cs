using System.Text.Json.Serialization;

namespace Fgs.User.Application.Features.Auth.Commands.EntraAttributeCollectionStart;

/// <summary>
/// Request body sent by Microsoft Entra External ID for OnAttributeCollectionStart.
/// </summary>
public sealed class EntraAttributeCollectionStartRequestDto
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("data")]
    public EntraAttributeCollectionStartRequestDataDto? Data { get; set; }
}

public sealed class EntraAttributeCollectionStartRequestDataDto
{
    [JsonPropertyName("userSignUpInfo")]
    public EntraAttributeCollectionUserSignUpInfoDto? UserSignUpInfo { get; set; }
}

public sealed class EntraAttributeCollectionUserSignUpInfoDto
{
    [JsonPropertyName("identities")]
    public List<EntraAttributeCollectionIdentityDto>? Identities { get; set; }
}

public sealed class EntraAttributeCollectionIdentityDto
{
    [JsonPropertyName("signInType")]
    public string? SignInType { get; set; }

    [JsonPropertyName("issuerAssignedId")]
    public string? IssuerAssignedId { get; set; }
}
