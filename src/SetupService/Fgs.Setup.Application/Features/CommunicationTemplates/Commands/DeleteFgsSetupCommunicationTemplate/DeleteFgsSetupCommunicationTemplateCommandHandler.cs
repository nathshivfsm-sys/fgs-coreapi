using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.CommunicationTemplates;
using Fgs.Setup.Application.Features.CommunicationTemplates.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.CommunicationTemplates.Commands.DeleteFgsSetupCommunicationTemplate;

public sealed class DeleteFgsSetupCommunicationTemplateCommandHandler(
    IFgsSetupCommunicationTemplateWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<DeleteFgsSetupCommunicationTemplateCommandHandler> logger)
    : IRequestHandler<DeleteFgsSetupCommunicationTemplateCommand, ApiResponse<FgsSetupCommunicationTemplateDetailDto>>
{
    public async Task<ApiResponse<FgsSetupCommunicationTemplateDetailDto>> Handle(
        DeleteFgsSetupCommunicationTemplateCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.DeleteAsync(request.Id, cancellationToken);
        logger.LogInformation("Soft-deleted communication template {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "communicationtemplate"),
                cancellationToken);
        return ApiResponse<FgsSetupCommunicationTemplateDetailDto>.Ok(result);
    }
}
