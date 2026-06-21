using Fgs.Setup.Application.Abstractions.ResolutionCodes;
using Fgs.Setup.Application.Features.ResolutionCodes.Commands.CreateResolutionCode;
using Fgs.Setup.Application.Features.ResolutionCodes.Commands.PatchResolutionCode;
using Fgs.Setup.Application.Features.ResolutionCodes.Commands.UpdateResolutionCode;
using FluentValidation;

namespace Fgs.Setup.Application.Features.ResolutionCodes.Validators;

public sealed class CreateResolutionCodeCommandValidator : AbstractValidator<CreateResolutionCodeCommand>
{
    public CreateResolutionCodeCommandValidator(IResolutionCodeReadRepository readRepository)
    {
        RuleFor(x => x.Dto.GloResolutionTypeId).MustAsync(async (command, value, cancellationToken) =>
                await readRepository.ExistsGloResolutionTypeIdAsync(value, cancellationToken))
            .WithMessage("The specified resolution type was not found.");
        RuleFor(x => x.Dto.ResolutionCode).NotEmpty();
        RuleFor(x => x.Dto.ResolutionCode).MaximumLength(50);
        RuleFor(x => x.Dto.ResolutionCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("ResolutionCode must be uppercase.");
        RuleFor(x => x.Dto.ResolutionCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByResolutionCodeAsync(code, null, cancellationToken))
            .WithMessage("A resolution code with this code already exists.");
        RuleFor(x => x.Dto.ResolutionName).NotEmpty();
        RuleFor(x => x.Dto.ResolutionName).MaximumLength(200);

    }
}

public sealed class UpdateResolutionCodeCommandValidator : AbstractValidator<UpdateResolutionCodeCommand>
{
    public UpdateResolutionCodeCommandValidator(IResolutionCodeReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.GloResolutionTypeId).MustAsync(async (command, value, cancellationToken) =>
                await readRepository.ExistsGloResolutionTypeIdAsync(value, cancellationToken))
            .WithMessage("The specified resolution type was not found.");
        RuleFor(x => x.Dto.ResolutionCode).NotEmpty();
        RuleFor(x => x.Dto.ResolutionCode).MaximumLength(50);
        RuleFor(x => x.Dto.ResolutionCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("ResolutionCode must be uppercase.");
        RuleFor(x => x.Dto.ResolutionCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByResolutionCodeAsync(code, command.Id, cancellationToken))
            .WithMessage("A resolution code with this code already exists.");
        RuleFor(x => x.Dto.ResolutionName).NotEmpty();
        RuleFor(x => x.Dto.ResolutionName).MaximumLength(200);

    }
}

public sealed class PatchResolutionCodeCommandValidator : AbstractValidator<PatchResolutionCodeCommand>
{
    public PatchResolutionCodeCommandValidator(IResolutionCodeReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.GloResolutionTypeId).MustAsync(async (command, value, cancellationToken) =>
                !value.HasValue || await readRepository.ExistsGloResolutionTypeIdAsync(value.Value, cancellationToken))
            .WithMessage("The specified resolution type was not found.").When(x => x.Dto.GloResolutionTypeId.HasValue);
        RuleFor(x => x.Dto.ResolutionCode).NotEmpty();
        RuleFor(x => x.Dto.ResolutionCode).MaximumLength(50);
        RuleFor(x => x.Dto.ResolutionCode).Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("ResolutionCode must be uppercase.");
        RuleFor(x => x.Dto.ResolutionCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByResolutionCodeAsync(code!, command.Id, cancellationToken))
            .WithMessage("A resolution code with this code already exists.");
        RuleFor(x => x.Dto.ResolutionName).NotEmpty();
        RuleFor(x => x.Dto.ResolutionName).MaximumLength(200);

    }
}
