namespace Fgs.Security.UserAuth;

public sealed class UserAuthCacheOptions
{
    public const string SectionName = "UserAuthCache";

    public int AbsoluteExpirationMinutes { get; set; } = 30;
}
