using Fgs.User.Domain.Enums;

namespace Fgs.User.Application.Signup;

/// <summary>
/// Q2 — Share your company details.
/// </summary>
public sealed record SignupCompanyDto(
    string Name,
    string? Website,
    SignupAddressDto Address,
    CompanySize CompanySize);
