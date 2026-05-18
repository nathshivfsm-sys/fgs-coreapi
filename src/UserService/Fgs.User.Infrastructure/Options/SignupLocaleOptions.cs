namespace Fgs.User.Infrastructure.Options;

public sealed class SignupLocaleOptions
{
    public const string SectionName = "Signup";

    public string DefaultTimeZone { get; set; } = "UTC";

    public string DefaultCurrency { get; set; } = "USD";
}
