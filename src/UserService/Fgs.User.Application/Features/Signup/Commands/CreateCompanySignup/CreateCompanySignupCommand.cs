using Fgs.User.Application.Common;
using Fgs.User.Application.Features.Signup.DTOs;
using MediatR;

namespace Fgs.User.Application.Features.Signup.Commands.CreateCompanySignup;

/// <summary>
/// Company self-serve signup aligned to the onboarding questionnaire (contact, company, industry).
/// </summary>
public sealed record CreateCompanySignupCommand(
    SignupContactDto Contact,
    SignupCompanyDto Company,
    IReadOnlyList<int> BusinessTypeIds,
    string? TimeZone = null,
    string? DefaultCurrency = null) : IRequest<ApiResponse<CompanySignupResultDto>>;
