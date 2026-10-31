using Fgs.Contracts.Api;
using Fgs.Crm.Application.Abstractions.Customers;
using Fgs.Crm.Application.Features.Customers.Dtos;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Crm.Application.Features.Customers.Commands.UpdateCrmCustomerNote;

public sealed class UpdateCrmCustomerNoteCommandHandler(
    ICrmCustomerWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateCrmCustomerNoteCommandHandler> logger)
    : IRequestHandler<UpdateCrmCustomerNoteCommand, ApiResponse<CrmNoteDetailDto>>
{
    public async Task<ApiResponse<CrmNoteDetailDto>> Handle(
        UpdateCrmCustomerNoteCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateNoteAsync(
            request.CustomerId,
            request.NoteId,
            request.Dto,
            cancellationToken);
        logger.LogInformation(
            "Updated note {NoteId} for customer {CustomerId}",
            result.Id,
            result.CustomerId);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "customer"),
            cancellationToken);
        return ApiResponse<CrmNoteDetailDto>.Ok(result);
    }
}
