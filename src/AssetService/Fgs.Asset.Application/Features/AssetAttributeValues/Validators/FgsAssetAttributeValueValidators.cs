using Fgs.Asset.Application.Abstractions.AssetAttributeValues;
using Fgs.Asset.Application.Features.AssetAttributeValues.Commands.CreateFgsAssetAttributeValue;
using Fgs.Asset.Application.Features.AssetAttributeValues.Commands.PatchFgsAssetAttributeValue;
using Fgs.Asset.Application.Features.AssetAttributeValues.Commands.UpdateFgsAssetAttributeValue;
using FluentValidation;
namespace Fgs.Asset.Application.Features.AssetAttributeValues.Validators;
public sealed class CreateFgsAssetAttributeValueCommandValidator : AbstractValidator<CreateFgsAssetAttributeValueCommand>
{
    public CreateFgsAssetAttributeValueCommandValidator(IFgsAssetAttributeValueReadRepository readRepository)
    {
        RuleFor(x => x.Dto.AssetId).GreaterThan(0);
        RuleFor(x => x.Dto.AssetAttributeId).GreaterThan(0);
        RuleFor(x => x.Dto.AssetId).MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetIdAsync(id, ct));
        RuleFor(x => x.Dto.AssetAttributeId).MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetAttributeIdAsync(id, ct));
        RuleFor(x => x.Dto.ValueText).MaximumLength(500);
    }
}
public sealed class UpdateFgsAssetAttributeValueCommandValidator : AbstractValidator<UpdateFgsAssetAttributeValueCommand>
{
    public UpdateFgsAssetAttributeValueCommandValidator(IFgsAssetAttributeValueReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.AssetId).GreaterThan(0);
        RuleFor(x => x.Dto.AssetAttributeId).GreaterThan(0);
        RuleFor(x => x.Dto.AssetId).MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetIdAsync(id, ct));
        RuleFor(x => x.Dto.AssetAttributeId).MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetAttributeIdAsync(id, ct));
        RuleFor(x => x.Dto.ValueText).MaximumLength(500);
    }
}
public sealed class PatchFgsAssetAttributeValueCommandValidator : AbstractValidator<PatchFgsAssetAttributeValueCommand>
{
    public PatchFgsAssetAttributeValueCommandValidator(IFgsAssetAttributeValueReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.AssetId).GreaterThan(0).When(x => x.Dto.AssetId.HasValue);
        RuleFor(x => x.Dto.AssetAttributeId).GreaterThan(0).When(x => x.Dto.AssetAttributeId.HasValue);
        RuleFor(x => x.Dto.AssetId).MustAsync(async (cmd, id, ct) => !id.HasValue || await readRepository.ExistsAssetIdAsync(id.Value, ct)).When(x => x.Dto.AssetId.HasValue);
        RuleFor(x => x.Dto.AssetAttributeId).MustAsync(async (cmd, id, ct) => !id.HasValue || await readRepository.ExistsAssetAttributeIdAsync(id.Value, ct)).When(x => x.Dto.AssetAttributeId.HasValue);
        RuleFor(x => x.Dto.ValueText).MaximumLength(500).When(x => x.Dto.ValueText is not null);
    }
}
