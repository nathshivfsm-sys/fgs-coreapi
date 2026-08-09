using Fgs.User.Application.Features.Auth;
using Fgs.User.Domain.Enums;

namespace Fgs.User.Tests.Application;

public sealed class EntraUserFlowResolverTests
{
    [Theory]
    [InlineData(AuthenticationMethod.Password)]
    [InlineData(AuthenticationMethod.PasswordWithMfa)]
    [InlineData(AuthenticationMethod.PasswordOrEmailOtp)]
    public void Resolve_PasswordMethods_UsesPasswordUserFlow(AuthenticationMethod method)
    {
        var flow = EntraUserFlowResolver.Resolve(
            method,
            userFlow: "Fgs_SignUpSignIn",
            passwordUserFlow: "Fgs_SignUpSignIn_Pwd");

        flow.Should().Be("Fgs_SignUpSignIn_Pwd");
    }

    [Theory]
    [InlineData(AuthenticationMethod.EmailOtp)]
    [InlineData(AuthenticationMethod.EntraIdOnly)]
    public void Resolve_NonPasswordMethods_UsesUserFlow(AuthenticationMethod method)
    {
        var flow = EntraUserFlowResolver.Resolve(
            method,
            userFlow: "Fgs_SignUpSignIn",
            passwordUserFlow: "Fgs_SignUpSignIn_Pwd");

        flow.Should().Be("Fgs_SignUpSignIn");
    }

    [Fact]
    public void Resolve_PasswordWithoutPasswordUserFlowConfigured_FallsBackToUserFlow()
    {
        var flow = EntraUserFlowResolver.Resolve(
            AuthenticationMethod.Password,
            userFlow: "Fgs_SignUpSignIn",
            passwordUserFlow: null);

        flow.Should().Be("Fgs_SignUpSignIn");
    }
}
