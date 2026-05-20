namespace Fgs.User.Infrastructure.Common.Options;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = "fgs-user-service";

    public string Audience { get; set; } = "fgs-platform";

    public string SigningKey { get; set; } = "CHANGE_ME_TO_A_LONG_RANDOM_SECRET_KEY_32+";

    public int ExpiryMinutes { get; set; } = 60;
}
