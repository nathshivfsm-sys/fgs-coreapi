using Fgs.Setup.Application.Abstractions.SetupPaymentMethods;
using Fgs.Setup.Application.Features.SetupPaymentMethods.Commands.CreateFgsSetupPaymentMethod;
using Fgs.Setup.Application.Features.SetupPaymentMethods.Commands.PatchFgsSetupPaymentMethod;
using Fgs.Setup.Application.Features.SetupPaymentMethods.Commands.UpdateFgsSetupPaymentMethod;
using FluentValidation;

namespace Fgs.Setup.Application.Features.SetupPaymentMethods.Validators;

public sealed class CreateFgsSetupPaymentMethodCommandValidator : AbstractValidator<CreateFgsSetupPaymentMethodCommand>
{
    public CreateFgsSetupPaymentMethodCommandValidator(IFgsSetupPaymentMethodReadRepository readRepository)
    {
        RuleFor(x => x.Dto.DisplayName).NotEmpty();
        RuleFor(x => x.Dto.DisplayName).MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByDisplayNameAsync(name, null, cancellationToken))
            .WithMessage("An active payment method with this name already exists.");
        RuleFor(x => x.Dto.SortOrder).GreaterThanOrEqualTo(0);

        RuleFor(x => x.Dto.DisplayName).NotEmpty();
        RuleFor(x => x.Dto.DisplayName).MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByDisplayNameAsync(name, null, cancellationToken))
            .WithMessage("An active payment method with this name already exists.");
        RuleFor(x => x.Dto.SortOrder).GreaterThanOrEqualTo(0);


    }
}

public sealed class UpdateFgsSetupPaymentMethodCommandValidator : AbstractValidator<UpdateFgsSetupPaymentMethodCommand>
{
    public UpdateFgsSetupPaymentMethodCommandValidator(IFgsSetupPaymentMethodReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.DisplayName).NotEmpty();
        RuleFor(x => x.Dto.DisplayName).MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByDisplayNameAsync(name, command.Id, cancellationToken))
            .WithMessage("An active payment method with this name already exists.");
        RuleFor(x => x.Dto.SortOrder).GreaterThanOrEqualTo(0);


    }
}

public sealed class PatchFgsSetupPaymentMethodCommandValidator : AbstractValidator<PatchFgsSetupPaymentMethodCommand>
{
    public PatchFgsSetupPaymentMethodCommandValidator(IFgsSetupPaymentMethodReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.DisplayName).NotEmpty().When(x => x.Dto.DisplayName is not null);
        RuleFor(x => x.Dto.DisplayName).MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByDisplayNameAsync(name!, command.Id, cancellationToken))
            .WithMessage("An active payment method with this name already exists.").When(x => x.Dto.DisplayName is not null);
        RuleFor(x => x.Dto.SortOrder).GreaterThanOrEqualTo(0).When(x => x.Dto.SortOrder.HasValue);


    }
}
