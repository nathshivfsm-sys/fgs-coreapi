using Fgs.User.Application.Features.ServiceSetups.Commands.PatchFgsTenantServiceSetup;
using Fgs.User.Application.Features.ServiceSetups.Commands.UpdateFgsTenantServiceSetup;
using Fgs.User.Domain.Entities;
using Fgs.User.Domain.Enums;
using FluentValidation;

namespace Fgs.User.Application.Features.ServiceSetups.Validators;

public sealed class UpdateFgsTenantServiceSetupCommandValidator : AbstractValidator<UpdateFgsTenantServiceSetupCommand>
{
    public UpdateFgsTenantServiceSetupCommandValidator()
    {
        RuleFor(x => x.Dto.TimeCardOptionId).IsInEnum();
        RuleFor(x => x.Dto.BillHoursFromDispatchOrArrive)
            .NotEmpty()
            .MaximumLength(20)
            .Must(v => v.Equals("DISPATCH", StringComparison.OrdinalIgnoreCase)
                || v.Equals("ARRIVE", StringComparison.OrdinalIgnoreCase))
            .WithMessage("BillHoursFromDispatchOrArrive must be DISPATCH or ARRIVE.");
        RuleFor(x => x.Dto.WorkLocationRadiusForAutoArrive)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Dto.WorkLocationRadiusForAutoArrive.HasValue);
        RuleFor(x => x.Dto.BillToStartNumber).GreaterThan(0);
        RuleFor(x => x.Dto.POStartNumber).GreaterThan(0);
        RuleFor(x => x.Dto.QuoteStartNumber).GreaterThan(0);
        RuleFor(x => x.Dto.WorkOrderStartNumber).GreaterThan(0);
        RuleFor(x => x.Dto.InvoiceNumberPrefix).MaximumLength(20);
        RuleFor(x => x.Dto.QuoteNumberPrefix).MaximumLength(20);
        RuleFor(x => x.Dto.PONumberPrefix).MaximumLength(20);
        RuleFor(x => x.Dto.WorkOrderNumberPrefix).MaximumLength(20);
        RuleFor(x => x.Dto.InvoiceBatchNumberFormat).MaximumLength(200);
        RuleFor(x => x.Dto.EstimateRevisionCreationMode)
            .NotEmpty()
            .MaximumLength(50)
            .Must(IsValidEstimateRevisionCreationMode)
            .WithMessage(
                "EstimateRevisionCreationMode must be OnDemand or OnPostSignatureChange.");
        RuleFor(x => x.Dto)
            .Must(d => !d.OTStartTime.HasValue || !d.OTEndTime.HasValue || d.OTEndTime > d.OTStartTime)
            .WithMessage("OTEndTime must be greater than OTStartTime.");
        RuleFor(x => x.Dto)
            .Must(d => !d.DTStartTime.HasValue || !d.DTEndTime.HasValue || d.DTEndTime > d.DTStartTime)
            .WithMessage("DTEndTime must be greater than DTStartTime.");
    }

    private static bool IsValidEstimateRevisionCreationMode(string value) =>
        value.Equals(EstimateRevisionCreationModes.OnDemand, StringComparison.Ordinal)
        || value.Equals(EstimateRevisionCreationModes.OnPostSignatureChange, StringComparison.Ordinal);
}

public sealed class PatchFgsTenantServiceSetupCommandValidator : AbstractValidator<PatchFgsTenantServiceSetupCommand>
{
    public PatchFgsTenantServiceSetupCommandValidator()
    {
        RuleFor(x => x.Dto.TimeCardOptionId)
            .Must(v => v is null || Enum.IsDefined(typeof(TimeCardOption), v.Value))
            .WithMessage("TimeCardOptionId must be a valid value.");
        RuleFor(x => x.Dto.BillHoursFromDispatchOrArrive)
            .MaximumLength(20)
            .Must(v => v is null
                || v.Equals("DISPATCH", StringComparison.OrdinalIgnoreCase)
                || v.Equals("ARRIVE", StringComparison.OrdinalIgnoreCase))
            .WithMessage("BillHoursFromDispatchOrArrive must be DISPATCH or ARRIVE.")
            .When(x => x.Dto.BillHoursFromDispatchOrArrive is not null);
        RuleFor(x => x.Dto.WorkLocationRadiusForAutoArrive)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Dto.WorkLocationRadiusForAutoArrive.HasValue);
        RuleFor(x => x.Dto.BillToStartNumber).GreaterThan(0).When(x => x.Dto.BillToStartNumber.HasValue);
        RuleFor(x => x.Dto.POStartNumber).GreaterThan(0).When(x => x.Dto.POStartNumber.HasValue);
        RuleFor(x => x.Dto.QuoteStartNumber).GreaterThan(0).When(x => x.Dto.QuoteStartNumber.HasValue);
        RuleFor(x => x.Dto.WorkOrderStartNumber).GreaterThan(0).When(x => x.Dto.WorkOrderStartNumber.HasValue);
        RuleFor(x => x.Dto.InvoiceNumberPrefix).MaximumLength(20).When(x => x.Dto.InvoiceNumberPrefix is not null);
        RuleFor(x => x.Dto.QuoteNumberPrefix).MaximumLength(20).When(x => x.Dto.QuoteNumberPrefix is not null);
        RuleFor(x => x.Dto.PONumberPrefix).MaximumLength(20).When(x => x.Dto.PONumberPrefix is not null);
        RuleFor(x => x.Dto.WorkOrderNumberPrefix).MaximumLength(20).When(x => x.Dto.WorkOrderNumberPrefix is not null);
        RuleFor(x => x.Dto.InvoiceBatchNumberFormat).MaximumLength(200).When(x => x.Dto.InvoiceBatchNumberFormat is not null);
        RuleFor(x => x.Dto.EstimateRevisionCreationMode)
            .MaximumLength(50)
            .Must(v => v is null
                || v.Equals(EstimateRevisionCreationModes.OnDemand, StringComparison.Ordinal)
                || v.Equals(EstimateRevisionCreationModes.OnPostSignatureChange, StringComparison.Ordinal))
            .WithMessage(
                "EstimateRevisionCreationMode must be OnDemand or OnPostSignatureChange.")
            .When(x => x.Dto.EstimateRevisionCreationMode is not null);
    }
}
