using Fgs.Contracts.Api;
using Fgs.Crm.Application.Abstractions.Customers;
using Fgs.Crm.Application.Features.Customers.Dtos;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Crm.Application.Features.Customers.Commands.AddCrmCustomerNote;

public sealed class AddCrmCustomerNoteCommandHandler(
    ICrmCustomerWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<AddCrmCustomerNoteCommandHandler> logger)
    : IRequestHandler<AddCrmCustomerNoteCommand, ApiResponse<CrmNoteDetailDto>>
{
    public async Task<ApiResponse<CrmNoteDetailDto>> Handle(
        AddCrmCustomerNoteCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.AddNoteAsync(request.CustomerId, request.Dto, cancellationToken);
        logger.LogInformation(
            "Created note {NoteId} for customer {CustomerId}",
            result.Id,
            result.CustomerId);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "customer"),
            cancellationToken);
        return ApiResponse<CrmNoteDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
