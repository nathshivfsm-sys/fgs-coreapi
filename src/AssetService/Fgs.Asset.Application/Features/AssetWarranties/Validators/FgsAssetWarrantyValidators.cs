using Fgs.Asset.Application.Abstractions.AssetWarranties;
using Fgs.Asset.Application.Features.AssetWarranties.Commands.CreateFgsAssetWarranty;
using Fgs.Asset.Application.Features.AssetWarranties.Commands.PatchFgsAssetWarranty;
using Fgs.Asset.Application.Features.AssetWarranties.Commands.UpdateFgsAssetWarranty;
using FluentValidation;
namespace Fgs.Asset.Application.Features.AssetWarranties.Validators;
public sealed class CreateFgsAssetWarrantyCommandValidator : AbstractValidator<CreateFgsAssetWarrantyCommand>
{
    public CreateFgsAssetWarrantyCommandValidator(IFgsAssetWarrantyReadRepository readRepository)
    {
        RuleFor(x => x.Dto.AssetId).GreaterThan(0);
        RuleFor(x => x.Dto.WarrantyType).NotEmpty().MaximumLength(75);
        RuleFor(x => x.Dto.WarrantyType).Must(t => string.Equals(t, t.Trim().ToUpperInvariant(), StringComparison.Ordinal));
        RuleFor(x => x.Dto.EndDate).GreaterThanOrEqualTo(x => x.Dto.StartDate);
        RuleFor(x => x.Dto.AssetId).MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetIdAsync(id, ct));
        RuleFor(x => x.Dto.WarrantyProvider).MaximumLength(200);
        RuleFor(x => x.Dto.WarrantyNumber).MaximumLength(100);
        RuleFor(x => x.Dto.RegistrationNumber).MaximumLength(100);
        RuleFor(x => x.Dto.CoverageDescription).MaximumLength(1000);
    }
}
public sealed class UpdateFgsAssetWarrantyCommandValidator : AbstractValidator<UpdateFgsAssetWarrantyCommand>
{
    public UpdateFgsAssetWarrantyCommandValidator(IFgsAssetWarrantyReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.AssetId).GreaterThan(0);
        RuleFor(x => x.Dto.WarrantyType).NotEmpty().MaximumLength(75);
        RuleFor(x => x.Dto.WarrantyType).Must(t => string.Equals(t, t.Trim().ToUpperInvariant(), StringComparison.Ordinal));
        RuleFor(x => x.Dto.EndDate).GreaterThanOrEqualTo(x => x.Dto.StartDate);
        RuleFor(x => x.Dto.AssetId).MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetIdAsync(id, ct));
        RuleFor(x => x.Dto.WarrantyProvider).MaximumLength(200);
        RuleFor(x => x.Dto.WarrantyNumber).MaximumLength(100);
        RuleFor(x => x.Dto.RegistrationNumber).MaximumLength(100);
        RuleFor(x => x.Dto.CoverageDescription).MaximumLength(1000);
    }
}
public sealed class PatchFgsAssetWarrantyCommandValidator : AbstractValidator<PatchFgsAssetWarrantyCommand>
{
    public PatchFgsAssetWarrantyCommandValidator(IFgsAssetWarrantyReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.AssetId).GreaterThan(0).When(x => x.Dto.AssetId.HasValue);
        RuleFor(x => x.Dto.WarrantyType).NotEmpty().MaximumLength(75).When(x => x.Dto.WarrantyType is not null);
        RuleFor(x => x.Dto.AssetId).MustAsync(async (cmd, id, ct) => !id.HasValue || await readRepository.ExistsAssetIdAsync(id.Value, ct)).When(x => x.Dto.AssetId.HasValue);
    }
}
