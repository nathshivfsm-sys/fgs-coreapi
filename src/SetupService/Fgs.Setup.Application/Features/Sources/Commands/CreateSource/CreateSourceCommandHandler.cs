using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Sources;
using Fgs.Setup.Application.Features.Sources.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.Sources.Commands.CreateSource;

public sealed class CreateSourceCommandHandler(
    ISourceWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateSourceCommandHandler> logger)
    : IRequestHandler<CreateSourceCommand, ApiResponse<SourceDetailDto>>
{
    public async Task<ApiResponse<SourceDetailDto>> Handle(
        CreateSourceCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created source {Id} with code {SourceCode}", result.Id, result.SourceCode);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "source"),
                cancellationToken);
        return ApiResponse<SourceDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
