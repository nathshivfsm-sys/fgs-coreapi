using Fgs.Foundation.Paging;
using Fgs.User.Application.Abstractions.ApiWebhooks;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.ApiWebhooks.Commands.CreateFgsApiWebhook;
using Fgs.User.Application.Features.ApiWebhooks.Commands.PatchFgsApiWebhook;
using Fgs.User.Application.Features.ApiWebhooks.Commands.UpdateFgsApiWebhook;
using Fgs.User.Application.Features.ApiWebhooks.Dtos;
using Fgs.User.Application.Features.ApiWebhooks.Queries.GetFgsApiWebhookById;
using Fgs.User.Application.Features.ApiWebhooks.Queries.ListFgsApiWebhooks;
using Fgs.User.Application.Features.ApiWebhooks.Queries.LookupFgsApiWebhooks;
using Fgs.User.Application.Features.ApiWebhooks.Validators;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class ApiWebhookHandlerTests
{
    private static readonly FgsApiWebhookDetailDto Detail =
        new(1, "Orders Hook", "Desc", "https://hooks.example.com/orders", "NONE", null, null, 30, 5, null, true);

    [Fact]
    public async Task CreateHandler_ReturnsCreatedWebhook()
    {
        var write = new Mock<IFgsApiWebhookWriteService>();
        write.Setup(w => w.CreateAsync(It.IsAny<FgsApiWebhookCreateDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail);

        var handler = new CreateFgsApiWebhookCommandHandler(write.Object, NullLogger<CreateFgsApiWebhookCommandHandler>.Instance);
        var response = await handler.Handle(
            new CreateFgsApiWebhookCommand(new FgsApiWebhookCreateDto("Orders Hook", "Desc", "https://hooks.example.com/orders", "NONE", null, null)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data!.Name.Should().Be("Orders Hook");
    }

    [Fact]
    public async Task UpdateHandler_ReturnsUpdatedWebhook()
    {
        var write = new Mock<IFgsApiWebhookWriteService>();
        write.Setup(w => w.UpdateAsync(1, It.IsAny<FgsApiWebhookUpdateDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail);

        var handler = new UpdateFgsApiWebhookCommandHandler(write.Object, NullLogger<UpdateFgsApiWebhookCommandHandler>.Instance);
        var response = await handler.Handle(
            new UpdateFgsApiWebhookCommand(1, new FgsApiWebhookUpdateDto("Orders Hook", "Desc", "https://hooks.example.com/orders", "NONE", null, null, 30, 5)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Id.Should().Be(1);
    }

    [Fact]
    public async Task PatchHandler_ReturnsPatchedWebhook()
    {
        var write = new Mock<IFgsApiWebhookWriteService>();
        write.Setup(w => w.PatchAsync(1, It.IsAny<FgsApiWebhookPatchDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail with { IsActive = false });

        var handler = new PatchFgsApiWebhookCommandHandler(write.Object, NullLogger<PatchFgsApiWebhookCommandHandler>.Instance);
        var response = await handler.Handle(
            new PatchFgsApiWebhookCommand(1, new FgsApiWebhookPatchDto(null, null, null, null, null, null, null, null, false)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task GetByIdHandler_WhenFound_ReturnsWebhook()
    {
        var read = new Mock<IFgsApiWebhookReadRepository>();
        read.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(Detail);

        var handler = new GetFgsApiWebhookByIdQueryHandler(read.Object);
        var response = await handler.Handle(new GetFgsApiWebhookByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Name.Should().Be("Orders Hook");
    }

    [Fact]
    public async Task GetByIdHandler_WhenMissing_ReturnsNotFound()
    {
        var read = new Mock<IFgsApiWebhookReadRepository>();
        read.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((FgsApiWebhookDetailDto?)null);

        var handler = new GetFgsApiWebhookByIdQueryHandler(read.Object);
        var response = await handler.Handle(new GetFgsApiWebhookByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task ListHandler_ReturnsPagedResults()
    {
        var summary = new FgsApiWebhookSummaryDto(1, "Orders Hook", "Desc", "https://hooks.example.com/orders", "NONE", 30, 5, null, true);
        var paged = new PagedResult<FgsApiWebhookSummaryDto>([summary], 1, 25, 1);
        var read = new Mock<IFgsApiWebhookReadRepository>();
        read.Setup(r => r.ListAsync(It.IsAny<IdentityListQuery>(), It.IsAny<FgsApiWebhookListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(paged);

        var handler = new ListFgsApiWebhooksQueryHandler(read.Object);
        var response = await handler.Handle(
            new ListFgsApiWebhooksQuery(new IdentityListQuery(), new FgsApiWebhookListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Items.Should().ContainSingle();
    }

    [Fact]
    public async Task LookupHandler_ReturnsLookups()
    {
        var read = new Mock<IFgsApiWebhookReadRepository>();
        read.Setup(r => r.LookupAsync(true, It.IsAny<CancellationToken>()))
            .ReturnsAsync([new FgsApiWebhookLookupDto(1, "Orders Hook", "https://hooks.example.com/orders")]);

        var handler = new LookupFgsApiWebhooksQueryHandler(read.Object);
        var response = await handler.Handle(new LookupFgsApiWebhooksQuery(), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().ContainSingle();
    }

    [Fact]
    public async Task CreateValidator_RejectsEmptyName()
    {
        var validator = new CreateFgsApiWebhookCommandValidator();
        var result = await validator.ValidateAsync(
            new CreateFgsApiWebhookCommand(new FgsApiWebhookCreateDto(string.Empty, null, "https://hooks.example.com/orders", "NONE", null, null)));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task CreateValidator_AcceptsValidPayload()
    {
        var validator = new CreateFgsApiWebhookCommandValidator();
        var result = await validator.ValidateAsync(
            new CreateFgsApiWebhookCommand(new FgsApiWebhookCreateDto("Orders Hook", null, "https://hooks.example.com/orders", "NONE", null, null)));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateValidator_RejectsInvalidId()
    {
        var validator = new UpdateFgsApiWebhookCommandValidator();
        var result = await validator.ValidateAsync(
            new UpdateFgsApiWebhookCommand(0, new FgsApiWebhookUpdateDto("Orders Hook", null, "https://hooks.example.com/orders", "NONE", null, null, 30, 5)));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task PatchValidator_RejectsInvalidId()
    {
        var validator = new PatchFgsApiWebhookCommandValidator();
        var result = await validator.ValidateAsync(
            new PatchFgsApiWebhookCommand(0, new FgsApiWebhookPatchDto("Orders Hook", null, null, null, null, null, null, null, null)));

        result.IsValid.Should().BeFalse();
    }
}
