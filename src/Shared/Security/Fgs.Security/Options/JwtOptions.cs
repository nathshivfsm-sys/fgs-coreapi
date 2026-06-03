namespace Fgs.Security.Options;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = "fgs-user-service";

    public string Audience { get; set; } = "fgs-platform";

    public string SigningKey { get; set; } = string.Empty;

    public int ExpiryMinutes { get; set; } = 60;
}
