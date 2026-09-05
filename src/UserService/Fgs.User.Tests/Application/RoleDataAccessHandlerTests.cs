using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.RoleDataAccesses;
using Fgs.User.Application.Features.RoleDataAccesses.Commands.CreateFgsRoleDataAccess;
using Fgs.User.Application.Features.RoleDataAccesses.Commands.PatchFgsRoleDataAccess;
using Fgs.User.Application.Features.RoleDataAccesses.Commands.SyncFgsRoleDataAccesses;
using Fgs.User.Application.Features.RoleDataAccesses.Commands.UpdateFgsRoleDataAccess;
using Fgs.User.Application.Features.RoleDataAccesses.Dtos;
using Fgs.User.Application.Features.RoleDataAccesses.Queries.GetFgsRoleDataAccessById;
using Fgs.User.Application.Features.RoleDataAccesses.Queries.ListFgsRoleDataAccessesByRoleId;
using Fgs.User.Application.Features.RoleDataAccesses.Queries.LookupFgsRoleDataAccesses;
using Fgs.User.Application.Features.RoleDataAccesses.Validators;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class RoleDataAccessHandlerTests
{
    private static FgsRoleDataAccessDetailDto Detail(long id = 1, long roleId = 10, long dataAccessId = 50) =>
        new(id, roleId, dataAccessId, DateTimeOffset.UtcNow, "test");

    [Fact]
    public async Task SyncHandler_ReturnsSyncedAssignments()
    {
        var write = new Mock<IFgsRoleDataAccessWriteService>();
        write.Setup(w => w.SyncAsync(It.IsAny<FgsRoleDataAccessSyncDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([Detail()]);

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
            .ReturnsAsync([Detail()]);

        var handler = new ListFgsRoleDataAccessesByRoleIdQueryHandler(read.Object);
        var response = await handler.Handle(new ListFgsRoleDataAccessesByRoleIdQuery(10), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetByIdHandler_ReturnsNotFound()
    {
        var read = new Mock<IFgsRoleDataAccessReadRepository>();
        read.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync((FgsRoleDataAccessDetailDto?)null);

        var handler = new GetFgsRoleDataAccessByIdQueryHandler(read.Object);
        var response = await handler.Handle(new GetFgsRoleDataAccessByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task GetByIdHandler_ReturnsDetail()
    {
        var read = new Mock<IFgsRoleDataAccessReadRepository>();
        read.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail());

        var handler = new GetFgsRoleDataAccessByIdQueryHandler(read.Object);
        var response = await handler.Handle(new GetFgsRoleDataAccessByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.FgsDataAccessId.Should().Be(50);
    }

    [Fact]
    public async Task LookupHandler_ReturnsItems()
    {
        var read = new Mock<IFgsRoleDataAccessReadRepository>();
        read.Setup(r => r.LookupAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync([new FgsRoleDataAccessLookupDto(1, 10, 50)]);

        var handler = new LookupFgsRoleDataAccessesQueryHandler(read.Object);
        var response = await handler.Handle(new LookupFgsRoleDataAccessesQuery(10), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().ContainSingle(x => x.FgsDataAccessId == 50);
    }

    [Fact]
    public async Task CreateHandler_ReturnsCreated()
    {
        var write = new Mock<IFgsRoleDataAccessWriteService>();
        write.Setup(w => w.CreateAsync(It.IsAny<FgsRoleDataAccessCreateDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail());

        var handler = new CreateFgsRoleDataAccessCommandHandler(
            write.Object,
            NullLogger<CreateFgsRoleDataAccessCommandHandler>.Instance);

        var response = await handler.Handle(
            new CreateFgsRoleDataAccessCommand(new FgsRoleDataAccessCreateDto(10, 50)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Created);
        response.Data!.Id.Should().Be(1);
    }

    [Fact]
    public async Task UpdateHandler_ReturnsUpdated()
    {
        var write = new Mock<IFgsRoleDataAccessWriteService>();
        write.Setup(w => w.UpdateAsync(1, It.IsAny<FgsRoleDataAccessUpdateDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail(dataAccessId: 60));

        var handler = new UpdateFgsRoleDataAccessCommandHandler(
            write.Object,
            NullLogger<UpdateFgsRoleDataAccessCommandHandler>.Instance);

        var response = await handler.Handle(
            new UpdateFgsRoleDataAccessCommand(1, new FgsRoleDataAccessUpdateDto(60)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.FgsDataAccessId.Should().Be(60);
    }

    [Fact]
    public async Task PatchHandler_ReturnsPatched()
    {
        var write = new Mock<IFgsRoleDataAccessWriteService>();
        write.Setup(w => w.PatchAsync(1, It.IsAny<FgsRoleDataAccessPatchDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail(dataAccessId: 61));

        var handler = new PatchFgsRoleDataAccessCommandHandler(
            write.Object,
            NullLogger<PatchFgsRoleDataAccessCommandHandler>.Instance);

        var response = await handler.Handle(
            new PatchFgsRoleDataAccessCommand(1, new FgsRoleDataAccessPatchDto(61)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.FgsDataAccessId.Should().Be(61);
    }

    [Fact]
    public async Task SyncValidator_RejectsInvalidIds()
    {
        var validator = new SyncFgsRoleDataAccessesCommandValidator();
        var result = await validator.ValidateAsync(
            new SyncFgsRoleDataAccessesCommand(new FgsRoleDataAccessSyncDto(0, null!)));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task CreateValidator_RejectsDuplicate()
    {
        var read = new Mock<IFgsRoleDataAccessReadRepository>();
        read.Setup(r => r.ExistsByRoleIdAndDataAccessIdAsync(10, 50, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var validator = new CreateFgsRoleDataAccessCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new CreateFgsRoleDataAccessCommand(new FgsRoleDataAccessCreateDto(10, 50)));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task CreateValidator_AcceptsValidCreate()
    {
        var read = new Mock<IFgsRoleDataAccessReadRepository>();
        read.Setup(r => r.ExistsByRoleIdAndDataAccessIdAsync(10, 50, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validator = new CreateFgsRoleDataAccessCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new CreateFgsRoleDataAccessCommand(new FgsRoleDataAccessCreateDto(10, 50)));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task LookupValidator_RejectsInvalidRoleId()
    {
        var validator = new LookupFgsRoleDataAccessesQueryValidator();
        var result = await validator.ValidateAsync(new LookupFgsRoleDataAccessesQuery(0));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateValidator_RejectsDuplicate()
    {
        var read = new Mock<IFgsRoleDataAccessReadRepository>();
        read.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail());
        read.Setup(r => r.ExistsByRoleIdAndDataAccessIdAsync(10, 60, 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var validator = new UpdateFgsRoleDataAccessCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new UpdateFgsRoleDataAccessCommand(1, new FgsRoleDataAccessUpdateDto(60)));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateValidator_AcceptsWhenMissing()
    {
        var read = new Mock<IFgsRoleDataAccessReadRepository>();
        read.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((FgsRoleDataAccessDetailDto?)null);

        var validator = new UpdateFgsRoleDataAccessCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new UpdateFgsRoleDataAccessCommand(1, new FgsRoleDataAccessUpdateDto(60)));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateValidator_AcceptsUnique()
    {
        var read = new Mock<IFgsRoleDataAccessReadRepository>();
        read.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail());
        read.Setup(r => r.ExistsByRoleIdAndDataAccessIdAsync(10, 60, 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validator = new UpdateFgsRoleDataAccessCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new UpdateFgsRoleDataAccessCommand(1, new FgsRoleDataAccessUpdateDto(60)));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task PatchValidator_RejectsDuplicate()
    {
        var read = new Mock<IFgsRoleDataAccessReadRepository>();
        read.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail());
        read.Setup(r => r.ExistsByRoleIdAndDataAccessIdAsync(10, 60, 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var validator = new PatchFgsRoleDataAccessCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new PatchFgsRoleDataAccessCommand(1, new FgsRoleDataAccessPatchDto(60)));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task PatchValidator_SkipsWhenDataAccessIdOmitted()
    {
        var read = new Mock<IFgsRoleDataAccessReadRepository>();
        var validator = new PatchFgsRoleDataAccessCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new PatchFgsRoleDataAccessCommand(1, new FgsRoleDataAccessPatchDto()));

        result.IsValid.Should().BeTrue();
        read.Verify(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task PatchValidator_AcceptsWhenMissing()
    {
        var read = new Mock<IFgsRoleDataAccessReadRepository>();
        read.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((FgsRoleDataAccessDetailDto?)null);

        var validator = new PatchFgsRoleDataAccessCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new PatchFgsRoleDataAccessCommand(1, new FgsRoleDataAccessPatchDto(60)));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task PatchValidator_AcceptsUnique()
    {
        var read = new Mock<IFgsRoleDataAccessReadRepository>();
        read.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail());
        read.Setup(r => r.ExistsByRoleIdAndDataAccessIdAsync(10, 60, 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validator = new PatchFgsRoleDataAccessCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new PatchFgsRoleDataAccessCommand(1, new FgsRoleDataAccessPatchDto(60)));

        result.IsValid.Should().BeTrue();
    }
}
