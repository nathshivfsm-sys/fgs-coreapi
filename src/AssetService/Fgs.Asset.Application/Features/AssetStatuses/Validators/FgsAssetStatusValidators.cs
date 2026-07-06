using Fgs.Asset.Application.Abstractions.AssetStatuses;
using Fgs.Asset.Application.Features.AssetStatuses.Commands.CreateFgsAssetStatus;
using Fgs.Asset.Application.Features.AssetStatuses.Commands.PatchFgsAssetStatus;
using Fgs.Asset.Application.Features.AssetStatuses.Commands.UpdateFgsAssetStatus;
using FluentValidation;

namespace Fgs.Asset.Application.Features.AssetStatuses.Validators;

public sealed class CreateFgsAssetStatusCommandValidator : AbstractValidator<CreateFgsAssetStatusCommand>
{
    public CreateFgsAssetStatusCommandValidator(IFgsAssetStatusReadRepository readRepository)
    {
        RuleFor(x => x.Dto.Code).NotEmpty().MaximumLength(75);
        RuleFor(x => x.Dto.Code).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("Code must be uppercase.");
        RuleFor(x => x.Dto.Code).MustAsync(async (command, code, ct) => !await readRepository.ExistsByCodeAsync(code, null, ct)).WithMessage("A asset status with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Dto.Description).MaximumLength(4000);
    }
}

public sealed class UpdateFgsAssetStatusCommandValidator : AbstractValidator<UpdateFgsAssetStatusCommand>
{
    public UpdateFgsAssetStatusCommandValidator(IFgsAssetStatusReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.Code).NotEmpty().MaximumLength(75);
        RuleFor(x => x.Dto.Code).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("Code must be uppercase.");
        RuleFor(x => x.Dto.Code).MustAsync(async (command, code, ct) => !await readRepository.ExistsByCodeAsync(code, command.Id, ct)).WithMessage("A asset status with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Dto.Description).MaximumLength(4000);
    }
}

public sealed class PatchFgsAssetStatusCommandValidator : AbstractValidator<PatchFgsAssetStatusCommand>
{
    public PatchFgsAssetStatusCommandValidator(IFgsAssetStatusReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.Code).NotEmpty().MaximumLength(75).When(x => x.Dto.Code is not null);
        RuleFor(x => x.Dto.Code).Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("Code must be uppercase.").When(x => x.Dto.Code is not null);
        RuleFor(x => x.Dto.Code).MustAsync(async (command, code, ct) => !await readRepository.ExistsByCodeAsync(code!, command.Id, ct)).WithMessage("A asset status with this code already exists.").When(x => x.Dto.Code is not null);
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200).When(x => x.Dto.Name is not null);
        RuleFor(x => x.Dto.Description).MaximumLength(4000).When(x => x.Dto.Description is not null);
    }
}
