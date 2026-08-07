using Fgs.Audit.Application.Abstractions;
using Fgs.Audit.Application.Features.Events.Dtos;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Audit.Application.Features.Events.Queries.GetAuditEventById;

public sealed class GetAuditEventByIdQueryHandler(IAuditEventReadRepository readRepository)
    : IRequestHandler<GetAuditEventByIdQuery, ApiResponse<AuditEventDetailDto>>
{
    public async Task<ApiResponse<AuditEventDetailDto>> Handle(
        GetAuditEventByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<AuditEventDetailDto>.Fail(
                [$"Audit event '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        return ApiResponse<AuditEventDetailDto>.Ok(result);
    }
}
