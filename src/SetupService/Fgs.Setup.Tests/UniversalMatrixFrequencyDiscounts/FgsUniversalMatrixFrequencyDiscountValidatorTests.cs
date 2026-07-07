using Fgs.Setup.Application.Abstractions.UniversalMatrixFrequencyDiscounts;
using Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Commands.CreateFgsUniversalMatrixFrequencyDiscount;
using Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Commands.PatchFgsUniversalMatrixFrequencyDiscount;
using Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Commands.UpdateFgsUniversalMatrixFrequencyDiscount;
using Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Dtos;
using Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Validators;
using Moq;

namespace Fgs.Setup.Tests.UniversalMatrixFrequencyDiscounts;

public sealed class FgsUniversalMatrixFrequencyDiscountValidatorTests
{
    private readonly Mock<IFgsUniversalMatrixFrequencyDiscountReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenNameMissing_HasValidationError()
    {
        var validator = new CreateFgsUniversalMatrixFrequencyDiscountCommandValidator(_readRepository.Object);
        var command = new CreateFgsUniversalMatrixFrequencyDiscountCommand(new FgsUniversalMatrixFrequencyDiscountCreateDto(1, "", 10.5m, 5));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.Name");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {

        _readRepository
            .Setup(r => r.ExistsByUniversalPricingServiceIdAndNameAsync(It.IsAny<long>(), It.IsAny<string>(), 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _readRepository
            .Setup(r => r.ExistsUniversalPricingServiceIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var validator = new UpdateFgsUniversalMatrixFrequencyDiscountCommandValidator(_readRepository.Object);
        var command = new UpdateFgsUniversalMatrixFrequencyDiscountCommand(5, new FgsUniversalMatrixFrequencyDiscountUpdateDto(1, "Name", 10.5m, 5));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
