using Fgs.User.Application.Abstractions.RoleDataAccesses;
using Fgs.User.Application.Features.RoleDataAccesses.Commands.SyncFgsRoleDataAccesses;
using Fgs.User.Application.Features.RoleDataAccesses.Dtos;
using Fgs.User.Application.Features.RoleDataAccesses.Queries.ListFgsRoleDataAccessesByRoleId;
using Fgs.User.Application.Features.RoleDataAccesses.Validators;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class RoleDataAccessHandlerTests
{
    [Fact]
    public async Task SyncHandler_ReturnsSyncedAssignments()
    {
        var write = new Mock<IFgsRoleDataAccessWriteService>();
        write.Setup(w => w.SyncAsync(It.IsAny<FgsRoleDataAccessSyncDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([new FgsRoleDataAccessDetailDto(1, 10, 50, DateTimeOffset.UtcNow, "test")]);

        var handler = new SyncFgsRoleDataAccessesCommandHandler(
            write.Object,
            NullLogger<SyncFgsRoleDataAccessesCommandHandler>.Instance);

        var response = await handler.Handle(
            new SyncFgsRoleDataAccessesCommand(new FgsRoleDataAccessSyncDto(10, [50])),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().ContainSingle(x => x.FgsDataAccessId == 50);
    }

    [Fact]
    public async Task ListByRoleIdHandler_ReturnsAssignments()
    {
        var read = new Mock<IFgsRoleDataAccessReadRepository>();
        read.Setup(r => r.ListByRoleIdAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync([new FgsRoleDataAccessDetailDto(1, 10, 50, DateTimeOffset.UtcNow, "test")]);

        var handler = new ListFgsRoleDataAccessesByRoleIdQueryHandler(read.Object);
        var response = await handler.Handle(new ListFgsRoleDataAccessesByRoleIdQuery(10), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().HaveCount(1);
    }

    [Fact]
    public async Task SyncValidator_RejectsInvalidIds()
    {
        var validator = new SyncFgsRoleDataAccessesCommandValidator();
        var result = await validator.ValidateAsync(
            new SyncFgsRoleDataAccessesCommand(new FgsRoleDataAccessSyncDto(0, null!)));

        result.IsValid.Should().BeFalse();
    }
}
