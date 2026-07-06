using Fgs.Asset.Application.Abstractions.AssetManufacturers;
using Fgs.Asset.Application.Features.AssetManufacturers.Commands.CreateFgsAssetManufacturer;
using Fgs.Asset.Application.Features.AssetManufacturers.Commands.PatchFgsAssetManufacturer;
using Fgs.Asset.Application.Features.AssetManufacturers.Commands.UpdateFgsAssetManufacturer;
using FluentValidation;

namespace Fgs.Asset.Application.Features.AssetManufacturers.Validators;

public sealed class CreateFgsAssetManufacturerCommandValidator : AbstractValidator<CreateFgsAssetManufacturerCommand>
{
    public CreateFgsAssetManufacturerCommandValidator(IFgsAssetManufacturerReadRepository readRepository)
    {
        RuleFor(x => x.Dto.Code).NotEmpty().MaximumLength(75);
        RuleFor(x => x.Dto.Code).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("Code must be uppercase.");
        RuleFor(x => x.Dto.Code).MustAsync(async (command, code, ct) => !await readRepository.ExistsByCodeAsync(code, null, ct)).WithMessage("A asset manufacturer with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Dto.Description).MaximumLength(4000);
    }
}

public sealed class UpdateFgsAssetManufacturerCommandValidator : AbstractValidator<UpdateFgsAssetManufacturerCommand>
{
    public UpdateFgsAssetManufacturerCommandValidator(IFgsAssetManufacturerReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.Code).NotEmpty().MaximumLength(75);
        RuleFor(x => x.Dto.Code).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("Code must be uppercase.");
        RuleFor(x => x.Dto.Code).MustAsync(async (command, code, ct) => !await readRepository.ExistsByCodeAsync(code, command.Id, ct)).WithMessage("A asset manufacturer with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Dto.Description).MaximumLength(4000);
    }
}

public sealed class PatchFgsAssetManufacturerCommandValidator : AbstractValidator<PatchFgsAssetManufacturerCommand>
{
    public PatchFgsAssetManufacturerCommandValidator(IFgsAssetManufacturerReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.Code).NotEmpty().MaximumLength(75).When(x => x.Dto.Code is not null);
        RuleFor(x => x.Dto.Code).Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("Code must be uppercase.").When(x => x.Dto.Code is not null);
        RuleFor(x => x.Dto.Code).MustAsync(async (command, code, ct) => !await readRepository.ExistsByCodeAsync(code!, command.Id, ct)).WithMessage("A asset manufacturer with this code already exists.").When(x => x.Dto.Code is not null);
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200).When(x => x.Dto.Name is not null);
        RuleFor(x => x.Dto.Description).MaximumLength(4000).When(x => x.Dto.Description is not null);
    }
}
