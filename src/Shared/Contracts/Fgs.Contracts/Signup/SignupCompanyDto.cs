namespace Fgs.Contracts.Signup;

/// <summary>Q2 — Share your company details.</summary>
public sealed record SignupCompanyDto(
    string Name,
    string? Website,
    SignupAddressDto Address,
    string CompanySize);
