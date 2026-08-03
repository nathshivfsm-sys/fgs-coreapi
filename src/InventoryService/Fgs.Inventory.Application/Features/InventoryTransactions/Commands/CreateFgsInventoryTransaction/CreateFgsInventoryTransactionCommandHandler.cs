using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.InventoryTransactions;
using Fgs.Inventory.Application.Features.InventoryTransactions.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Inventory.Application.Features.InventoryTransactions.Commands.CreateFgsInventoryTransaction;

public sealed class CreateFgsInventoryTransactionCommandHandler(
    IFgsInventoryTransactionWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsInventoryTransactionCommandHandler> logger)
    : IRequestHandler<CreateFgsInventoryTransactionCommand, ApiResponse<FgsInventoryTransactionDetailDto>>
{
    public async Task<ApiResponse<FgsInventoryTransactionDetailDto>> Handle(
        CreateFgsInventoryTransactionCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created inventory transaction {Id} with number {TransactionNumber}", result.Id, result.TransactionNumber);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "inventorytransaction"),
            cancellationToken);
        return ApiResponse<FgsInventoryTransactionDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
