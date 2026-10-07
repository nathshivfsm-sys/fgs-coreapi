using Fgs.Setup.Application.Abstractions.Sources;
using Fgs.Setup.Application.Features.Sources.Commands.CreateSource;
using Fgs.Setup.Application.Features.Sources.Commands.PatchSource;
using Fgs.Setup.Application.Features.Sources.Commands.UpdateSource;
using FluentValidation;

namespace Fgs.Setup.Application.Features.Sources.Validators;

public sealed class CreateSourceCommandValidator : AbstractValidator<CreateSourceCommand>
{
    public CreateSourceCommandValidator(ISourceReadRepository readRepository)
    {
        RuleFor(x => x.Dto.SourceCode).NotEmpty();
        RuleFor(x => x.Dto.SourceCode).MaximumLength(50);
        RuleFor(x => x.Dto.SourceCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("SourceCode must be uppercase.");
        RuleFor(x => x.Dto.SourceCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsBySourceCodeAsync(code, null, cancellationToken))
            .WithMessage("A source with this code already exists.");
        RuleFor(x => x.Dto.SourceName).NotEmpty();
        RuleFor(x => x.Dto.SourceName).MaximumLength(100);
        RuleFor(x => x.Dto.Description).MaximumLength(255); RuleFor(x => x.Dto.SourceCode).NotEmpty();
        RuleFor(x => x.Dto.SourceCode).MaximumLength(50);
        RuleFor(x => x.Dto.SourceCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("SourceCode must be uppercase.");
        RuleFor(x => x.Dto.SourceCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsBySourceCodeAsync(code, null, cancellationToken))
            .WithMessage("A source with this code already exists.");
        RuleFor(x => x.Dto.SourceName).NotEmpty();
        RuleFor(x => x.Dto.SourceName).MaximumLength(100);
        RuleFor(x => x.Dto.Description).MaximumLength(255);
    }
}

public sealed class UpdateSourceCommandValidator : AbstractValidator<UpdateSourceCommand>
{
    public UpdateSourceCommandValidator(ISourceReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.SourceCode).NotEmpty();
        RuleFor(x => x.Dto.SourceCode).MaximumLength(50);
        RuleFor(x => x.Dto.SourceCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("SourceCode must be uppercase.");
        RuleFor(x => x.Dto.SourceCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsBySourceCodeAsync(code, command.Id, cancellationToken))
            .WithMessage("A source with this code already exists.");
        RuleFor(x => x.Dto.SourceName).NotEmpty();
        RuleFor(x => x.Dto.SourceName).MaximumLength(100);
        RuleFor(x => x.Dto.Description).MaximumLength(255);
    }
}

public sealed class PatchSourceCommandValidator : AbstractValidator<PatchSourceCommand>
{
    public PatchSourceCommandValidator(ISourceReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.SourceCode).NotEmpty().When(x => x.Dto.SourceCode is not null);
        RuleFor(x => x.Dto.SourceCode).MaximumLength(50).When(x => x.Dto.SourceCode is not null);
        RuleFor(x => x.Dto.SourceCode).Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("SourceCode must be uppercase.").When(x => x.Dto.SourceCode is not null);
        RuleFor(x => x.Dto.SourceCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsBySourceCodeAsync(code!, command.Id, cancellationToken))
            .WithMessage("A source with this code already exists.").When(x => x.Dto.SourceCode is not null);
        RuleFor(x => x.Dto.SourceName).NotEmpty().When(x => x.Dto.SourceName is not null);
        RuleFor(x => x.Dto.SourceName).MaximumLength(100).When(x => x.Dto.SourceName is not null);
        RuleFor(x => x.Dto.Description).MaximumLength(255).When(x => x.Dto.Description is not null);
    }
}
