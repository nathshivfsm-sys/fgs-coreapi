using Fgs.Audit.Infrastructure.Audit;
using Fgs.Audit.Infrastructure.Database;
using Fgs.Contracts.Audit;
using Fgs.MultiTenancy;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Fgs.Audit.Tests;

public sealed class AuditEventWriterTests
{
    [Fact]
    public async Task WriteAsync_PersistsEventWithDetailsAndAttachments()
    {
        var options = new DbContextOptionsBuilder<FgsAuditDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var tenantAccessor = new Mock<ITenantContextAccessor>();
        await using var context = new FgsAuditDbContext(options, tenantAccessor.Object);
        var writer = new AuditEventWriter(context);

        var result = await writer.WriteAsync(new RecordAuditEventRequest(
            TenantId: 1,
            CompanyId: 2,
            EventCode: "WO_CREATED",
            EventSource: "API",
            RecordType: "WORK_ORDER",
            EntityId: 42,
            Summary: "Created",
            EntityNumber: "WO-42",
            UserName: "alice",
            Details:
            [
                new RecordAuditEventDetailRequest("FIELD_CHANGE", "Status", null, "Open")
            ],
            Attachments:
            [
                new RecordAuditEventAttachmentRequest(99, "photo")
            ]));

        result.Id.Should().BeGreaterThan(0);
        result.Details.Should().HaveCount(1);
        result.Attachments.Should().HaveCount(1);

        var stored = await context.FgsEvents
            .Include(e => e.Details)
            .Include(e => e.Attachments)
            .SingleAsync();
        stored.TenantId.Should().Be(1);
        stored.EventCode.Should().Be("WO_CREATED");
        stored.Details.Should().HaveCount(1);
        stored.Attachments.Single().DocumentId.Should().Be(99);
    }
}
