using Fgs.Asset.Application.Abstractions.AssetAttributes;
using Fgs.Asset.Application.Features.AssetAttributes.Commands.CreateFgsAssetAttribute;
using Fgs.Asset.Application.Features.AssetAttributes.Dtos;
using Fgs.Asset.Application.Features.AssetAttributes.Validators;
using Moq;
namespace Fgs.Asset.Tests.AssetAttributes;
public sealed class FgsAssetAttributeValidatorTests
{
  [Fact] public async Task CreateValidator_RejectsInvalidInputType() { var repo = new Mock<IFgsAssetAttributeReadRepository>(); repo.Setup(r => r.ExistsAssetTypeIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(true); var v = new CreateFgsAssetAttributeCommandValidator(repo.Object); var r = await v.ValidateAsync(new CreateFgsAssetAttributeCommand(new FgsAssetAttributeCreateDto(1, "CODE", "Name", "BAD", null, null, null, null, null, null, false, true, 0))); r.IsValid.Should().BeFalse(); }
}
