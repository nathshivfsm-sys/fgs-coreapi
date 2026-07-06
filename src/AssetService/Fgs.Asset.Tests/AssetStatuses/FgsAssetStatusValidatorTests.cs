using Fgs.Asset.Application.Abstractions.AssetStatuses;
using Fgs.Asset.Application.Features.AssetStatuses.Commands.CreateFgsAssetStatus;
using Fgs.Asset.Application.Features.AssetStatuses.Commands.UpdateFgsAssetStatus;
using Fgs.Asset.Application.Features.AssetStatuses.Dtos;
using Fgs.Asset.Application.Features.AssetStatuses.Validators;
using Moq;

namespace Fgs.Asset.Tests.AssetStatuses;

public sealed class FgsAssetStatusValidatorTests
{
    private readonly Mock<IFgsAssetStatusReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenCodeMissing_HasValidationError()
    {
        var validator = new CreateFgsAssetStatusCommandValidator(_readRepository.Object);
        var command = new CreateFgsAssetStatusCommand(new FgsAssetStatusCreateDto("", "Name", null));
        var result = await validator.ValidateAsync(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(err => err.PropertyName == "Dto.Code");
    }

    [Fact]
    public async Task CreateValidator_WhenCodeNotUppercase_HasValidationError()
    {
        var validator = new CreateFgsAssetStatusCommandValidator(_readRepository.Object);
        var command = new CreateFgsAssetStatusCommand(new FgsAssetStatusCreateDto("test", "Name", null));
        var result = await validator.ValidateAsync(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(err => err.PropertyName == "Dto.Code");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {
        _readRepository.Setup(repo => repo.ExistsByCodeAsync("TEST", 5, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        var validator = new UpdateFgsAssetStatusCommandValidator(_readRepository.Object);
        var command = new UpdateFgsAssetStatusCommand(5, new FgsAssetStatusUpdateDto("TEST", "Name", null));
        var result = await validator.ValidateAsync(command);
        result.IsValid.Should().BeTrue();
    }
}
