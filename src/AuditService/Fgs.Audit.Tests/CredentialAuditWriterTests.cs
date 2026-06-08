using Fgs.Audit.Infrastructure.Audit;
using Fgs.Audit.Infrastructure.Database;
using Fgs.Contracts.CredentialAudit;
using Fgs.MultiTenancy;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Fgs.Audit.Tests;

public sealed class CredentialAuditWriterTests
{
    [Fact]
    public async Task WriteAsync_PersistsCredentialAuditRecord()
    {
        var options = new DbContextOptionsBuilder<FgsAuditDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var tenantAccessor = new Mock<ITenantContextAccessor>();
        await using var context = new FgsAuditDbContext(options, tenantAccessor.Object);
        var writer = new CredentialAuditWriter(context);
        var credentialId = Guid.NewGuid();

        await writer.WriteAsync(new RecordCredentialAuditRequest(
            TenantId: 1,
            CompanyId: 2,
            CredentialId: credentialId,
            ActionType: CredentialAuditActions.Created,
            Remarks: "Credential created.",
            CreatedBy: "test"));

        var stored = await context.FgsCredentialAudits.SingleAsync();
        stored.TenantId.Should().Be(1);
        stored.CompanyId.Should().Be(2);
        stored.CredentialId.Should().Be(credentialId);
        stored.ActionType.Should().Be(CredentialAuditActions.Created);
        stored.Remarks.Should().Be("Credential created.");
    }
}
