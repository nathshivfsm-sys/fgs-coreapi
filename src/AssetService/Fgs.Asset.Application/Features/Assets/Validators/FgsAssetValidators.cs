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
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage("Request body is required. Ensure the JSON is valid (unresolved Postman variables like {{assetStatusId}} produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.AssetNumber)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MaximumLength(100)
                .MustAsync(async (cmd, value, ct) =>
                    !await readRepository.ExistsByAssetNumberAsync(value!, null, ct))
                .WithMessage("An asset with this asset number already exists.");

            RuleFor(x => x.Dto.AssetStatusId)
                .GreaterThan(0)
                .WithMessage("Asset status is required.");

            RuleFor(x => x.Dto.AssetStatusId)
                .MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetStatusIdAsync(id, ct))
                .WithMessage("The specified asset status was not found.")
                .When(x => x.Dto.AssetStatusId > 0);

            RuleFor(x => x.Dto.ServiceLocationId)
                .GreaterThan(0)
                .WithMessage("Service location must be greater than zero when provided.")
                .When(x => x.Dto.ServiceLocationId.HasValue);
            RuleFor(x => x.Dto.ServiceLocationId)
                .MustAsync(async (cmd, id, ct) => await readRepository.ExistsServiceLocationIdAsync(id, ct))
                .WithMessage("The specified service location was not found.")
                .When(x => x.Dto.ServiceLocationId.HasValue);

            RuleFor(x => x.Dto.AssetTypeId)
                .MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetTypeIdAsync(id, ct))
                .WithMessage("The specified asset type was not found.")
                .When(x => x.Dto.AssetTypeId.HasValue);
            RuleFor(x => x.Dto.AssetManufacturerId)
                .MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetManufacturerIdAsync(id, ct))
                .WithMessage("The specified asset manufacturer was not found.")
                .When(x => x.Dto.AssetManufacturerId.HasValue);
            RuleFor(x => x.Dto.AssetModelId)
                .MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetModelIdAsync(id, ct))
                .WithMessage("The specified asset model was not found.")
                .When(x => x.Dto.AssetModelId.HasValue);
        });
    }
}

public sealed class UpdateFgsAssetCommandValidator : AbstractValidator<UpdateFgsAssetCommand>
{
    public UpdateFgsAssetCommandValidator(IFgsAssetReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage("Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.AssetNumber)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MaximumLength(100)
                .MustAsync(async (cmd, value, ct) =>
                    !await readRepository.ExistsByAssetNumberAsync(value!, cmd.Id, ct))
                .WithMessage("An asset with this asset number already exists.");

            RuleFor(x => x.Dto.AssetStatusId)
                .GreaterThan(0)
                .WithMessage("Asset status is required.");

            RuleFor(x => x.Dto.AssetStatusId)
                .MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetStatusIdAsync(id, ct))
                .WithMessage("The specified asset status was not found.")
                .When(x => x.Dto.AssetStatusId > 0);

            RuleFor(x => x.Dto.ServiceLocationId)
                .GreaterThan(0)
                .WithMessage("Service location must be greater than zero when provided.")
                .When(x => x.Dto.ServiceLocationId.HasValue);
            RuleFor(x => x.Dto.ServiceLocationId)
                .MustAsync(async (cmd, id, ct) => await readRepository.ExistsServiceLocationIdAsync(id, ct))
                .WithMessage("The specified service location was not found.")
                .When(x => x.Dto.ServiceLocationId.HasValue);

            RuleFor(x => x.Dto.AssetTypeId)
                .MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetTypeIdAsync(id, ct))
                .WithMessage("The specified asset type was not found.")
                .When(x => x.Dto.AssetTypeId.HasValue);
            RuleFor(x => x.Dto.AssetManufacturerId)
                .MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetManufacturerIdAsync(id, ct))
                .WithMessage("The specified asset manufacturer was not found.")
                .When(x => x.Dto.AssetManufacturerId.HasValue);
            RuleFor(x => x.Dto.AssetModelId)
                .MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetModelIdAsync(id, ct))
                .WithMessage("The specified asset model was not found.")
                .When(x => x.Dto.AssetModelId.HasValue);
        });
    }
}

public sealed class PatchFgsAssetCommandValidator : AbstractValidator<PatchFgsAssetCommand>
{
    public PatchFgsAssetCommandValidator(IFgsAssetReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage("Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.AssetNumber)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MaximumLength(100)
                .When(x => x.Dto.AssetNumber is not null);
            RuleFor(x => x.Dto.ServiceLocationId)
                .GreaterThan(0)
                .When(x => x.Dto.ServiceLocationId.HasValue);
            RuleFor(x => x.Dto.AssetStatusId)
                .GreaterThan(0)
                .When(x => x.Dto.AssetStatusId.HasValue);

            RuleFor(x => x.Dto.ServiceLocationId)
                .MustAsync(async (cmd, id, ct) => await readRepository.ExistsServiceLocationIdAsync(id, ct))
                .WithMessage("The specified service location was not found.")
                .When(x => x.Dto.ServiceLocationId.HasValue);
            RuleFor(x => x.Dto.AssetStatusId)
                .MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetStatusIdAsync(id!.Value, ct))
                .WithMessage("The specified asset status was not found.")
                .When(x => x.Dto.AssetStatusId.HasValue);
            RuleFor(x => x.Dto.AssetTypeId)
                .MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetTypeIdAsync(id, ct))
                .WithMessage("The specified asset type was not found.")
                .When(x => x.Dto.AssetTypeId.HasValue);
            RuleFor(x => x.Dto.AssetManufacturerId)
                .MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetManufacturerIdAsync(id, ct))
                .WithMessage("The specified asset manufacturer was not found.")
                .When(x => x.Dto.AssetManufacturerId.HasValue);
            RuleFor(x => x.Dto.AssetModelId)
                .MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetModelIdAsync(id, ct))
                .WithMessage("The specified asset model was not found.")
                .When(x => x.Dto.AssetModelId.HasValue);
        });
    }
}
