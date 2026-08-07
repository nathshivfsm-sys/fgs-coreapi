using Fgs.Audit.Application.Abstractions;
using Fgs.Audit.Application.Features.Events.Commands.RecordAuditEvent;
using Fgs.Audit.Application.Features.Events.Dtos;
using Fgs.Contracts.Api;
using Fgs.Contracts.Audit;
using FluentAssertions;
using Moq;

namespace Fgs.Audit.Tests;

public sealed class RecordAuditEventCommandHandlerTests
{
    [Fact]
    public async Task Handle_WithValidRequest_ReturnsCreated()
    {
        var detail = new AuditEventDetailDto(
            Id: 10,
            TenantId: 1,
            CompanyId: 2,
            EventCode: "WO_CREATED",
            EventSource: "API",
            RecordType: "WORK_ORDER",
            EntityId: 100,
            EntityNumber: "WO-100",
            UserName: "tech",
            Summary: "Work order created",
            OccurredOn: DateTime.UtcNow,
            CreatedOn: DateTime.UtcNow,
            Details: [],
            Attachments: []);

        var writer = new Mock<IAuditEventWriter>();
        writer
            .Setup(w => w.WriteAsync(It.IsAny<RecordAuditEventRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(detail);

        var handler = new RecordAuditEventCommandHandler(writer.Object);
        var response = await handler.Handle(
            new RecordAuditEventCommand(ValidRequest()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Created);
        response.Data.Should().BeEquivalentTo(detail);
        writer.Verify(
            w => w.WriteAsync(It.IsAny<RecordAuditEventRequest>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithInvalidEventSource_ReturnsBadRequest()
    {
        var writer = new Mock<IAuditEventWriter>();
        var handler = new RecordAuditEventCommandHandler(writer.Object);

        var response = await handler.Handle(
            new RecordAuditEventCommand(ValidRequest() with { EventSource = "NOT_A_SOURCE" }),
            CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.BadRequest);
        writer.Verify(
            w => w.WriteAsync(It.IsAny<RecordAuditEventRequest>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WithMissingSummary_ReturnsBadRequest()
    {
        var writer = new Mock<IAuditEventWriter>();
        var handler = new RecordAuditEventCommandHandler(writer.Object);

        var response = await handler.Handle(
            new RecordAuditEventCommand(ValidRequest() with { Summary = " " }),
            CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.BadRequest);
    }

    [Fact]
    public async Task Handle_WithInvalidDetailEntryType_ReturnsBadRequest()
    {
        var writer = new Mock<IAuditEventWriter>();
        var handler = new RecordAuditEventCommandHandler(writer.Object);

        var response = await handler.Handle(
            new RecordAuditEventCommand(ValidRequest() with
            {
                Details =
                [
                    new RecordAuditEventDetailRequest("BAD_TYPE", "Status")
                ]
            }),
            CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.BadRequest);
        writer.Verify(
            w => w.WriteAsync(It.IsAny<RecordAuditEventRequest>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    private static RecordAuditEventRequest ValidRequest() =>
        new(
            TenantId: 1,
            CompanyId: 2,
            EventCode: "WO_CREATED",
            EventSource: "API",
            RecordType: "WORK_ORDER",
            EntityId: 100,
            Summary: "Work order created",
            Details:
            [
                new RecordAuditEventDetailRequest("FIELD_CHANGE", "Status", "New", "Open")
            ]);
}
