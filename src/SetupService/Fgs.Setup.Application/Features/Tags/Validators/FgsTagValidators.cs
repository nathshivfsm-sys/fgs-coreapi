using Fgs.Setup.Application.Abstractions.Tags;
using Fgs.Setup.Application.Features.Tags.Commands.CreateFgsTag;
using Fgs.Setup.Application.Features.Tags.Commands.PatchFgsTag;
using Fgs.Setup.Application.Features.Tags.Commands.UpdateFgsTag;
using FluentValidation;

namespace Fgs.Setup.Application.Features.Tags.Validators;

public sealed class CreateFgsTagCommandValidator : AbstractValidator<CreateFgsTagCommand>
{
    public CreateFgsTagCommandValidator(IFgsTagReadRepository readRepository)
    {
        RuleFor(x => x.Dto.TagCode).MaximumLength(50);
        RuleFor(x => x.Dto.TagCode).Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("TagCode must be uppercase.").When(x => x.Dto.TagCode is not null);
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MaximumLength(100);

        RuleFor(x => x.Dto.BackgroundColor).MaximumLength(20);
        RuleFor(x => x.Dto.TextColor).MaximumLength(20);
        RuleFor(x => x.Dto.TagCode).MaximumLength(50);
        RuleFor(x => x.Dto.TagCode).Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("TagCode must be uppercase.").When(x => x.Dto.TagCode is not null);
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MaximumLength(100);

        RuleFor(x => x.Dto.BackgroundColor).MaximumLength(20);
        RuleFor(x => x.Dto.TextColor).MaximumLength(20);

    }
}

public sealed class UpdateFgsTagCommandValidator : AbstractValidator<UpdateFgsTagCommand>
{
    public UpdateFgsTagCommandValidator(IFgsTagReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.TagCode).MaximumLength(50);
        RuleFor(x => x.Dto.TagCode).Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("TagCode must be uppercase.").When(x => x.Dto.TagCode is not null);
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MaximumLength(100);

        RuleFor(x => x.Dto.BackgroundColor).MaximumLength(20);
        RuleFor(x => x.Dto.TextColor).MaximumLength(20);

    }
}

public sealed class PatchFgsTagCommandValidator : AbstractValidator<PatchFgsTagCommand>
{
    public PatchFgsTagCommandValidator(IFgsTagReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.TagCode).MaximumLength(50).When(x => x.Dto.TagCode is not null);
        RuleFor(x => x.Dto.TagCode).Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("TagCode must be uppercase.").When(x => x.Dto.TagCode is not null);
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MaximumLength(100);

        RuleFor(x => x.Dto.BackgroundColor).MaximumLength(20).When(x => x.Dto.BackgroundColor is not null);
        RuleFor(x => x.Dto.TextColor).MaximumLength(20).When(x => x.Dto.TextColor is not null);

    }
}
