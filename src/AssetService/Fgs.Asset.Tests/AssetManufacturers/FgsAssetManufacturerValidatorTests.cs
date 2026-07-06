using Fgs.Asset.Application.Abstractions.AssetManufacturers;
using Fgs.Asset.Application.Features.AssetManufacturers.Commands.CreateFgsAssetManufacturer;
using Fgs.Asset.Application.Features.AssetManufacturers.Commands.UpdateFgsAssetManufacturer;
using Fgs.Asset.Application.Features.AssetManufacturers.Dtos;
using Fgs.Asset.Application.Features.AssetManufacturers.Validators;
using Moq;

namespace Fgs.Asset.Tests.AssetManufacturers;

public sealed class FgsAssetManufacturerValidatorTests
{
    private readonly Mock<IFgsAssetManufacturerReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenCodeMissing_HasValidationError()
    {
        var validator = new CreateFgsAssetManufacturerCommandValidator(_readRepository.Object);
        var command = new CreateFgsAssetManufacturerCommand(new FgsAssetManufacturerCreateDto("", "Name", null));
        var result = await validator.ValidateAsync(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(err => err.PropertyName == "Dto.Code");
    }

    [Fact]
    public async Task CreateValidator_WhenCodeNotUppercase_HasValidationError()
    {
        var validator = new CreateFgsAssetManufacturerCommandValidator(_readRepository.Object);
        var command = new CreateFgsAssetManufacturerCommand(new FgsAssetManufacturerCreateDto("test", "Name", null));
        var result = await validator.ValidateAsync(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(err => err.PropertyName == "Dto.Code");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {
        _readRepository.Setup(repo => repo.ExistsByCodeAsync("TEST", 5, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        var validator = new UpdateFgsAssetManufacturerCommandValidator(_readRepository.Object);
        var command = new UpdateFgsAssetManufacturerCommand(5, new FgsAssetManufacturerUpdateDto("TEST", "Name", null));
        var result = await validator.ValidateAsync(command);
        result.IsValid.Should().BeTrue();
    }
}
