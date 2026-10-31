using Fgs.Contracts.Api;
using Fgs.Crm.Application.Abstractions.Customers;
using Fgs.Crm.Application.Features.Customers.Dtos;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Crm.Application.Features.Customers.Commands.DeleteCrmCustomerNote;

public sealed class DeleteCrmCustomerNoteCommandHandler(
    ICrmCustomerWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<DeleteCrmCustomerNoteCommandHandler> logger)
    : IRequestHandler<DeleteCrmCustomerNoteCommand, ApiResponse<CrmNoteDetailDto>>
{
    public async Task<ApiResponse<CrmNoteDetailDto>> Handle(
        DeleteCrmCustomerNoteCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.DeleteNoteAsync(
            request.CustomerId,
            request.NoteId,
            cancellationToken);
        logger.LogInformation(
            "Soft-deleted note {NoteId} for customer {CustomerId}",
            result.Id,
            result.CustomerId);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "customer"),
            cancellationToken);
        return ApiResponse<CrmNoteDetailDto>.Ok(result);
    }
}
