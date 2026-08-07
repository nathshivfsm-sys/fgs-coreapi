using Fgs.Audit.Application.Abstractions;
using Fgs.Audit.Application.Features.ArchiveCatalogs.Commands.UpsertArchiveCatalog;
using Fgs.Audit.Application.Features.ArchiveCatalogs.Dtos;
using Fgs.Audit.Application.Features.ArchiveCatalogs.Queries.GetArchiveCatalogById;
using Fgs.Audit.Application.Features.ArchiveCatalogs.Queries.ListArchiveCatalogs;
using Fgs.Contracts.Api;
using Fgs.Contracts.Audit;
using FluentAssertions;
using Moq;

namespace Fgs.Audit.Tests;

public sealed class ArchiveCatalogHandlerTests
{
    [Fact]
    public async Task Upsert_WithValidRequest_ReturnsCreated()
    {
        var dto = new ArchiveCatalogDto(
            1,
            new DateOnly(2026, 7, 1),
            "s3://bucket/audit/2026-07",
            1024,
            DateTime.UtcNow);

        var writer = new Mock<IArchiveCatalogWriter>();
        writer
            .Setup(w => w.UpsertAsync(It.IsAny<UpsertArchiveCatalogRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((dto, true));

        var handler = new UpsertArchiveCatalogCommandHandler(writer.Object);
        var response = await handler.Handle(
            new UpsertArchiveCatalogCommand(
                new UpsertArchiveCatalogRequest(new DateOnly(2026, 7, 15), "s3://bucket/audit/2026-07", 1024)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Created);
        response.Data.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task Upsert_WhenExisting_ReturnsOk()
    {
        var dto = new ArchiveCatalogDto(
            1,
            new DateOnly(2026, 7, 1),
            "s3://bucket/audit/2026-07",
            2048,
            DateTime.UtcNow);

        var writer = new Mock<IArchiveCatalogWriter>();
        writer
            .Setup(w => w.UpsertAsync(It.IsAny<UpsertArchiveCatalogRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((dto, false));

        var handler = new UpsertArchiveCatalogCommandHandler(writer.Object);
        var response = await handler.Handle(
            new UpsertArchiveCatalogCommand(
                new UpsertArchiveCatalogRequest(new DateOnly(2026, 7, 1), "s3://bucket/audit/2026-07", 2048)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
    }

    [Fact]
    public async Task Upsert_WithBlankStoragePath_ReturnsBadRequest()
    {
        var writer = new Mock<IArchiveCatalogWriter>();
        var handler = new UpsertArchiveCatalogCommandHandler(writer.Object);

        var response = await handler.Handle(
            new UpsertArchiveCatalogCommand(
                new UpsertArchiveCatalogRequest(new DateOnly(2026, 7, 1), "  ", 10)),
            CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.BadRequest);
        writer.Verify(
            w => w.UpsertAsync(It.IsAny<UpsertArchiveCatalogRequest>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Upsert_WithNegativeFileSize_ReturnsBadRequest()
    {
        var writer = new Mock<IArchiveCatalogWriter>();
        var handler = new UpsertArchiveCatalogCommandHandler(writer.Object);

        var response = await handler.Handle(
            new UpsertArchiveCatalogCommand(
                new UpsertArchiveCatalogRequest(new DateOnly(2026, 7, 1), "path", -1)),
            CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.BadRequest);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var read = new Mock<IArchiveCatalogReadRepository>();
        read.Setup(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ArchiveCatalogDto?)null);

        var handler = new GetArchiveCatalogByIdQueryHandler(read.Object);
        var response = await handler.Handle(new GetArchiveCatalogByIdQuery(3), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsOk()
    {
        var items = new List<ArchiveCatalogDto>
        {
            new(1, new DateOnly(2026, 7, 1), "a", 1, DateTime.UtcNow)
        };

        var read = new Mock<IArchiveCatalogReadRepository>();
        read.Setup(r => r.ListAsync(It.IsAny<CancellationToken>())).ReturnsAsync(items);

        var handler = new ListArchiveCatalogsQueryHandler(read.Object);
        var response = await handler.Handle(new ListArchiveCatalogsQuery(), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().HaveCount(1);
    }
}
