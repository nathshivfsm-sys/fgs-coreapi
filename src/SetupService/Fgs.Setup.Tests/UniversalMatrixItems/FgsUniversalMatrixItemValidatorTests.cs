using Fgs.Setup.Application.Abstractions.UniversalMatrixItems;
using Fgs.Setup.Application.Features.UniversalMatrixItems.Commands.CreateFgsUniversalMatrixItem;
using Fgs.Setup.Application.Features.UniversalMatrixItems.Commands.PatchFgsUniversalMatrixItem;
using Fgs.Setup.Application.Features.UniversalMatrixItems.Commands.UpdateFgsUniversalMatrixItem;
using Fgs.Setup.Application.Features.UniversalMatrixItems.Dtos;
using Fgs.Setup.Application.Features.UniversalMatrixItems.Validators;
using Moq;

namespace Fgs.Setup.Tests.UniversalMatrixItems;

public sealed class FgsUniversalMatrixItemValidatorTests
{
    private readonly Mock<IFgsUniversalMatrixItemReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenItemNameMissing_HasValidationError()
    {
        var validator = new CreateFgsUniversalMatrixItemCommandValidator(_readRepository.Object);
        var command = new CreateFgsUniversalMatrixItemCommand(new FgsUniversalMatrixItemCreateDto(1, "", "UnitType", 10.5m, 5));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.ItemName");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {

        _readRepository
            .Setup(r => r.ExistsByUniversalPricingServiceIdAndItemNameAsync(It.IsAny<long>(), It.IsAny<string>(), 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _readRepository
            .Setup(r => r.ExistsUniversalPricingServiceIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var validator = new UpdateFgsUniversalMatrixItemCommandValidator(_readRepository.Object);
        var command = new UpdateFgsUniversalMatrixItemCommand(5, new FgsUniversalMatrixItemUpdateDto(1, "ItemName", "UnitType", 10.5m, 5));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
