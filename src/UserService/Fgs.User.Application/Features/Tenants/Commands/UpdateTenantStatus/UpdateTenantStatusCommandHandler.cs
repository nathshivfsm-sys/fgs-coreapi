using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Persistence.Abstractions;
using Fgs.User.Domain.Entities;
using MediatR;

namespace Fgs.User.Application.Features.Tenants.Commands.UpdateTenantStatus;

public sealed class UpdateTenantStatusCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateTenantStatusCommand, ApiResponse<object>>
{
    public async Task<ApiResponse<object>> Handle(
        UpdateTenantStatusCommand request,
        CancellationToken cancellationToken)
    {
        var tenant = await unitOfWork.Repository<FgsTenant>()
            .FirstOrDefaultAsync(t => t.Id == request.TenantId, cancellationToken);

        if (tenant is null)
        {
            return ApiResponse<object>.Fail(["Tenant not found."], ApiStatusCodes.NotFound);
        }

        tenant.FgsTenantStatusId = request.Request.FgsTenantStatusId;
        tenant.UpdatedOn = DateTimeOffset.UtcNow;
        if (request.Request.FgsTenantStatusId == TenantStatusIds.Active)
        {
            tenant.IsActive = true;
        }

        unitOfWork.Repository<FgsTenant>().Update(tenant);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<object>.Ok(new object());
    }
}
