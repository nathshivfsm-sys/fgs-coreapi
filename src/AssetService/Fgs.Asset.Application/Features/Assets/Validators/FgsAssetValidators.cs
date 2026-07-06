using Fgs.Asset.Application.Abstractions.Assets;
using Fgs.Asset.Application.Features.Assets.Commands.CreateFgsAsset;
using Fgs.Asset.Application.Features.Assets.Commands.PatchFgsAsset;
using Fgs.Asset.Application.Features.Assets.Commands.UpdateFgsAsset;
using FluentValidation;
namespace Fgs.Asset.Application.Features.Assets.Validators;
public sealed class CreateFgsAssetCommandValidator : AbstractValidator<CreateFgsAssetCommand>
{
    public CreateFgsAssetCommandValidator(IFgsAssetReadRepository readRepository)
    {
        RuleFor(x => x.Dto.AssetNumber).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Dto.AssetNumber).MustAsync(async (cmd, n, ct) => !await readRepository.ExistsByAssetNumberAsync(n, null, ct));
        RuleFor(x => x.Dto.ServiceLocationId).GreaterThan(0);
        RuleFor(x => x.Dto.AssetStatusId).GreaterThan(0);
        RuleFor(x => x.Dto.ServiceLocationId).MustAsync(async (cmd, id, ct) => await readRepository.ExistsServiceLocationIdAsync(id, ct));
        RuleFor(x => x.Dto.AssetStatusId).MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetStatusIdAsync(id, ct));
        RuleFor(x => x.Dto.AssetTypeId).MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetTypeIdAsync(id, ct));
        RuleFor(x => x.Dto.AssetManufacturerId).MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetManufacturerIdAsync(id, ct));
        RuleFor(x => x.Dto.AssetModelId).MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetModelIdAsync(id, ct));
    }
}
public sealed class UpdateFgsAssetCommandValidator : AbstractValidator<UpdateFgsAssetCommand>
{
    public UpdateFgsAssetCommandValidator(IFgsAssetReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.AssetNumber).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Dto.AssetNumber).MustAsync(async (cmd, n, ct) => !await readRepository.ExistsByAssetNumberAsync(n, cmd.Id, ct));
        RuleFor(x => x.Dto.ServiceLocationId).GreaterThan(0);
        RuleFor(x => x.Dto.AssetStatusId).GreaterThan(0);
        RuleFor(x => x.Dto.ServiceLocationId).MustAsync(async (cmd, id, ct) => await readRepository.ExistsServiceLocationIdAsync(id, ct));
        RuleFor(x => x.Dto.AssetStatusId).MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetStatusIdAsync(id, ct));
        RuleFor(x => x.Dto.AssetTypeId).MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetTypeIdAsync(id, ct));
        RuleFor(x => x.Dto.AssetManufacturerId).MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetManufacturerIdAsync(id, ct));
        RuleFor(x => x.Dto.AssetModelId).MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetModelIdAsync(id, ct));
    }
}
public sealed class PatchFgsAssetCommandValidator : AbstractValidator<PatchFgsAssetCommand>
{
    public PatchFgsAssetCommandValidator(IFgsAssetReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.AssetNumber).NotEmpty().MaximumLength(100).When(x => x.Dto.AssetNumber is not null);
        RuleFor(x => x.Dto.ServiceLocationId).GreaterThan(0).When(x => x.Dto.ServiceLocationId.HasValue);
        RuleFor(x => x.Dto.AssetStatusId).GreaterThan(0).When(x => x.Dto.AssetStatusId.HasValue);
    }
}
