using Fgs.Setup.Application.Abstractions.LeadSources;
using Fgs.Setup.Application.Features.LeadSources.Commands.CreateLeadSource;
using Fgs.Setup.Application.Features.LeadSources.Commands.PatchLeadSource;
using Fgs.Setup.Application.Features.LeadSources.Commands.UpdateLeadSource;
using FluentValidation;

namespace Fgs.Setup.Application.Features.LeadSources.Validators;

public sealed class CreateLeadSourceCommandValidator : AbstractValidator<CreateLeadSourceCommand>
{
    public CreateLeadSourceCommandValidator(ILeadSourceReadRepository readRepository)
    {
        RuleFor(x => x.Dto.SourceCode).NotEmpty();
        RuleFor(x => x.Dto.SourceCode).MaximumLength(50);
        RuleFor(x => x.Dto.SourceCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("SourceCode must be uppercase.");
        RuleFor(x => x.Dto.SourceCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsBySourceCodeAsync(code, null, cancellationToken))
            .WithMessage("A lead source with this code already exists.");
        RuleFor(x => x.Dto.SourceName).NotEmpty();
        RuleFor(x => x.Dto.SourceName).MaximumLength(100);
        RuleFor(x => x.Dto.Description).MaximumLength(255);
    }
}

public sealed class UpdateLeadSourceCommandValidator : AbstractValidator<UpdateLeadSourceCommand>
{
    public UpdateLeadSourceCommandValidator(ILeadSourceReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.SourceCode).NotEmpty();
        RuleFor(x => x.Dto.SourceCode).MaximumLength(50);
        RuleFor(x => x.Dto.SourceCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("SourceCode must be uppercase.");
        RuleFor(x => x.Dto.SourceCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsBySourceCodeAsync(code, command.Id, cancellationToken))
            .WithMessage("A lead source with this code already exists.");
        RuleFor(x => x.Dto.SourceName).NotEmpty();
        RuleFor(x => x.Dto.SourceName).MaximumLength(100);
        RuleFor(x => x.Dto.Description).MaximumLength(255);
    }
}

public sealed class PatchLeadSourceCommandValidator : AbstractValidator<PatchLeadSourceCommand>
{
    public PatchLeadSourceCommandValidator(ILeadSourceReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.SourceCode).NotEmpty();
        RuleFor(x => x.Dto.SourceCode).MaximumLength(50);
        RuleFor(x => x.Dto.SourceCode).Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("SourceCode must be uppercase.");
        RuleFor(x => x.Dto.SourceCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsBySourceCodeAsync(code!, command.Id, cancellationToken))
            .WithMessage("A lead source with this code already exists.");
        RuleFor(x => x.Dto.SourceName).NotEmpty();
        RuleFor(x => x.Dto.SourceName).MaximumLength(100);
        RuleFor(x => x.Dto.Description).MaximumLength(255).When(x => x.Dto.Description is not null);
    }
}
