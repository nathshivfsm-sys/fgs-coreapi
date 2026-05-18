namespace Fgs.User.Application.Signup;

public interface ISignupUniquenessValidator
{
    Task<IReadOnlyList<string>> ValidateAsync(
        CreateCompanySignupCommand command,
        CancellationToken cancellationToken = default);
}
