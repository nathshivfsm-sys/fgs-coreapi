using Fgs.User.Application.Features.Signup.Commands.CreateCompanySignup;

namespace Fgs.User.Application.Features.Signup;

public interface ISignupUniquenessValidator
{
    Task<IReadOnlyList<string>> ValidateAsync(
        CreateCompanySignupCommand command,
        CancellationToken cancellationToken = default);
}
