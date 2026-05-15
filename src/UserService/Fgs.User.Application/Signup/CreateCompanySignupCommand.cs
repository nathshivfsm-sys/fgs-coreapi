using Fgs.User.Application.Common;
using MediatR;

namespace Fgs.User.Application.Signup;

public sealed record CreateCompanySignupCommand(
    string TenantCode,
    string TenantName,
    string Email,
    string DisplayName,
    string? Website,
    string? Password,
    string? TimeZone,
    string? DefaultCurrency) : IRequest<ApiResponse<CompanySignupResultDto>>;
