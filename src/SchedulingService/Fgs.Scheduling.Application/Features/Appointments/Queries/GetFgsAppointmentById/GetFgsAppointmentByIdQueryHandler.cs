using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Scheduling.Application.Abstractions.Appointments;
using Fgs.Scheduling.Application.Features.Appointments.Dtos;
using MediatR;

namespace Fgs.Scheduling.Application.Features.Appointments.Queries.GetFgsAppointmentById;

public sealed class GetFgsAppointmentByIdQueryHandler(
    IFgsAppointmentReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsAppointmentByIdQuery, ApiResponse<FgsAppointmentDetailDto>>
{
    public async Task<ApiResponse<FgsAppointmentDetailDto>> Handle(
        GetFgsAppointmentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "appointment",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsAppointmentDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsAppointmentDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsAppointmentDetailDto>.Fail(
                [$"Appointment '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsAppointmentDetailDto>.Ok(result);
    }
}
