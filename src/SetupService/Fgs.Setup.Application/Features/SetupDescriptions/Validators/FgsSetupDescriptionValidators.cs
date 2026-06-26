using Fgs.Setup.Application.Abstractions.SetupDescriptions;
using Fgs.Setup.Application.Features.SetupDescriptions.Commands.CreateFgsSetupDescription;
using Fgs.Setup.Application.Features.SetupDescriptions.Commands.PatchFgsSetupDescription;
using Fgs.Setup.Application.Features.SetupDescriptions.Commands.UpdateFgsSetupDescription;
using FluentValidation;

namespace Fgs.Setup.Application.Features.SetupDescriptions.Validators;

public sealed class CreateFgsSetupDescriptionCommandValidator : AbstractValidator<CreateFgsSetupDescriptionCommand>
{
    public CreateFgsSetupDescriptionCommandValidator(IFgsSetupDescriptionReadRepository readRepository)
    {
        RuleFor(x => x.Dto.DescriptionTypeCode).NotEmpty();
        RuleFor(x => x.Dto.DescriptionTypeCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByDescriptionTypeCodeAsync(code, null, cancellationToken))
            .WithMessage("A setup description with this code already exists.");
        RuleFor(x => x.Dto.ShortNote).MaximumLength(30);
        RuleFor(x => x.Dto.Body).NotEmpty();
        RuleFor(x => x.Dto.FgsSetupTechTradeId).MustAsync(async (command, value, cancellationToken) =>
                !value.HasValue || await readRepository.ExistsTechTradeIdAsync(value.Value, cancellationToken))
            .WithMessage("The specified tech trade was not found.");
        RuleFor(x => x.Dto.SortOrder).GreaterThanOrEqualTo(0); RuleFor(x => x.Dto.DescriptionTypeCode).NotEmpty();
        RuleFor(x => x.Dto.DescriptionTypeCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByDescriptionTypeCodeAsync(code, null, cancellationToken))
            .WithMessage("A setup description with this code already exists.");
        RuleFor(x => x.Dto.ShortNote).MaximumLength(30);
        RuleFor(x => x.Dto.Body).NotEmpty();
        RuleFor(x => x.Dto.FgsSetupTechTradeId).MustAsync(async (command, value, cancellationToken) =>
                !value.HasValue || await readRepository.ExistsTechTradeIdAsync(value.Value, cancellationToken))
            .WithMessage("The specified tech trade was not found.");
        RuleFor(x => x.Dto.SortOrder).GreaterThanOrEqualTo(0);
    }
}

public sealed class UpdateFgsSetupDescriptionCommandValidator : AbstractValidator<UpdateFgsSetupDescriptionCommand>
{
    public UpdateFgsSetupDescriptionCommandValidator(IFgsSetupDescriptionReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.DescriptionTypeCode).NotEmpty();
        RuleFor(x => x.Dto.DescriptionTypeCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByDescriptionTypeCodeAsync(code, command.Id, cancellationToken))
            .WithMessage("A setup description with this code already exists.");
        RuleFor(x => x.Dto.ShortNote).MaximumLength(30);
        RuleFor(x => x.Dto.Body).NotEmpty();
        RuleFor(x => x.Dto.FgsSetupTechTradeId).MustAsync(async (command, value, cancellationToken) =>
                !value.HasValue || await readRepository.ExistsTechTradeIdAsync(value.Value, cancellationToken))
            .WithMessage("The specified tech trade was not found.");
        RuleFor(x => x.Dto.SortOrder).GreaterThanOrEqualTo(0);
    }
}

public sealed class PatchFgsSetupDescriptionCommandValidator : AbstractValidator<PatchFgsSetupDescriptionCommand>
{
    public PatchFgsSetupDescriptionCommandValidator(IFgsSetupDescriptionReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.DescriptionTypeCode).NotEmpty();
        RuleFor(x => x.Dto.DescriptionTypeCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByDescriptionTypeCodeAsync(code!, command.Id, cancellationToken))
            .WithMessage("A setup description with this code already exists.");
        RuleFor(x => x.Dto.ShortNote).MaximumLength(30).When(x => x.Dto.ShortNote is not null);
        RuleFor(x => x.Dto.Body).NotEmpty();
        RuleFor(x => x.Dto.FgsSetupTechTradeId).MustAsync(async (command, value, cancellationToken) =>
                !value.HasValue || await readRepository.ExistsTechTradeIdAsync(value.Value, cancellationToken))
            .WithMessage("The specified tech trade was not found.").When(x => x.Dto.FgsSetupTechTradeId.HasValue);
        RuleFor(x => x.Dto.SortOrder).GreaterThanOrEqualTo(0).When(x => x.Dto.SortOrder.HasValue);
    }
}
