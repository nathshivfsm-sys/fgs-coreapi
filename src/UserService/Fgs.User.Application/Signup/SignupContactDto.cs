namespace Fgs.User.Application.Signup;

/// <summary>
/// Q1 — Tell us about yourself.
/// </summary>
public sealed record SignupContactDto(
    string Name,
    string PhoneNumber,
    string Email);
