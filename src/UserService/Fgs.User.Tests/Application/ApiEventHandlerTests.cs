using Fgs.Foundation.Paging;
using Fgs.User.Application.Abstractions.ApiEvents;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.ApiEvents.Commands.CreateFgsApiEvent;
using Fgs.User.Application.Features.ApiEvents.Commands.PatchFgsApiEvent;
using Fgs.User.Application.Features.ApiEvents.Commands.UpdateFgsApiEvent;
using Fgs.User.Application.Features.ApiEvents.Dtos;
using Fgs.User.Application.Features.ApiEvents.Queries.GetFgsApiEventById;
using Fgs.User.Application.Features.ApiEvents.Queries.ListFgsApiEvents;
using Fgs.User.Application.Features.ApiEvents.Queries.LookupFgsApiEvents;
using Fgs.User.Application.Features.ApiEvents.Validators;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class ApiEventHandlerTests
{
    private static readonly FgsApiEventDetailDto Detail =
        new(1, "WORKORDER.CREATED", "WORKORDER", "Work Order Created", "Desc", 1, 1, true);

    [Fact]
    public async Task CreateHandler_ReturnsCreatedEvent()
    {
        var write = new Mock<IFgsApiEventWriteService>();
        write.Setup(w => w.CreateAsync(It.IsAny<FgsApiEventCreateDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail);

        var handler = new CreateFgsApiEventCommandHandler(write.Object, NullLogger<CreateFgsApiEventCommandHandler>.Instance);
        var response = await handler.Handle(
            new CreateFgsApiEventCommand(new FgsApiEventCreateDto("WORKORDER.CREATED", "WORKORDER", "Work Order Created", "Desc")),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data!.EventCode.Should().Be("WORKORDER.CREATED");
    }

    [Fact]
    public async Task UpdateHandler_ReturnsUpdatedEvent()
    {
        var write = new Mock<IFgsApiEventWriteService>();
        write.Setup(w => w.UpdateAsync(1, It.IsAny<FgsApiEventUpdateDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail);

        var handler = new UpdateFgsApiEventCommandHandler(write.Object, NullLogger<UpdateFgsApiEventCommandHandler>.Instance);
        var response = await handler.Handle(
            new UpdateFgsApiEventCommand(1, new FgsApiEventUpdateDto("WORKORDER.CREATED", "WORKORDER", "Work Order Created", "Desc", 1, 1)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Id.Should().Be(1);
    }

    [Fact]
    public async Task PatchHandler_ReturnsPatchedEvent()
    {
        var write = new Mock<IFgsApiEventWriteService>();
        write.Setup(w => w.PatchAsync(1, It.IsAny<FgsApiEventPatchDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail with { IsActive = false });

        var handler = new PatchFgsApiEventCommandHandler(write.Object, NullLogger<PatchFgsApiEventCommandHandler>.Instance);
        var response = await handler.Handle(
            new PatchFgsApiEventCommand(1, new FgsApiEventPatchDto(null, null, null, null, null, null, false)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task GetByIdHandler_WhenFound_ReturnsEvent()
    {
        var read = new Mock<IFgsApiEventReadRepository>();
        read.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(Detail);

        var handler = new GetFgsApiEventByIdQueryHandler(read.Object);
        var response = await handler.Handle(new GetFgsApiEventByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.EventCode.Should().Be("WORKORDER.CREATED");
    }

    [Fact]
    public async Task GetByIdHandler_WhenMissing_ReturnsNotFound()
    {
        var read = new Mock<IFgsApiEventReadRepository>();
        read.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((FgsApiEventDetailDto?)null);

        var handler = new GetFgsApiEventByIdQueryHandler(read.Object);
        var response = await handler.Handle(new GetFgsApiEventByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task ListHandler_ReturnsPagedResults()
    {
        var summary = new FgsApiEventSummaryDto(1, "WORKORDER.CREATED", "WORKORDER", "Work Order Created", "Desc", 1, 1, true);
        var paged = new PagedResult<FgsApiEventSummaryDto>([summary], 1, 25, 1);
        var read = new Mock<IFgsApiEventReadRepository>();
        read.Setup(r => r.ListAsync(It.IsAny<IdentityListQuery>(), It.IsAny<FgsApiEventListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(paged);

        var handler = new ListFgsApiEventsQueryHandler(read.Object);
        var response = await handler.Handle(
            new ListFgsApiEventsQuery(new IdentityListQuery(), new FgsApiEventListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Items.Should().ContainSingle();
    }

    [Fact]
    public async Task LookupHandler_ReturnsLookups()
    {
        var read = new Mock<IFgsApiEventReadRepository>();
        read.Setup(r => r.LookupAsync(true, It.IsAny<CancellationToken>()))
            .ReturnsAsync([new FgsApiEventLookupDto(1, "WORKORDER.CREATED", "WORKORDER", "Work Order Created", 1, 1)]);

        var handler = new LookupFgsApiEventsQueryHandler(read.Object);
        var response = await handler.Handle(new LookupFgsApiEventsQuery(), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().ContainSingle();
    }

    [Fact]
    public async Task CreateValidator_RejectsDuplicateCode()
    {
        var read = new Mock<IFgsApiEventReadRepository>();
        read.Setup(r => r.ExistsByEventCodeAsync("WORKORDER.CREATED", null, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var validator = new CreateFgsApiEventCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new CreateFgsApiEventCommand(new FgsApiEventCreateDto("WORKORDER.CREATED", "WORKORDER", "Work Order Created")));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task CreateValidator_AcceptsValidPayload()
    {
        var read = new Mock<IFgsApiEventReadRepository>();
        read.Setup(r => r.ExistsByEventCodeAsync(It.IsAny<string>(), It.IsAny<long?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validator = new CreateFgsApiEventCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new CreateFgsApiEventCommand(new FgsApiEventCreateDto("WORKORDER.CREATED", "WORKORDER", "Work Order Created")));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateValidator_RejectsInvalidId()
    {
        var read = new Mock<IFgsApiEventReadRepository>();
        var validator = new UpdateFgsApiEventCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new UpdateFgsApiEventCommand(0, new FgsApiEventUpdateDto("WORKORDER.CREATED", "WORKORDER", "Work Order Created", null, 1, 1)));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task PatchValidator_RejectsInvalidId()
    {
        var read = new Mock<IFgsApiEventReadRepository>();
        var validator = new PatchFgsApiEventCommandValidator(read.Object);
        var result = await validator.ValidateAsync(
            new PatchFgsApiEventCommand(0, new FgsApiEventPatchDto("WORKORDER.CREATED", null, null, null, null, null, null)));

        result.IsValid.Should().BeFalse();
    }
}
