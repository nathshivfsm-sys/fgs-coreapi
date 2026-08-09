using Fgs.Contracts.Api;
using Fgs.Contracts.Signup;
using MediatR;

namespace Fgs.User.Application.Features.Signup.Commands.CreateCompanySignup;

/// <summary>
/// Company self-serve signup aligned to the onboarding questionnaire (contact, company, industry).
/// Identity ownership only — <see cref="BusinessTypeIds"/> are validated for contract parity;
/// BFF applies them via Setup AddCompanyBusinessTypes after identity creation.
/// </summary>
public sealed record CreateCompanySignupCommand(
    SignupContactDto Contact,
    SignupCompanyDto Company,
    IReadOnlyList<int> BusinessTypeIds,
    string? TimeZone = null,
    string? DefaultCurrency = null,
    AuthenticationMethod? AuthenticationMethod = null) : IRequest<ApiResponse<CompanySignupResultDto>>;
