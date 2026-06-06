using FluentValidation;
using Fgs.Contracts.Requests;

namespace Fgs.Setup.Application.Features.TenantProvisioning.Commands.ProvisionTenant;

public sealed class ProvisionTenantCommandValidator : AbstractValidator<ProvisionTenantCommand>
{
    public ProvisionTenantCommandValidator()
    {
        RuleFor(x => x.Request.TenantId).GreaterThan(0);
        RuleFor(x => x.Request.CompanyId).GreaterThan(0);
        RuleFor(x => x.Request.TenantCode).NotEmpty();
        RuleFor(x => x.Request.CorrelationId).NotEmpty();
    }
}
