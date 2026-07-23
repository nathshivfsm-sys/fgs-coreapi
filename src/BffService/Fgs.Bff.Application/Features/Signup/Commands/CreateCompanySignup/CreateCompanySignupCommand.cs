using Fgs.Contracts.Api;
using Fgs.Contracts.Signup;
using MediatR;

namespace Fgs.Bff.Application.Features.Signup.Commands.CreateCompanySignup;

/// <summary>
/// Orchestrates company signup across User (identity) and Setup (business types).
/// </summary>
public sealed record CreateCompanySignupCommand(
    SignupContactDto Contact,
    SignupCompanyDto Company,
    IReadOnlyList<int> BusinessTypeIds,
    string? TimeZone = null,
    string? DefaultCurrency = null) : IRequest<ApiResponse<CompanySignupResultDto>>;
