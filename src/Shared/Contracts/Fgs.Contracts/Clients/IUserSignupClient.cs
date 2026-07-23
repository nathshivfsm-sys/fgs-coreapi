using Fgs.Contracts.Signup;
using Refit;

namespace Fgs.Contracts.Clients;

/// <summary>
/// Refit client for UserService company signup (identity + invitation). Business-type seeding is owned by BFF.
/// </summary>
public interface IUserSignupClient
{
    [Post("/api/v1/signup/company")]
    Task<Fgs.Contracts.Api.ApiResponse<CompanySignupResultDto>> CreateCompanySignupAsync(
        [Body] CompanySignupRequest request,
        CancellationToken cancellationToken = default);
}
