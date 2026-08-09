using Fgs.Audit.Application.Abstractions;
using Fgs.Audit.Application.Features.Events.Dtos;
using Fgs.Audit.Application.Features.Events.Queries.GetAuditEventById;
using Fgs.Audit.Application.Features.Events.Queries.ListAuditEventsByEntity;
using Fgs.Audit.Domain.Enums;
using Fgs.Contracts.Api;
using FluentAssertions;
using Moq;

namespace Fgs.Audit.Tests;

public sealed class AuditEventQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var detail = new AuditEventDetailDto(
            Id: 5,
            TenantId: 1,
            CompanyId: 2,
            EventCode: "INV_PAID",
            EventSource: "WEB",
            RecordType: "INVOICE",
            EntityId: 55,
            EntityNumber: null,
            UserName: null,
            Summary: "Invoice paid",
            OccurredOn: DateTime.UtcNow,
            CreatedOn: DateTime.UtcNow,
            Details: [],
            Attachments: []);

        var read = new Mock<IAuditEventReadRepository>();
        read.Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        var handler = new GetAuditEventByIdQueryHandler(read.Object);
        var response = await handler.Handle(new GetAuditEventByIdQuery(5), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Id.Should().Be(5);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var read = new Mock<IAuditEventReadRepository>();
        read.Setup(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((AuditEventDetailDto?)null);

        var handler = new GetAuditEventByIdQueryHandler(read.Object);
        var response = await handler.Handle(new GetAuditEventByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task ListByEntity_WithValidRecordType_ReturnsOk()
    {
        var summaries = new List<AuditEventSummaryDto>
        {
            new(
                1, 1, 2, "WO_UPDATED", "API", "WORK_ORDER", 10, "WO-10", null,
                "Updated", DateTime.UtcNow, DateTime.UtcNow)
        };

        var read = new Mock<IAuditEventReadRepository>();
        read.Setup(r => r.ListByEntityAsync(
                AuditRecordType.WORK_ORDER,
                10,
                1,
                2,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(summaries);

        var handler = new ListAuditEventsByEntityQueryHandler(read.Object);
        var response = await handler.Handle(
            new ListAuditEventsByEntityQuery("WORK_ORDER", 10, 1, 2),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().HaveCount(1);
    }

    [Fact]
    public async Task ListByEntity_WithInvalidRecordType_ReturnsBadRequest()
    {
        var read = new Mock<IAuditEventReadRepository>();
        var handler = new ListAuditEventsByEntityQueryHandler(read.Object);

        var response = await handler.Handle(
            new ListAuditEventsByEntityQuery("NOPE", 10),
            CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.BadRequest);
        read.Verify(
            r => r.ListByEntityAsync(
                It.IsAny<AuditRecordType>(),
                It.IsAny<long>(),
                It.IsAny<long?>(),
                It.IsAny<long?>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
