using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.JobTypes;
using Fgs.Setup.Application.Features.JobTypes.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.JobTypes.Commands.CreateJobType;

public sealed class CreateJobTypeCommandHandler(
    IJobTypeWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateJobTypeCommandHandler> logger)
    : IRequestHandler<CreateJobTypeCommand, ApiResponse<JobTypeDetailDto>>
{
    public async Task<ApiResponse<JobTypeDetailDto>> Handle(
        CreateJobTypeCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created job type {Id} with code {JobTypeCode}", result.Id, result.JobTypeCode);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "jobtype"),
                cancellationToken);
        return ApiResponse<JobTypeDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
