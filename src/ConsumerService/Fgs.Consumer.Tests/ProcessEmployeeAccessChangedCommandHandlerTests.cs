using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Contracts.IntegrationEvents;
using Fgs.Consumer.Application.Features.Employees.Commands.ProcessEmployeeAccessChanged;
using Fgs.Messaging.Consumer;
using Moq;

namespace Fgs.Consumer.Tests;

public sealed class ProcessEmployeeAccessChangedCommandHandlerTests
{
    [Fact]
    public async Task Handle_CallsUserClient()
    {
        var client = new Mock<IUserInternalUsersClient>();
        client.Setup(c => c.SetUserAccessAsync(
                It.IsAny<Guid>(),
                It.IsAny<SetUserAccessRequest>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApiResponse<object>.Ok(new object()));

        var handler = new ProcessEmployeeAccessChangedCommandHandler(client.Object);
        var userId = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
        var evt = new EmployeeAccessChangedEvent(10, 1, 42, userId, IsActive: false);

        await handler.Handle(
            new ProcessEmployeeAccessChangedCommand(evt, CreateContext()),
            CancellationToken.None);

        client.Verify(
            c => c.SetUserAccessAsync(
                userId,
                It.Is<SetUserAccessRequest>(r => r.IsActive == false),
                "10",
                "1",
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenClientFails_Throws()
    {
        var client = new Mock<IUserInternalUsersClient>();
        client.Setup(c => c.SetUserAccessAsync(
                It.IsAny<Guid>(),
                It.IsAny<SetUserAccessRequest>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApiResponse<object>.Fail(["failed"], ApiStatusCodes.InternalServerError));

        var handler = new ProcessEmployeeAccessChangedCommandHandler(client.Object);
        var act = () => handler.Handle(
            new ProcessEmployeeAccessChangedCommand(
                new EmployeeAccessChangedEvent(10, 1, 1, Guid.NewGuid(), false),
                CreateContext()),
            CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    private static ConsumerMessageContext CreateContext() => new()
    {
        RoutingKey = IntegrationEventRoutingKeys.EmployeeAccessChanged,
        MessageId = Guid.NewGuid().ToString("N"),
        CorrelationId = Guid.NewGuid().ToString(),
        RawBody = "{}"
    };
}
