using Fgs.Setup.Application.Abstractions.SetupPaymentMethods;
using Fgs.Setup.Application.Features.SetupPaymentMethods.Commands.CreateFgsSetupPaymentMethod;
using Fgs.Setup.Application.Features.SetupPaymentMethods.Commands.PatchFgsSetupPaymentMethod;
using Fgs.Setup.Application.Features.SetupPaymentMethods.Commands.UpdateFgsSetupPaymentMethod;
using Fgs.Setup.Application.Features.SetupPaymentMethods.Dtos;
using Fgs.Setup.Application.Features.SetupPaymentMethods.Validators;
using Moq;

namespace Fgs.Setup.Tests.SetupPaymentMethods;

public sealed class FgsSetupPaymentMethodValidatorTests
{
    private readonly Mock<IFgsSetupPaymentMethodReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenDisplayNameMissing_HasValidationError()
    {
        var validator = new CreateFgsSetupPaymentMethodCommandValidator(_readRepository.Object);
        var command = new CreateFgsSetupPaymentMethodCommand(new FgsSetupPaymentMethodCreateDto("", 1, true, true));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.DisplayName");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {

        _readRepository
            .Setup(r => r.ExistsByDisplayNameAsync(It.IsAny<string>(), 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var validator = new UpdateFgsSetupPaymentMethodCommandValidator(_readRepository.Object);
        var command = new UpdateFgsSetupPaymentMethodCommand(5, new FgsSetupPaymentMethodUpdateDto("DisplayName value", 1, true, true));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
