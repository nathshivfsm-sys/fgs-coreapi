using Fgs.Setup.Application.Abstractions.SetupPricingMatrices;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrixLabors;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrixLaborTiers;
using Fgs.Setup.Application.Abstractions.SetupTechSkillLevels;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Dtos;
using Fgs.Setup.Application.Features.SetupPricingMatrixLaborTiers.Commands.CreateFgsSetupPricingMatrixLaborTier;
using Fgs.Setup.Application.Features.SetupPricingMatrixLaborTiers.Dtos;
using Fgs.Setup.Application.Features.SetupPricingMatrixLaborTiers.Validators;
using Moq;

namespace Fgs.Setup.Tests.SetupPricingMatrixLaborTiers;

public sealed class FgsSetupPricingMatrixLaborTierValidatorTests
{
    private readonly Mock<IFgsSetupPricingMatrixLaborReadRepository> _labors = new();
    private readonly Mock<IFgsSetupPricingMatrixLaborTierReadRepository> _tiers = new();
    private readonly Mock<IFgsSetupPricingMatrixReadRepository> _matrices = new();
    private readonly Mock<IFgsSetupTechSkillLevelReadRepository> _skills = new();

    [Fact]
    public async Task CreateValidator_WhenMatrixIsNotTiered_Rejects()
    {
        Setup(false);

        var result = await Validator().ValidateAsync(
            new CreateFgsSetupPricingMatrixLaborTierCommand(
                new FgsSetupPricingMatrixLaborTierCreateDto(10, 1, 60, 50m, null)));

        result.Errors.Should().Contain(e =>
            e.PropertyName == "Dto.PricingMatrixLaborId" &&
            e.ErrorMessage.Contains("does not allow labor tiers"));
    }

    [Fact]
    public async Task CreateValidator_WhenTierIsValid_Passes()
    {
        Setup(true);

        var result = await Validator().ValidateAsync(
            new CreateFgsSetupPricingMatrixLaborTierCommand(
                new FgsSetupPricingMatrixLaborTierCreateDto(10, 1, 60, 50m, null)));

        result.IsValid.Should().BeTrue();
    }

    private void Setup(bool isTiered)
    {
        _labors.Setup(x => x.GetPricingMatrixIdAsync(10, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        _matrices.Setup(x => x.GetFlagsByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FgsSetupPricingMatrixFlagsDto(1, isTiered, false, 1, true));
        _tiers.Setup(x => x.ExistsBySequenceOrderAsync(
            10, 1, null, It.IsAny<CancellationToken>())).ReturnsAsync(false);
    }

    private CreateFgsSetupPricingMatrixLaborTierCommandValidator Validator() =>
        new(_labors.Object, _tiers.Object, _matrices.Object, _skills.Object);
}
