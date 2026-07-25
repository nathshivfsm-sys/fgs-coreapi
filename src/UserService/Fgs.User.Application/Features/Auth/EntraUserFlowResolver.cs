using Fgs.User.Domain.Enums;

namespace Fgs.User.Application.Features.Auth;

/// <summary>
/// Maps stored <see cref="AuthenticationMethod"/> to Entra External ID user-flow selection.
/// </summary>
public static class EntraUserFlowResolver
{
    public static bool RequiresPasswordUserFlow(AuthenticationMethod method) =>
        method is AuthenticationMethod.Password or AuthenticationMethod.PasswordWithMfa;

    public static string Resolve(
        AuthenticationMethod method,
        string? userFlow,
        string? passwordUserFlow)
    {
        if (RequiresPasswordUserFlow(method)
            && !string.IsNullOrWhiteSpace(passwordUserFlow))
        {
            return passwordUserFlow;
        }

        return string.IsNullOrWhiteSpace(userFlow) ? string.Empty : userFlow;
    }
}
