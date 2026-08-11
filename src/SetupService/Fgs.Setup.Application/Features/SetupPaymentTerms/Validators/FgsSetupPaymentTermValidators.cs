using Fgs.Setup.Application.Abstractions.SetupPaymentTerms;
using Fgs.Setup.Application.Features.SetupPaymentTerms.Commands.CreateFgsSetupPaymentTerm;
using Fgs.Setup.Application.Features.SetupPaymentTerms.Commands.PatchFgsSetupPaymentTerm;
using Fgs.Setup.Application.Features.SetupPaymentTerms.Commands.UpdateFgsSetupPaymentTerm;
using FluentValidation;

namespace Fgs.Setup.Application.Features.SetupPaymentTerms.Validators;

public sealed class CreateFgsSetupPaymentTermCommandValidator : AbstractValidator<CreateFgsSetupPaymentTermCommand>
{
    public CreateFgsSetupPaymentTermCommandValidator(IFgsSetupPaymentTermReadRepository readRepository)
    {
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByNameAsync(name, null, cancellationToken))
            .WithMessage("An active payment term with this name already exists.");
        RuleFor(x => x.Dto.DueDateMethod).NotEmpty();



        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByNameAsync(name, null, cancellationToken))
            .WithMessage("An active payment term with this name already exists.");
        RuleFor(x => x.Dto.DueDateMethod).NotEmpty();




    }
}

public sealed class UpdateFgsSetupPaymentTermCommandValidator : AbstractValidator<UpdateFgsSetupPaymentTermCommand>
{
    public UpdateFgsSetupPaymentTermCommandValidator(IFgsSetupPaymentTermReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByNameAsync(name, command.Id, cancellationToken))
            .WithMessage("An active payment term with this name already exists.");
        RuleFor(x => x.Dto.DueDateMethod).NotEmpty();




    }
}

public sealed class PatchFgsSetupPaymentTermCommandValidator : AbstractValidator<PatchFgsSetupPaymentTermCommand>
{
    public PatchFgsSetupPaymentTermCommandValidator(IFgsSetupPaymentTermReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.Name).NotEmpty().When(x => x.Dto.Name is not null);
        RuleFor(x => x.Dto.Name).MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByNameAsync(name!, command.Id, cancellationToken))
            .WithMessage("An active payment term with this name already exists.").When(x => x.Dto.Name is not null);
        RuleFor(x => x.Dto.DueDateMethod).NotEmpty().When(x => x.Dto.DueDateMethod is not null);




    }
}
