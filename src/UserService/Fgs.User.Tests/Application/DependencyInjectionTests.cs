using Fgs.User.Application;
using Fgs.User.Application.Features.Signup;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.User.Tests.Application;

public sealed class DependencyInjectionTests
{
    [Fact]
    public void AddFgsUserApplication_RegistersMediatRAndValidators()
    {
        var services = new ServiceCollection();
        services.AddFgsUserApplication();

        services.Should().Contain(sd =>
            sd.ServiceType.Name.Contains("ISignupUniquenessValidator", StringComparison.Ordinal));
        services.Should().Contain(sd =>
            sd.ImplementationType == typeof(SignupUniquenessValidator));
    }
}
