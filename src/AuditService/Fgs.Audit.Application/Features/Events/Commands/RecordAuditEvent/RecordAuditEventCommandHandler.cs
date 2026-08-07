using Fgs.Audit.Application.Abstractions;
using Fgs.Audit.Application.Features.Events.Dtos;
using Fgs.Audit.Domain.Enums;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Audit.Application.Features.Events.Commands.RecordAuditEvent;

public sealed class RecordAuditEventCommandHandler(IAuditEventWriter writer)
    : IRequestHandler<RecordAuditEventCommand, ApiResponse<AuditEventDetailDto>>
{
    public async Task<ApiResponse<AuditEventDetailDto>> Handle(
        RecordAuditEventCommand request,
        CancellationToken cancellationToken)
    {
        var body = request.Request;
        if (string.IsNullOrWhiteSpace(body.EventCode))
        {
            return ApiResponse<AuditEventDetailDto>.Fail(
                ["EventCode is required."],
                ApiStatusCodes.BadRequest);
        }

        if (string.IsNullOrWhiteSpace(body.Summary))
        {
            return ApiResponse<AuditEventDetailDto>.Fail(
                ["Summary is required."],
                ApiStatusCodes.BadRequest);
        }

        if (!Enum.TryParse<AuditEventSource>(body.EventSource, ignoreCase: true, out _))
        {
            return ApiResponse<AuditEventDetailDto>.Fail(
                [$"EventSource '{body.EventSource}' is invalid."],
                ApiStatusCodes.BadRequest);
        }

        if (!Enum.TryParse<AuditRecordType>(body.RecordType, ignoreCase: true, out _))
        {
            return ApiResponse<AuditEventDetailDto>.Fail(
                [$"RecordType '{body.RecordType}' is invalid."],
                ApiStatusCodes.BadRequest);
        }

        if (body.Details is not null)
        {
            foreach (var detail in body.Details)
            {
                if (!Enum.TryParse<AuditEventDetailType>(detail.EntryType, ignoreCase: true, out _))
                {
                    return ApiResponse<AuditEventDetailDto>.Fail(
                        [$"Detail EntryType '{detail.EntryType}' is invalid."],
                        ApiStatusCodes.BadRequest);
                }

                if (string.IsNullOrWhiteSpace(detail.ItemName))
                {
                    return ApiResponse<AuditEventDetailDto>.Fail(
                        ["Detail ItemName is required."],
                        ApiStatusCodes.BadRequest);
                }
            }
        }

        var result = await writer.WriteAsync(body, cancellationToken);
        return ApiResponse<AuditEventDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
