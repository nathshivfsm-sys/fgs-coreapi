using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Employees;
using Fgs.Setup.Application.Features.Employees.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.Employees.Commands.CreateFgsEmployee;

public sealed class CreateFgsEmployeeCommandHandler(
    IFgsEmployeeWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsEmployeeCommandHandler> logger)
    : IRequestHandler<CreateFgsEmployeeCommand, ApiResponse<FgsEmployeeDetailDto>>
{
    public async Task<ApiResponse<FgsEmployeeDetailDto>> Handle(
        CreateFgsEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation(
            "Created employee {Id} with number {EmployeeNumber}",
            result.Id,
            result.EmployeeNumber);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "employees"),
            cancellationToken);
        return ApiResponse<FgsEmployeeDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
