using System.Reflection;
using Fgs.Contracts.Clients;
using Fgs.Foundation.Api;
using Fgs.Setup.API.Controllers;
using Refit;

namespace Fgs.Notification.Tests.Notifications;

public sealed class SetupClientActiveTemplateRouteTests
{
    [Fact]
    public void GetActiveTemplateAsync_PathMatchesCommunicationTemplateRoute()
    {
        var route = typeof(CommunicationTemplateController).GetCustomAttribute<FgsVersionedRouteAttribute>();
        route.Should().NotBeNull();

        var segment = route!.Template!.Trim('/').Split('/')[^1];
        segment.Should().Be("communicationtemplate");

        var method = typeof(ISetupClient).GetMethod(nameof(ISetupClient.GetActiveTemplateAsync));
        var path = method!.GetCustomAttribute<GetAttribute>()!.Path;
        path.Should().Be($"/api/v1/{segment}/active");
    }
}
