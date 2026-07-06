using Fgs.Asset.Application.Abstractions.AssetModels;
using Fgs.Asset.Application.Features.AssetModels.Commands.CreateFgsAssetModel;
using Fgs.Asset.Application.Features.AssetModels.Commands.UpdateFgsAssetModel;
using Fgs.Asset.Application.Features.AssetModels.Dtos;
using Fgs.Asset.Application.Features.AssetModels.Validators;
using Moq;

namespace Fgs.Asset.Tests.AssetModels;

public sealed class FgsAssetModelValidatorTests
{
    private readonly Mock<IFgsAssetModelReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenModelNumberMissing_HasValidationError()
    {
        var validator = new CreateFgsAssetModelCommandValidator(_readRepository.Object);
        var command = new CreateFgsAssetModelCommand(new FgsAssetModelCreateDto(1, 1, "", "Description"));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.ModelNumber");
    }

    [Fact]
    public async Task UpdateValidator_WhenAssetTypeMissing_HasValidationError()
    {
        _readRepository.Setup(r => r.ExistsAssetManufacturerIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        var validator = new UpdateFgsAssetModelCommandValidator(_readRepository.Object);
        var command = new UpdateFgsAssetModelCommand(1, new FgsAssetModelUpdateDto(0, 1, "58MCA", "Description"));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
    }
}
