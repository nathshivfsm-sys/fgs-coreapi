namespace Fgs.User.Application.Features.Signup;

internal static class SignupPhoneNormalizer
{
    /// <summary>
    /// Strips formatting characters and keeps digits only.
    /// </summary>
    public static string ToStorageFormat(string phoneNumber) =>
        new string(phoneNumber.Where(char.IsDigit).ToArray());
}
