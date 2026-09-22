using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.Users;
using Fgs.User.Application.Features.Users.Commands.SetFgsUserAccess;
using Fgs.User.Application.Features.Users.Dtos;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class SetFgsUserAccessCommandHandlerTests
{
    private static readonly Guid UserId = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");

    [Fact]
    public async Task Handle_CallsWriteService()
    {
        var write = new Mock<IFgsUserWriteService>();
        write.Setup(w => w.SetAccessAsync(UserId, false, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new SetFgsUserAccessCommandHandler(
            write.Object,
            NullLogger<SetFgsUserAccessCommandHandler>.Instance);

        var response = await handler.Handle(
            new SetFgsUserAccessCommand(UserId, IsActive: false),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        write.Verify(w => w.SetAccessAsync(UserId, false, It.IsAny<CancellationToken>()), Times.Once);
    }
}
