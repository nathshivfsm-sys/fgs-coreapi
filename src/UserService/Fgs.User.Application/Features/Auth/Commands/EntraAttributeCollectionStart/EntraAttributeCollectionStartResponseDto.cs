using System.Text.Json.Serialization;

namespace Fgs.User.Application.Features.Auth.Commands.EntraAttributeCollectionStart;

/// <summary>
/// Response body for Entra OnAttributeCollectionStart (prefill Display Name from signup contact).
/// </summary>
public sealed class EntraAttributeCollectionStartResponseDto
{
    [JsonPropertyName("data")]
    public required EntraAttributeCollectionStartResponseDataDto Data { get; init; }

    public static EntraAttributeCollectionStartResponseDto Continue() =>
        new()
        {
            Data = new EntraAttributeCollectionStartResponseDataDto
            {
                ODataType = "microsoft.graph.onAttributeCollectionStartResponseData",
                Actions =
                [
                    new EntraAttributeCollectionStartActionDto
                    {
                        ODataType = "microsoft.graph.attributeCollectionStart.continueWithDefaultBehavior"
                    }
                ]
            }
        };

    public static EntraAttributeCollectionStartResponseDto PrefillDisplayName(string displayName) =>
        new()
        {
            Data = new EntraAttributeCollectionStartResponseDataDto
            {
                ODataType = "microsoft.graph.onAttributeCollectionStartResponseData",
                Actions =
                [
                    new EntraAttributeCollectionStartActionDto
                    {
                        ODataType = "microsoft.graph.attributeCollectionStart.setPrefillValues",
                        Inputs = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                        {
                            ["displayName"] = displayName
                        }
                    }
                ]
            }
        };
}

public sealed class EntraAttributeCollectionStartResponseDataDto
{
    [JsonPropertyName("@odata.type")]
    public required string ODataType { get; init; }

    [JsonPropertyName("actions")]
    public required IReadOnlyList<EntraAttributeCollectionStartActionDto> Actions { get; init; }
}

public sealed class EntraAttributeCollectionStartActionDto
{
    [JsonPropertyName("@odata.type")]
    public required string ODataType { get; init; }

    [JsonPropertyName("inputs")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, string>? Inputs { get; init; }
}
