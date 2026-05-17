using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.User.API.Controllers;

/// <summary>
/// Temporary local dashboard page after Entra sign-in (dev gateway flow).
/// </summary>
[ApiController]
[Route("api/dashboard")]
public sealed class DashboardController : ControllerBase
{
    private const string PageStyles = """
        :root {
          font-family: system-ui, -apple-system, Segoe UI, Roboto, sans-serif;
          color: #0f172a;
          background: #f1f5f9;
        }
        body {
          margin: 0;
          min-height: 100vh;
          display: grid;
          place-items: center;
          padding: 1.5rem;
        }
        main {
          max-width: 36rem;
          width: 100%;
          background: #fff;
          border-radius: 12px;
          box-shadow: 0 10px 40px rgba(15, 23, 42, 0.08);
          padding: 2rem;
        }
        h1 { margin: 0 0 0.5rem; font-size: 1.5rem; }
        h2 {
          margin: 1.5rem 0 0.75rem;
          font-size: 1rem;
          font-weight: 600;
          color: #334155;
        }
        p { margin: 0 0 1rem; color: #475569; line-height: 1.5; }
        .badge {
          display: inline-block;
          padding: 0.25rem 0.75rem;
          border-radius: 999px;
          font-size: 0.875rem;
          font-weight: 600;
          background: #dcfce7;
          color: #166534;
        }
        .badge.warn {
          background: #fef3c7;
          color: #92400e;
        }
        dl {
          margin: 0;
          display: grid;
          grid-template-columns: 9rem 1fr;
          gap: 0.5rem 1rem;
          font-size: 0.9375rem;
        }
        dt {
          margin: 0;
          color: #64748b;
          font-weight: 500;
        }
        dd {
          margin: 0;
          color: #0f172a;
          word-break: break-word;
        }
        .alert {
          padding: 0.75rem 1rem;
          border-radius: 8px;
          background: #fef2f2;
          color: #991b1b;
          font-size: 0.875rem;
        }
        .token-details {
          margin-top: 1.5rem;
          font-size: 0.875rem;
          color: #64748b;
        }
        .token-details pre {
          margin: 0.5rem 0 0;
          padding: 1rem;
          border-radius: 8px;
          background: #0f172a;
          color: #e2e8f0;
          font-size: 0.7rem;
          overflow-x: auto;
          word-break: break-all;
          white-space: pre-wrap;
        }
        .muted {
          font-size: 0.8125rem;
          color: #94a3b8;
          margin-top: 1.5rem;
        }
        """;

    [HttpGet]
    [Produces("text/html")]
    public ContentResult Get([FromQuery] string? token)
    {
        var html = BuildPageHtml(token);
        return Content(html, "text/html; charset=utf-8");
    }

    private static string BuildPageHtml(string? token)
    {
        var parseResult = DashboardJwtReader.TryRead(token);
        var userSection = parseResult.Success
            ? BuildUserDetailsSection(parseResult.Claims!)
            : BuildTokenErrorSection(parseResult.ErrorMessage ?? "Invalid token.");

        var rawTokenBlock = parseResult.Success && !string.IsNullOrWhiteSpace(token)
            ? $"""
               <details class="token-details">
                 <summary>Raw access token (dev only)</summary>
                 <pre>{Encode(token)}</pre>
               </details>
               """
            : string.Empty;

        return $"""
            <!DOCTYPE html>
            <html lang="en">
            <head>
              <meta charset="utf-8" />
              <meta name="viewport" content="width=device-width, initial-scale=1" />
              <title>FGS — Dashboard</title>
              <style>{PageStyles}</style>
            </head>
            <body>
              <main>
                <span class="badge">Temporary dev page</span>
                <h1>Welcome</h1>
                <p>Signed in via Microsoft Entra. User details below are read from the JWT query parameter.</p>
                {userSection}
                {rawTokenBlock}
                <p class="muted">FGS User Service · <code>GET /api/dashboard?token=…</code></p>
              </main>
            </body>
            </html>
            """;
    }

    private static string BuildUserDetailsSection(DashboardUserClaims claims)
    {
        var rows = new StringBuilder();
        AppendRow(rows, "User ID", claims.UserId);
        AppendRow(rows, "Email", claims.Email);
        AppendRow(rows, "Role", claims.Role);
        AppendRow(rows, "Tenant ID", claims.TenantId);
        AppendRow(rows, "Company ID", claims.CompanyId);
        if (!string.IsNullOrWhiteSpace(claims.EntraObjectId))
        {
            AppendRow(rows, "Entra object ID", claims.EntraObjectId);
        }

        AppendRow(rows, "Issuer", claims.Issuer);
        AppendRow(rows, "Audience", claims.Audience);
        AppendRow(rows, "Expires (UTC)", claims.ExpiresUtc);

        return $"""
            <h2>User details</h2>
            <dl>{rows}</dl>
            """;
    }

    private static void AppendRow(StringBuilder rows, string label, string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        rows.Append("<dt>").Append(Encode(label)).Append("</dt><dd>")
            .Append(Encode(value))
            .Append("</dd>");
    }

    private static string BuildTokenErrorSection(string message) =>
        $"""
         <span class="badge warn">No user data</span>
         <div class="alert">{Encode(message)}</div>
         <p>Complete sign-in via <code>/api/auth/entra/callback</code> to receive a <code>token</code> query parameter.</p>
         """;

    private static string Encode(string? value) =>
        string.IsNullOrEmpty(value) ? string.Empty : System.Net.WebUtility.HtmlEncode(value);

    private sealed record DashboardUserClaims(
        string? UserId,
        string? Email,
        string? Role,
        string? TenantId,
        string? CompanyId,
        string? EntraObjectId,
        string? Issuer,
        string? Audience,
        string? ExpiresUtc);

    private static class DashboardJwtReader
    {
        public static (bool Success, DashboardUserClaims? Claims, string? ErrorMessage) TryRead(string? token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return (false, null, "No token query parameter was provided.");
            }

            try
            {
                var handler = new JwtSecurityTokenHandler();
                if (!handler.CanReadToken(token))
                {
                    return (false, null, "The token is not a valid JWT.");
                }

                var jwt = handler.ReadJwtToken(token);
                var claims = jwt.Claims.ToList();

                string? Get(params string[] types) =>
                    types.Select(t => claims.FirstOrDefault(c => c.Type == t)?.Value)
                        .FirstOrDefault(v => !string.IsNullOrWhiteSpace(v));

                var exp = Get(JwtRegisteredClaimNames.Exp);
                var expiresUtc = exp is not null && long.TryParse(exp, out var unix)
                    ? DateTimeOffset.FromUnixTimeSeconds(unix).UtcDateTime.ToString("u")
                    : jwt.ValidTo.ToUniversalTime().ToString("u");

                var userClaims = new DashboardUserClaims(
                    UserId: Get(JwtRegisteredClaimNames.Sub, "sub"),
                    Email: Get(JwtRegisteredClaimNames.Email, "email"),
                    Role: Get(ClaimTypes.Role, "role", "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"),
                    TenantId: Get("tenant_id"),
                    CompanyId: Get("company_id"),
                    EntraObjectId: Get("entra_oid"),
                    Issuer: jwt.Issuer,
                    Audience: jwt.Audiences.FirstOrDefault(),
                    ExpiresUtc: expiresUtc);

                if (string.IsNullOrWhiteSpace(userClaims.UserId) && string.IsNullOrWhiteSpace(userClaims.Email))
                {
                    return (false, null, "The token does not contain recognizable user claims.");
                }

                return (true, userClaims, null);
            }
            catch (Exception ex)
            {
                return (false, null, $"Could not read token: {ex.Message}");
            }
        }
    }
}
