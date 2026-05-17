using Fgs.User.Application.Common;
using MediatR;

namespace Fgs.User.Application.Signup;

/// <summary>
/// Company self-serve signup aligned to the onboarding questionnaire (contact, company, industry).
/// </summary>
public sealed record CreateCompanySignupCommand(
    SignupContactDto Contact,
    SignupCompanyDto Company,
    int BusinessTypeId,
    string? Password = null,
    string? TimeZone = null,
    string? DefaultCurrency = null) : IRequest<ApiResponse<CompanySignupResultDto>>;
