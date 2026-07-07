using Fgs.Setup.Application.Abstractions.UniversalPricingServices;
using Fgs.Setup.Application.Features.UniversalPricingServices.Commands.CreateFgsUniversalPricingService;
using Fgs.Setup.Application.Features.UniversalPricingServices.Commands.PatchFgsUniversalPricingService;
using Fgs.Setup.Application.Features.UniversalPricingServices.Commands.UpdateFgsUniversalPricingService;
using Fgs.Setup.Application.Features.UniversalPricingServices.Dtos;
using Fgs.Setup.Application.Features.UniversalPricingServices.Validators;
using Moq;

namespace Fgs.Setup.Tests.UniversalPricingServices;

public sealed class FgsUniversalPricingServiceValidatorTests
{
    private readonly Mock<IFgsUniversalPricingServiceReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenUniversalPricingServiceCodeMissing_HasValidationError()
    {
        var validator = new CreateFgsUniversalPricingServiceCommandValidator(_readRepository.Object);
        var command = new CreateFgsUniversalPricingServiceCommand(new FgsUniversalPricingServiceCreateDto("", 5));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.UniversalPricingServiceCode");
    }

    [Fact]
    public async Task CreateValidator_WhenUniversalPricingServiceCodeNotUppercase_HasValidationError()
    {
        var validator = new CreateFgsUniversalPricingServiceCommandValidator(_readRepository.Object);
        var args = new FgsUniversalPricingServiceCreateDto("TEST", 5);
        var command = new CreateFgsUniversalPricingServiceCommand(args with { UniversalPricingServiceCode = "test" });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.UniversalPricingServiceCode");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {

        _readRepository
            .Setup(r => r.ExistsByUniversalPricingServiceCodeAsync("TEST", 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _readRepository
            .Setup(r => r.ExistsGloUniversalPricingServiceCodeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var validator = new UpdateFgsUniversalPricingServiceCommandValidator(_readRepository.Object);
        var command = new UpdateFgsUniversalPricingServiceCommand(5, new FgsUniversalPricingServiceUpdateDto("TEST", 5));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
