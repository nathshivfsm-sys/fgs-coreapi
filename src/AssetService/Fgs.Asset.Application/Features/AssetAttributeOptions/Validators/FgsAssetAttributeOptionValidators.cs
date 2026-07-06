using Fgs.Asset.Application.Abstractions.AssetAttributeOptions;
using Fgs.Asset.Application.Features.AssetAttributeOptions.Commands.CreateFgsAssetAttributeOption;
using Fgs.Asset.Application.Features.AssetAttributeOptions.Commands.PatchFgsAssetAttributeOption;
using Fgs.Asset.Application.Features.AssetAttributeOptions.Commands.UpdateFgsAssetAttributeOption;
using FluentValidation;
namespace Fgs.Asset.Application.Features.AssetAttributeOptions.Validators;
public sealed class CreateFgsAssetAttributeOptionCommandValidator : AbstractValidator<CreateFgsAssetAttributeOptionCommand>
{
    public CreateFgsAssetAttributeOptionCommandValidator(IFgsAssetAttributeOptionReadRepository readRepository)
    {
        RuleFor(x => x.Dto.AssetAttributeId).GreaterThan(0);
        RuleFor(x => x.Dto.OptionCode).NotEmpty().MaximumLength(75);
        RuleFor(x => x.Dto.OptionCode).Must(c => string.Equals(c, c.Trim().ToUpperInvariant(), StringComparison.Ordinal));
        RuleFor(x => x.Dto.OptionCode).MustAsync(async (cmd, code, ct) => !await readRepository.ExistsByOptionCodeAsync(cmd.Dto.AssetAttributeId, code, null, ct));
        RuleFor(x => x.Dto.OptionName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Dto.AssetAttributeId).MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetAttributeIdAsync(id, ct));
    }
}
public sealed class UpdateFgsAssetAttributeOptionCommandValidator : AbstractValidator<UpdateFgsAssetAttributeOptionCommand>
{
    public UpdateFgsAssetAttributeOptionCommandValidator(IFgsAssetAttributeOptionReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.AssetAttributeId).GreaterThan(0);
        RuleFor(x => x.Dto.OptionCode).NotEmpty().MaximumLength(75);
        RuleFor(x => x.Dto.OptionCode).MustAsync(async (cmd, code, ct) => !await readRepository.ExistsByOptionCodeAsync(cmd.Dto.AssetAttributeId, code, cmd.Id, ct));
        RuleFor(x => x.Dto.OptionName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Dto.AssetAttributeId).MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetAttributeIdAsync(id, ct));
    }
}
public sealed class PatchFgsAssetAttributeOptionCommandValidator : AbstractValidator<PatchFgsAssetAttributeOptionCommand>
{
    public PatchFgsAssetAttributeOptionCommandValidator(IFgsAssetAttributeOptionReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.AssetAttributeId).MustAsync(async (cmd, id, ct) => !id.HasValue || await readRepository.ExistsAssetAttributeIdAsync(id.Value, ct)).When(x => x.Dto.AssetAttributeId.HasValue);
    }
}
