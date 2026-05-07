using MediatR;
using UserService.Application.Common.Models;

namespace UserService.Application.Signup.CreateCompanySignup;

public sealed record CreateCompanySignupCommand(
    string CompanyName,
    string AdminEmail,
    string? AdminDisplayName) : IRequest<ApiResponse<CompanySignupResponse>>;
