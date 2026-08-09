using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Scheduling.Application.Abstractions.Appointments;
using Fgs.Scheduling.Application.Features.Appointments.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Scheduling.Application.Features.Appointments.Commands.CreateFgsAppointment;

public sealed class CreateFgsAppointmentCommandHandler(
    IFgsAppointmentWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsAppointmentCommandHandler> logger)
    : IRequestHandler<CreateFgsAppointmentCommand, ApiResponse<FgsAppointmentDetailDto>>
{
    public async Task<ApiResponse<FgsAppointmentDetailDto>> Handle(
        CreateFgsAppointmentCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation(
            "Created appointment {Id} for source {SourceTypeId}/{SourceId}",
            result.Id,
            result.SourceTypeId,
            result.SourceId);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "appointment"),
            cancellationToken);
        return ApiResponse<FgsAppointmentDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
