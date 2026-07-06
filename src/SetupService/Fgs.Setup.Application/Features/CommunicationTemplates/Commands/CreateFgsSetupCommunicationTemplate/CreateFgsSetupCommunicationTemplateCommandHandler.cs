using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.CommunicationTemplates;
using Fgs.Setup.Application.Features.CommunicationTemplates.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.CommunicationTemplates.Commands.CreateFgsSetupCommunicationTemplate;

public sealed class CreateFgsSetupCommunicationTemplateCommandHandler(
    IFgsSetupCommunicationTemplateWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsSetupCommunicationTemplateCommandHandler> logger)
    : IRequestHandler<CreateFgsSetupCommunicationTemplateCommand, ApiResponse<FgsSetupCommunicationTemplateDetailDto>>
{
    public async Task<ApiResponse<FgsSetupCommunicationTemplateDetailDto>> Handle(
        CreateFgsSetupCommunicationTemplateCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created communication template {Id} with code {Code}", result.Id, result.Code);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "communication-template"),
                cancellationToken);
        return ApiResponse<FgsSetupCommunicationTemplateDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
