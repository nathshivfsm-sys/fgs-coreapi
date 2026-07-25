using Fgs.Contracts.Api;
using Fgs.Contracts.Signup;
using MediatR;

namespace Fgs.User.Application.Features.Signup.Commands.CreateCompanySignup;

/// <summary>
/// Company self-serve signup aligned to the onboarding questionnaire (contact, company, industry).
/// Identity ownership only — business-type seeding is orchestrated by the BFF.
/// </summary>
public sealed record CreateCompanySignupCommand(
    SignupContactDto Contact,
    SignupCompanyDto Company,
    IReadOnlyList<int> BusinessTypeIds,
    string? TimeZone = null,
    string? DefaultCurrency = null,
    AuthenticationMethod? AuthenticationMethod = null) : IRequest<ApiResponse<CompanySignupResultDto>>;
