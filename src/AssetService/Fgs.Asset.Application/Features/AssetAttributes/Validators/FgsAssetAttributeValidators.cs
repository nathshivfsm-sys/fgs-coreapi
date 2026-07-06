using Fgs.Asset.Application.Abstractions.AssetAttributes;
using Fgs.Asset.Application.Features.AssetAttributes.Commands.CreateFgsAssetAttribute;
using Fgs.Asset.Application.Features.AssetAttributes.Commands.PatchFgsAssetAttribute;
using Fgs.Asset.Application.Features.AssetAttributes.Commands.UpdateFgsAssetAttribute;
using FluentValidation;
namespace Fgs.Asset.Application.Features.AssetAttributes.Validators;
public sealed class CreateFgsAssetAttributeCommandValidator : AbstractValidator<CreateFgsAssetAttributeCommand>
{
    private static readonly HashSet<string> Allowed = new(StringComparer.OrdinalIgnoreCase) { "TEXT", "TEXTAREA", "INTEGER", "DECIMAL", "DATE", "BOOLEAN", "DROPDOWN" };
    public CreateFgsAssetAttributeCommandValidator(IFgsAssetAttributeReadRepository readRepository)
    {
        RuleFor(x => x.Dto.AssetTypeId).GreaterThan(0);
        RuleFor(x => x.Dto.AttributeCode).NotEmpty().MaximumLength(75);
        RuleFor(x => x.Dto.AttributeCode).Must(c => string.Equals(c, c.Trim().ToUpperInvariant(), StringComparison.Ordinal));
        RuleFor(x => x.Dto.AttributeCode).MustAsync(async (cmd, code, ct) => !await readRepository.ExistsByAttributeCodeAsync(cmd.Dto.AssetTypeId, code, null, ct));
        RuleFor(x => x.Dto.AttributeName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Dto.InputType).NotEmpty().Must(t => Allowed.Contains(t));
        RuleFor(x => x.Dto.AssetTypeId).MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetTypeIdAsync(id, ct));
        RuleFor(x => x.Dto.DefaultValueText).MaximumLength(500);
    }
}
public sealed class UpdateFgsAssetAttributeCommandValidator : AbstractValidator<UpdateFgsAssetAttributeCommand>
{
    private static readonly HashSet<string> Allowed = new(StringComparer.OrdinalIgnoreCase) { "TEXT", "TEXTAREA", "INTEGER", "DECIMAL", "DATE", "BOOLEAN", "DROPDOWN" };
    public UpdateFgsAssetAttributeCommandValidator(IFgsAssetAttributeReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.AssetTypeId).GreaterThan(0);
        RuleFor(x => x.Dto.AttributeCode).NotEmpty().MaximumLength(75);
        RuleFor(x => x.Dto.AttributeCode).MustAsync(async (cmd, code, ct) => !await readRepository.ExistsByAttributeCodeAsync(cmd.Dto.AssetTypeId, code, cmd.Id, ct));
        RuleFor(x => x.Dto.AttributeName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Dto.InputType).NotEmpty().Must(t => Allowed.Contains(t));
        RuleFor(x => x.Dto.AssetTypeId).MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetTypeIdAsync(id, ct));
    }
}
public sealed class PatchFgsAssetAttributeCommandValidator : AbstractValidator<PatchFgsAssetAttributeCommand>
{
    private static readonly HashSet<string> Allowed = new(StringComparer.OrdinalIgnoreCase) { "TEXT", "TEXTAREA", "INTEGER", "DECIMAL", "DATE", "BOOLEAN", "DROPDOWN" };
    public PatchFgsAssetAttributeCommandValidator(IFgsAssetAttributeReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.InputType).Must(t => Allowed.Contains(t!)).When(x => x.Dto.InputType is not null);
        RuleFor(x => x.Dto.AssetTypeId).MustAsync(async (cmd, id, ct) => !id.HasValue || await readRepository.ExistsAssetTypeIdAsync(id.Value, ct)).When(x => x.Dto.AssetTypeId.HasValue);
    }
}
