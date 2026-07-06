using Fgs.Asset.Application.Abstractions.AssetModels;
using Fgs.Asset.Application.Features.AssetModels.Commands.CreateFgsAssetModel;
using Fgs.Asset.Application.Features.AssetModels.Commands.PatchFgsAssetModel;
using Fgs.Asset.Application.Features.AssetModels.Commands.UpdateFgsAssetModel;
using FluentValidation;
namespace Fgs.Asset.Application.Features.AssetModels.Validators;
public sealed class CreateFgsAssetModelCommandValidator : AbstractValidator<CreateFgsAssetModelCommand>
{
    public CreateFgsAssetModelCommandValidator(IFgsAssetModelReadRepository readRepository)
    {
        RuleFor(x => x.Dto.AssetTypeId).GreaterThan(0);
        RuleFor(x => x.Dto.AssetManufacturerId).GreaterThan(0);
        RuleFor(x => x.Dto.ModelNumber).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Dto.ModelDescription).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Dto.AssetTypeId).MustAsync(async (c, v, ct) => await readRepository.ExistsAssetTypeIdAsync(v, ct)).WithMessage("The specified asset type was not found.");
        RuleFor(x => x.Dto.AssetManufacturerId).MustAsync(async (c, v, ct) => await readRepository.ExistsAssetManufacturerIdAsync(v, ct)).WithMessage("The specified asset manufacturer was not found.");
    }
}
public sealed class UpdateFgsAssetModelCommandValidator : AbstractValidator<UpdateFgsAssetModelCommand>
{
    public UpdateFgsAssetModelCommandValidator(IFgsAssetModelReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.AssetTypeId).GreaterThan(0);
        RuleFor(x => x.Dto.AssetManufacturerId).GreaterThan(0);
        RuleFor(x => x.Dto.ModelNumber).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Dto.ModelDescription).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Dto.AssetTypeId).MustAsync(async (c, v, ct) => await readRepository.ExistsAssetTypeIdAsync(v, ct)).WithMessage("The specified asset type was not found.");
        RuleFor(x => x.Dto.AssetManufacturerId).MustAsync(async (c, v, ct) => await readRepository.ExistsAssetManufacturerIdAsync(v, ct)).WithMessage("The specified asset manufacturer was not found.");
    }
}
public sealed class PatchFgsAssetModelCommandValidator : AbstractValidator<PatchFgsAssetModelCommand>
{
    public PatchFgsAssetModelCommandValidator(IFgsAssetModelReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.AssetTypeId).GreaterThan(0).When(x => x.Dto.AssetTypeId.HasValue);
        RuleFor(x => x.Dto.AssetManufacturerId).GreaterThan(0).When(x => x.Dto.AssetManufacturerId.HasValue);
        RuleFor(x => x.Dto.ModelNumber).NotEmpty().MaximumLength(100).When(x => x.Dto.ModelNumber is not null);
        RuleFor(x => x.Dto.ModelDescription).NotEmpty().MaximumLength(500).When(x => x.Dto.ModelDescription is not null);
        RuleFor(x => x.Dto.AssetTypeId).MustAsync(async (c, v, ct) => !v.HasValue || await readRepository.ExistsAssetTypeIdAsync(v.Value, ct)).When(x => x.Dto.AssetTypeId.HasValue);
        RuleFor(x => x.Dto.AssetManufacturerId).MustAsync(async (c, v, ct) => !v.HasValue || await readRepository.ExistsAssetManufacturerIdAsync(v.Value, ct)).When(x => x.Dto.AssetManufacturerId.HasValue);
    }
}
