using Fgs.Asset.Application.Abstractions.AssetTypes;
using Fgs.Asset.Application.Features.AssetTypes.Commands.CreateFgsAssetType;
using Fgs.Asset.Application.Features.AssetTypes.Commands.UpdateFgsAssetType;
using Fgs.Asset.Application.Features.AssetTypes.Dtos;
using Fgs.Asset.Application.Features.AssetTypes.Validators;
using Moq;

namespace Fgs.Asset.Tests.AssetTypes;

public sealed class FgsAssetTypeValidatorTests
{
    private readonly Mock<IFgsAssetTypeReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenCodeMissing_HasValidationError()
    {
        var validator = new CreateFgsAssetTypeCommandValidator(_readRepository.Object);
        var command = new CreateFgsAssetTypeCommand(new FgsAssetTypeCreateDto("", "Name", null));
        var result = await validator.ValidateAsync(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(err => err.PropertyName == "Dto.Code");
    }

    [Fact]
    public async Task CreateValidator_WhenCodeNotUppercase_HasValidationError()
    {
        var validator = new CreateFgsAssetTypeCommandValidator(_readRepository.Object);
        var command = new CreateFgsAssetTypeCommand(new FgsAssetTypeCreateDto("test", "Name", null));
        var result = await validator.ValidateAsync(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(err => err.PropertyName == "Dto.Code");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {
        _readRepository.Setup(repo => repo.ExistsByCodeAsync("TEST", 5, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        var validator = new UpdateFgsAssetTypeCommandValidator(_readRepository.Object);
        var command = new UpdateFgsAssetTypeCommand(5, new FgsAssetTypeUpdateDto("TEST", "Name", null));
        var result = await validator.ValidateAsync(command);
        result.IsValid.Should().BeTrue();
    }
}
