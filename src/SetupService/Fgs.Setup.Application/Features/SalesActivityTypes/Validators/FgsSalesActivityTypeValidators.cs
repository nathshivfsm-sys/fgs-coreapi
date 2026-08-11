using Fgs.Setup.Application.Abstractions.SalesActivityTypes;
using Fgs.Setup.Application.Features.SalesActivityTypes.Commands.CreateFgsSalesActivityType;
using Fgs.Setup.Application.Features.SalesActivityTypes.Commands.PatchFgsSalesActivityType;
using Fgs.Setup.Application.Features.SalesActivityTypes.Commands.UpdateFgsSalesActivityType;
using FluentValidation;

namespace Fgs.Setup.Application.Features.SalesActivityTypes.Validators;

public sealed class CreateFgsSalesActivityTypeCommandValidator : AbstractValidator<CreateFgsSalesActivityTypeCommand>
{
    public CreateFgsSalesActivityTypeCommandValidator(IFgsSalesActivityTypeReadRepository readRepository)
    {
        RuleFor(x => x.Dto.ActivityTypeCode).NotEmpty();
        RuleFor(x => x.Dto.ActivityTypeCode).MaximumLength(50);
        RuleFor(x => x.Dto.ActivityTypeCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("ActivityTypeCode must be uppercase.");
        RuleFor(x => x.Dto.ActivityTypeCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByActivityTypeCodeAsync(code, null, cancellationToken))
            .WithMessage("A sales activity type with this code already exists.");
        RuleFor(x => x.Dto.ActivityTypeName).NotEmpty();
        RuleFor(x => x.Dto.ActivityTypeName).MaximumLength(100);
        RuleFor(x => x.Dto.ActivityTypeName).MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByActivityTypeNameAsync(name, null, cancellationToken))
            .WithMessage("An active sales activity type with this name already exists.");
        RuleFor(x => x.Dto.Description).MaximumLength(255);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0);




        RuleFor(x => x.Dto).Must(dto => dto.AppliesToLead || dto.AppliesToOpportunity)
            .WithMessage("At least one of AppliesToLead or AppliesToOpportunity must be true.");
    }
}

public sealed class UpdateFgsSalesActivityTypeCommandValidator : AbstractValidator<UpdateFgsSalesActivityTypeCommand>
{
    public UpdateFgsSalesActivityTypeCommandValidator(IFgsSalesActivityTypeReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.ActivityTypeCode).NotEmpty();
        RuleFor(x => x.Dto.ActivityTypeCode).MaximumLength(50);
        RuleFor(x => x.Dto.ActivityTypeCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("ActivityTypeCode must be uppercase.");
        RuleFor(x => x.Dto.ActivityTypeCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByActivityTypeCodeAsync(code, command.Id, cancellationToken))
            .WithMessage("A sales activity type with this code already exists.");
        RuleFor(x => x.Dto.ActivityTypeName).NotEmpty();
        RuleFor(x => x.Dto.ActivityTypeName).MaximumLength(100);
        RuleFor(x => x.Dto.ActivityTypeName).MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByActivityTypeNameAsync(name, command.Id, cancellationToken))
            .WithMessage("An active sales activity type with this name already exists.");
        RuleFor(x => x.Dto.Description).MaximumLength(255);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0);




        RuleFor(x => x.Dto).Must(dto => dto.AppliesToLead || dto.AppliesToOpportunity)
            .WithMessage("At least one of AppliesToLead or AppliesToOpportunity must be true.");
    }
}

public sealed class PatchFgsSalesActivityTypeCommandValidator : AbstractValidator<PatchFgsSalesActivityTypeCommand>
{
    public PatchFgsSalesActivityTypeCommandValidator(IFgsSalesActivityTypeReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.ActivityTypeCode).NotEmpty().When(x => x.Dto.ActivityTypeCode is not null);
        RuleFor(x => x.Dto.ActivityTypeCode).MaximumLength(50).When(x => x.Dto.ActivityTypeCode is not null);
        RuleFor(x => x.Dto.ActivityTypeCode).Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("ActivityTypeCode must be uppercase.").When(x => x.Dto.ActivityTypeCode is not null);
        RuleFor(x => x.Dto.ActivityTypeCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByActivityTypeCodeAsync(code!, command.Id, cancellationToken))
            .WithMessage("A sales activity type with this code already exists.").When(x => x.Dto.ActivityTypeCode is not null);
        RuleFor(x => x.Dto.ActivityTypeName).NotEmpty().When(x => x.Dto.ActivityTypeName is not null);
        RuleFor(x => x.Dto.ActivityTypeName).MaximumLength(100).When(x => x.Dto.ActivityTypeName is not null);
        RuleFor(x => x.Dto.ActivityTypeName).MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByActivityTypeNameAsync(name!, command.Id, cancellationToken))
            .WithMessage("An active sales activity type with this name already exists.").When(x => x.Dto.ActivityTypeName is not null);
        RuleFor(x => x.Dto.Description).MaximumLength(255).When(x => x.Dto.Description is not null);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);




        RuleFor(x => x.Dto).Must(dto =>
                (!dto.AppliesToLead.HasValue && !dto.AppliesToOpportunity.HasValue)
                || dto.AppliesToLead == true
                || dto.AppliesToOpportunity == true)
            .WithMessage("At least one of AppliesToLead or AppliesToOpportunity must be true.");
    }
}
