using Fgs.Foundation.Paging;
using Fgs.User.Application.Abstractions.DataAccesses;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.DataAccesses.Commands.CreateFgsDataAccess;
using Fgs.User.Application.Features.DataAccesses.Commands.PatchFgsDataAccess;
using Fgs.User.Application.Features.DataAccesses.Commands.UpdateFgsDataAccess;
using Fgs.User.Application.Features.DataAccesses.Dtos;
using Fgs.User.Application.Features.DataAccesses.Queries.GetFgsDataAccessById;
using Fgs.User.Application.Features.DataAccesses.Queries.ListFgsDataAccesses;
using Fgs.User.Application.Features.DataAccesses.Queries.LookupFgsDataAccesses;
using Fgs.User.Application.Features.DataAccesses.Validators;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class DataAccessHandlerTests
{
    private static readonly FgsDataAccessDetailDto Detail =
        new(1, "ALL_LOCATIONS", "All Locations", "Desc", false, 1, true);

    [Fact]
    public async Task CreateHandler_ReturnsCreatedDataAccess()
    {
        var write = new Mock<IFgsDataAccessWriteService>();
        write.Setup(w => w.CreateAsync(It.IsAny<FgsDataAccessCreateDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail);

        var handler = new CreateFgsDataAccessCommandHandler(write.Object, NullLogger<CreateFgsDataAccessCommandHandler>.Instance);
        var response = await handler.Handle(
            new CreateFgsDataAccessCommand(new FgsDataAccessCreateDto("ALL_LOCATIONS", "All Locations", "Desc")),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data!.DataAccessCode.Should().Be("ALL_LOCATIONS");
    }

    [Fact]
    public async Task UpdateHandler_ReturnsUpdatedDataAccess()
    {
        var write = new Mock<IFgsDataAccessWriteService>();
        write.Setup(w => w.UpdateAsync(1, It.IsAny<FgsDataAccessUpdateDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail);

        var handler = new UpdateFgsDataAccessCommandHandler(write.Object, NullLogger<UpdateFgsDataAccessCommandHandler>.Instance);
        var response = await handler.Handle(
            new UpdateFgsDataAccessCommand(1, new FgsDataAccessUpdateDto("ALL_LOCATIONS", "All Locations", "Desc", 1)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Id.Should().Be(1);
    }

    [Fact]
    public async Task PatchHandler_ReturnsPatchedDataAccess()
    {
        var write = new Mock<IFgsDataAccessWriteService>();
        write.Setup(w => w.PatchAsync(1, It.IsAny<FgsDataAccessPatchDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail with { IsActive = false });

        var handler = new PatchFgsDataAccessCommandHandler(write.Object, NullLogger<PatchFgsDataAccessCommandHandler>.Instance);
        var response = await handler.Handle(
            new PatchFgsDataAccessCommand(1, new FgsDataAccessPatchDto(null, null, null, null, false)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task GetByIdHandler_WhenFound_ReturnsDataAccess()
    {
        var read = new Mock<IFgsDataAccessReadRepository>();
        read.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(Detail);

        var handler = new GetFgsDataAccessByIdQueryHandler(read.Object);
        var response = await handler.Handle(new GetFgsDataAccessByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.DataAccessCode.Should().Be("ALL_LOCATIONS");
    }

    [Fact]
    public async Task GetByIdHandler_WhenMissing_ReturnsNotFound()
    {
        var read = new Mock<IFgsDataAccessReadRepository>();
        read.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((FgsDataAccessDetailDto?)null);

        var handler = new GetFgsDataAccessByIdQueryHandler(read.Object);
        var response = await handler.Handle(new GetFgsDataAccessByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task ListHandler_ReturnsPagedResults()
    {
        var summary = new FgsDataAccessSummaryDto(1, "ALL_LOCATIONS", "All Locations", "Desc", false, 1, true);
        var paged = new PagedResult<FgsDataAccessSummaryDto>([summary], 1, 25, 1);
        var read = new Mock<IFgsDataAccessReadRepository>();
        read.Setup(r => r.ListAsync(It.IsAny<IdentityListQuery>(), It.IsAny<FgsDataAccessListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(paged);

        var handler = new ListFgsDataAccessesQueryHandler(read.Object);
        var response = await handler.Handle(
            new ListFgsDataAccessesQuery(new IdentityListQuery(), new FgsDataAccessListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Items.Should().ContainSingle();
    }

    [Fact]
    public async Task LookupHandler_ReturnsLookups()
    {
        var read = new Mock<IFgsDataAccessReadRepository>();
        read.Setup(r => r.LookupAsync(true, It.IsAny<CancellationToken>()))
            .ReturnsAsync([new FgsDataAccessLookupDto(1, "ALL_LOCATIONS", "All Locations", false, 1)]);

        var handler = new LookupFgsDataAccessesQueryHandler(read.Object);
        var response = await handler.Handle(new LookupFgsDataAccessesQuery(), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().ContainSingle();
    }

    [Fact]
    public async Task CreateValidator_RejectsLowercaseCode()
    {
        var read = new Mock<IFgsDataAccessReadRepository>();
        read.Setup(r => r.ExistsByDataAccessCodeAsync(It.IsAny<string>(), It.IsAny<long?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validator = new CreateFgsDataAccessCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new CreateFgsDataAccessCommand(new FgsDataAccessCreateDto("all_locations", "All Locations", null)));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task CreateValidator_AcceptsValidPayload()
    {
        var read = new Mock<IFgsDataAccessReadRepository>();
        read.Setup(r => r.ExistsByDataAccessCodeAsync(It.IsAny<string>(), It.IsAny<long?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validator = new CreateFgsDataAccessCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new CreateFgsDataAccessCommand(new FgsDataAccessCreateDto("ALL_LOCATIONS", "All Locations", null)));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateValidator_RejectsInvalidId()
    {
        var read = new Mock<IFgsDataAccessReadRepository>();
        var validator = new UpdateFgsDataAccessCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new UpdateFgsDataAccessCommand(0, new FgsDataAccessUpdateDto("ALL_LOCATIONS", "All Locations", null, 1)));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task PatchValidator_RejectsInvalidId()
    {
        var read = new Mock<IFgsDataAccessReadRepository>();
        var validator = new PatchFgsDataAccessCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new PatchFgsDataAccessCommand(0, new FgsDataAccessPatchDto("ALL_LOCATIONS", null, null, null, null)));

        result.IsValid.Should().BeFalse();
    }
}
