using Fgs.Audit.Infrastructure.Audit;
using Fgs.Audit.Infrastructure.Database;
using Fgs.Contracts.Audit;
using Fgs.MultiTenancy;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Fgs.Audit.Tests;

public sealed class ArchiveCatalogWriterTests
{
    [Fact]
    public async Task UpsertAsync_CreatesThenUpdatesByArchiveMonth()
    {
        var options = new DbContextOptionsBuilder<FgsAuditDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var tenantAccessor = new Mock<ITenantContextAccessor>();
        await using var context = new FgsAuditDbContext(options, tenantAccessor.Object);
        var writer = new ArchiveCatalogWriter(context);

        var (created, wasCreated) = await writer.UpsertAsync(
            new UpsertArchiveCatalogRequest(new DateOnly(2026, 8, 15), "path/v1", 100));

        wasCreated.Should().BeTrue();
        created.ArchiveMonth.Should().Be(new DateOnly(2026, 8, 1));
        created.StoragePath.Should().Be("path/v1");

        var (updated, wasUpdatedCreate) = await writer.UpsertAsync(
            new UpsertArchiveCatalogRequest(new DateOnly(2026, 8, 1), "path/v2", 200));

        wasUpdatedCreate.Should().BeFalse();
        updated.Id.Should().Be(created.Id);
        updated.StoragePath.Should().Be("path/v2");
        updated.FileSize.Should().Be(200);

        (await context.FgsArchiveCatalogs.CountAsync()).Should().Be(1);
    }
}
