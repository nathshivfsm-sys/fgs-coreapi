using Fgs.Setup.Application.Abstractions.SetupLaborRateTypes;
using Fgs.Setup.Application.Features.SetupLaborRateTypes.Commands.CreateFgsSetupLaborRateType;
using Fgs.Setup.Application.Features.SetupLaborRateTypes.Commands.PatchFgsSetupLaborRateType;
using Fgs.Setup.Application.Features.SetupLaborRateTypes.Commands.UpdateFgsSetupLaborRateType;
using FluentValidation;

namespace Fgs.Setup.Application.Features.SetupLaborRateTypes.Validators;

public sealed class CreateFgsSetupLaborRateTypeCommandValidator : AbstractValidator<CreateFgsSetupLaborRateTypeCommand>
{
    public CreateFgsSetupLaborRateTypeCommandValidator(IFgsSetupLaborRateTypeReadRepository readRepository)
    {
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByNameAsync(name, null, cancellationToken))
            .WithMessage("An active labor rate type with this name already exists.");

        RuleFor(x => x.Dto.SortOrder).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByNameAsync(name, null, cancellationToken))
            .WithMessage("An active labor rate type with this name already exists.");

        RuleFor(x => x.Dto.SortOrder).GreaterThanOrEqualTo(0);

    }
}

public sealed class UpdateFgsSetupLaborRateTypeCommandValidator : AbstractValidator<UpdateFgsSetupLaborRateTypeCommand>
{
    public UpdateFgsSetupLaborRateTypeCommandValidator(IFgsSetupLaborRateTypeReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByNameAsync(name, command.Id, cancellationToken))
            .WithMessage("An active labor rate type with this name already exists.");

        RuleFor(x => x.Dto.SortOrder).GreaterThanOrEqualTo(0);

    }
}

public sealed class PatchFgsSetupLaborRateTypeCommandValidator : AbstractValidator<PatchFgsSetupLaborRateTypeCommand>
{
    public PatchFgsSetupLaborRateTypeCommandValidator(IFgsSetupLaborRateTypeReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsByNameAsync(name!, command.Id, cancellationToken))
            .WithMessage("An active labor rate type with this name already exists.");

        RuleFor(x => x.Dto.SortOrder).GreaterThanOrEqualTo(0).When(x => x.Dto.SortOrder.HasValue);

    }
}
