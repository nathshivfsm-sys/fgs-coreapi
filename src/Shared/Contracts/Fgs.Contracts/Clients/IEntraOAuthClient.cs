using Refit;

namespace Fgs.Contracts.Clients;

public interface IEntraOAuthClient
{
    [Post("")]
    Task<EntraTokenEndpointResponse> ExchangeAuthorizationCodeAsync(
        [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, string> form,
        CancellationToken cancellationToken = default);
}

public sealed class EntraTokenEndpointResponse
{
    public string? Access_token { get; set; }
    public string? Id_token { get; set; }
    public string? Token_type { get; set; }
    public int? Expires_in { get; set; }
}
