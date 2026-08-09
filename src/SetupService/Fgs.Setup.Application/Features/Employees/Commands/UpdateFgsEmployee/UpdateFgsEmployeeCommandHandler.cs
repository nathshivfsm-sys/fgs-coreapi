using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Employees;
using Fgs.Setup.Application.Features.Employees.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.Employees.Commands.UpdateFgsEmployee;

public sealed class UpdateFgsEmployeeCommandHandler(
    IFgsEmployeeWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateFgsEmployeeCommandHandler> logger)
    : IRequestHandler<UpdateFgsEmployeeCommand, ApiResponse<FgsEmployeeDetailDto>>
{
    public async Task<ApiResponse<FgsEmployeeDetailDto>> Handle(
        UpdateFgsEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated employee {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "employees"),
            cancellationToken);
        return ApiResponse<FgsEmployeeDetailDto>.Ok(result);
    }
}
