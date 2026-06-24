using Fgs.Setup.Application.Abstractions.FgsBusinessTypes;
using Fgs.Setup.Application.Features.FgsBusinessTypes.Commands.CreateFgsBusinessType;
using Fgs.Setup.Application.Features.FgsBusinessTypes.Commands.PatchFgsBusinessType;
using Fgs.Setup.Application.Features.FgsBusinessTypes.Commands.UpdateFgsBusinessType;
using FluentValidation;

namespace Fgs.Setup.Application.Features.FgsBusinessTypes.Validators;

public sealed class CreateFgsBusinessTypeCommandValidator : AbstractValidator<CreateFgsBusinessTypeCommand>
{
    public CreateFgsBusinessTypeCommandValidator(IFgsBusinessTypeReadRepository readRepository)
    {
        RuleFor(x => x.Dto.Code).NotEmpty();
        RuleFor(x => x.Dto.Code).MaximumLength(100);
        RuleFor(x => x.Dto.Code).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("Code must be uppercase.");
        RuleFor(x => x.Dto.Code).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByCodeAsync(code, null, cancellationToken))
            .WithMessage("A business type with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MaximumLength(200);

        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);        RuleFor(x => x.Dto.Code).NotEmpty();
        RuleFor(x => x.Dto.Code).MaximumLength(100);
        RuleFor(x => x.Dto.Code).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("Code must be uppercase.");
        RuleFor(x => x.Dto.Code).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByCodeAsync(code, null, cancellationToken))
            .WithMessage("A business type with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MaximumLength(200);

        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);
    }
}

public sealed class UpdateFgsBusinessTypeCommandValidator : AbstractValidator<UpdateFgsBusinessTypeCommand>
{
    public UpdateFgsBusinessTypeCommandValidator(IFgsBusinessTypeReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.Code).NotEmpty();
        RuleFor(x => x.Dto.Code).MaximumLength(100);
        RuleFor(x => x.Dto.Code).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("Code must be uppercase.");
        RuleFor(x => x.Dto.Code).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByCodeAsync(code, command.Id, cancellationToken))
            .WithMessage("A business type with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MaximumLength(200);

        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);
    }
}

public sealed class PatchFgsBusinessTypeCommandValidator : AbstractValidator<PatchFgsBusinessTypeCommand>
{
    public PatchFgsBusinessTypeCommandValidator(IFgsBusinessTypeReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.Code).NotEmpty();
        RuleFor(x => x.Dto.Code).MaximumLength(100);
        RuleFor(x => x.Dto.Code).Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("Code must be uppercase.");
        RuleFor(x => x.Dto.Code).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByCodeAsync(code!, command.Id, cancellationToken))
            .WithMessage("A business type with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MaximumLength(200);

        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)0).When(x => x.Dto.DisplayOrder.HasValue);
    }
}
