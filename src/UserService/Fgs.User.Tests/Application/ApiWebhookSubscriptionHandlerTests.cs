using Fgs.Foundation.Paging;
using Fgs.User.Application.Abstractions.ApiWebhookSubscriptions;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.ApiWebhookSubscriptions.Commands.CreateFgsApiWebhookSubscription;
using Fgs.User.Application.Features.ApiWebhookSubscriptions.Commands.DeleteFgsApiWebhookSubscription;
using Fgs.User.Application.Features.ApiWebhookSubscriptions.Dtos;
using Fgs.User.Application.Features.ApiWebhookSubscriptions.Queries.GetFgsApiWebhookSubscriptionById;
using Fgs.User.Application.Features.ApiWebhookSubscriptions.Queries.ListFgsApiWebhookSubscriptions;
using Fgs.User.Application.Features.ApiWebhookSubscriptions.Validators;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class ApiWebhookSubscriptionHandlerTests
{
    private static readonly DateTimeOffset Now = DateTimeOffset.UtcNow;

    private static readonly FgsApiWebhookSubscriptionDetailDto Detail =
        new(1, 10, 20, Now, "test");

    [Fact]
    public async Task CreateHandler_ReturnsCreatedSubscription()
    {
        var write = new Mock<IFgsApiWebhookSubscriptionWriteService>();
        write.Setup(w => w.CreateAsync(It.IsAny<FgsApiWebhookSubscriptionCreateDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Detail);

        var handler = new CreateFgsApiWebhookSubscriptionCommandHandler(
            write.Object,
            NullLogger<CreateFgsApiWebhookSubscriptionCommandHandler>.Instance);
        var response = await handler.Handle(
            new CreateFgsApiWebhookSubscriptionCommand(new FgsApiWebhookSubscriptionCreateDto(10, 20)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data!.FgsApiWebhookId.Should().Be(10);
    }

    [Fact]
    public async Task DeleteHandler_RemovesSubscription()
    {
        var write = new Mock<IFgsApiWebhookSubscriptionWriteService>();
        write.Setup(w => w.DeleteAsync(1, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var handler = new DeleteFgsApiWebhookSubscriptionCommandHandler(
            write.Object,
            NullLogger<DeleteFgsApiWebhookSubscriptionCommandHandler>.Instance);
        var response = await handler.Handle(new DeleteFgsApiWebhookSubscriptionCommand(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        write.Verify(w => w.DeleteAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdHandler_WhenFound_ReturnsSubscription()
    {
        var read = new Mock<IFgsApiWebhookSubscriptionReadRepository>();
        read.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(Detail);

        var handler = new GetFgsApiWebhookSubscriptionByIdQueryHandler(read.Object);
        var response = await handler.Handle(new GetFgsApiWebhookSubscriptionByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.FgsApiEventId.Should().Be(20);
    }

    [Fact]
    public async Task GetByIdHandler_WhenMissing_ReturnsNotFound()
    {
        var read = new Mock<IFgsApiWebhookSubscriptionReadRepository>();
        read.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync((FgsApiWebhookSubscriptionDetailDto?)null);

        var handler = new GetFgsApiWebhookSubscriptionByIdQueryHandler(read.Object);
        var response = await handler.Handle(new GetFgsApiWebhookSubscriptionByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task ListHandler_ReturnsPagedResults()
    {
        var summary = new FgsApiWebhookSubscriptionSummaryDto(1, 10, 20, Now, "test");
        var paged = new PagedResult<FgsApiWebhookSubscriptionSummaryDto>([summary], 1, 25, 1);
        var read = new Mock<IFgsApiWebhookSubscriptionReadRepository>();
        read.Setup(r => r.ListAsync(It.IsAny<IdentityListQuery>(), It.IsAny<FgsApiWebhookSubscriptionListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(paged);

        var handler = new ListFgsApiWebhookSubscriptionsQueryHandler(read.Object);
        var response = await handler.Handle(
            new ListFgsApiWebhookSubscriptionsQuery(new IdentityListQuery(), new FgsApiWebhookSubscriptionListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Items.Should().ContainSingle();
    }

    [Fact]
    public async Task CreateValidator_RejectsInvalidIds()
    {
        var validator = new CreateFgsApiWebhookSubscriptionCommandValidator();
        var result = await validator.ValidateAsync(
            new CreateFgsApiWebhookSubscriptionCommand(new FgsApiWebhookSubscriptionCreateDto(0, 0)));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task CreateValidator_AcceptsValidPayload()
    {
        var validator = new CreateFgsApiWebhookSubscriptionCommandValidator();
        var result = await validator.ValidateAsync(
            new CreateFgsApiWebhookSubscriptionCommand(new FgsApiWebhookSubscriptionCreateDto(10, 20)));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteValidator_RejectsInvalidId()
    {
        var validator = new DeleteFgsApiWebhookSubscriptionCommandValidator();
        var result = await validator.ValidateAsync(new DeleteFgsApiWebhookSubscriptionCommand(0));

        result.IsValid.Should().BeFalse();
    }
}
