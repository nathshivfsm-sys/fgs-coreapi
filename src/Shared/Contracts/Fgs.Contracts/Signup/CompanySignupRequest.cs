namespace Fgs.Contracts.Signup;

/// <summary>
/// Company self-serve signup request shared by BFF orchestration and User identity API.
/// </summary>
public sealed record CompanySignupRequest(
    SignupContactDto Contact,
    SignupCompanyDto Company,
    IReadOnlyList<int> BusinessTypeIds,
    string? TimeZone = null,
    string? DefaultCurrency = null,
    AuthenticationMethod? AuthenticationMethod = null);
