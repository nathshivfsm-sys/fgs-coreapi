using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using Fgs.Security.Abstractions;
using Fgs.Security.Models;
using Microsoft.AspNetCore.Http;

namespace Fgs.Security.Services;

public sealed class RemoteFgsClaimsEnricher(
    HttpClient httpClient,
    IHttpContextAccessor httpContextAccessor) : IFgsClaimsEnricher
{
    public async Task EnrichAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default)
    {
        if (FgsClaimsEnrichment.IsEnriched(principal))
        {
            return;
        }

        var accessToken = ExtractBearerToken(httpContextAccessor.HttpContext);
        if (string.IsNullOrWhiteSpace(accessToken))
        {
            return;
        }

        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/auth/me");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        using var response = await httpClient.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return;
        }

        var profile = await response.Content.ReadFromJsonAsync<FgsAuthenticatedUserProfile>(cancellationToken);
        if (profile is null)
        {
            return;
        }

        FgsClaimsEnrichment.Apply(principal, profile);
    }

    private static string? ExtractBearerToken(HttpContext? httpContext)
    {
        var authorization = httpContext?.Request.Headers.Authorization.ToString();
        if (string.IsNullOrWhiteSpace(authorization)
            || !authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        return authorization["Bearer ".Length..].Trim();
    }
}
