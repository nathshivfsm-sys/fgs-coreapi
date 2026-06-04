using Fgs.Billing.Application;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Billing.Tests.Application;

public sealed class DependencyInjectionTests
{
    [Fact]
    public void AddFgsBillingApplication_RegistersMediatR()
    {
        var services = new ServiceCollection();
        services.AddFgsBillingApplication();
        var provider = services.BuildServiceProvider();
        Assert.NotNull(provider.GetService<IMediator>());
    }
}
