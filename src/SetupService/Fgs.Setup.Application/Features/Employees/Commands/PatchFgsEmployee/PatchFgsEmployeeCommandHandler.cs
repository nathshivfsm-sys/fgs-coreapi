using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Employees;
using Fgs.Setup.Application.Features.Employees.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.Employees.Commands.PatchFgsEmployee;

public sealed class PatchFgsEmployeeCommandHandler(
    IFgsEmployeeWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<PatchFgsEmployeeCommandHandler> logger)
    : IRequestHandler<PatchFgsEmployeeCommand, ApiResponse<FgsEmployeeDetailDto>>
{
    public async Task<ApiResponse<FgsEmployeeDetailDto>> Handle(
        PatchFgsEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patched employee {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "employees"),
            cancellationToken);
        return ApiResponse<FgsEmployeeDetailDto>.Ok(result);
    }
}
