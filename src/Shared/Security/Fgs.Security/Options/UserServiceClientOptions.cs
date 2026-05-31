namespace Fgs.Security.Options;

public sealed class UserServiceClientOptions
{
    public const string SectionName = "UserService";

    public string BaseUrl { get; set; } = "http://localhost:5001";
}
