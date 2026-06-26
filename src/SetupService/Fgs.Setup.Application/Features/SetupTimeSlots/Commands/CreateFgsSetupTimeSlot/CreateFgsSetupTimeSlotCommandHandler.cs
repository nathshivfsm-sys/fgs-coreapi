using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupTimeSlots;
using Fgs.Setup.Application.Features.SetupTimeSlots.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.SetupTimeSlots.Commands.CreateFgsSetupTimeSlot;

public sealed class CreateFgsSetupTimeSlotCommandHandler(
    IFgsSetupTimeSlotWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsSetupTimeSlotCommandHandler> logger)
    : IRequestHandler<CreateFgsSetupTimeSlotCommand, ApiResponse<FgsSetupTimeSlotDetailDto>>
{
    public async Task<ApiResponse<FgsSetupTimeSlotDetailDto>> Handle(
        CreateFgsSetupTimeSlotCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created time slot {Id} with code {Code}", result.Id, result.Code);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "timeslots"),
                cancellationToken);
        return ApiResponse<FgsSetupTimeSlotDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
