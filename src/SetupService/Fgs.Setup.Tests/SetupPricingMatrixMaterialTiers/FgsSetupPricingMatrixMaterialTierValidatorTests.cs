using Fgs.Setup.Application.Abstractions.SetupPricingMatrices;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrixMaterialTiers;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Dtos;
using Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Commands.CreateFgsSetupPricingMatrixMaterialTier;
using Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Dtos;
using Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Validators;
using Moq;

namespace Fgs.Setup.Tests.SetupPricingMatrixMaterialTiers;

public sealed class FgsSetupPricingMatrixMaterialTierValidatorTests
{
    private readonly Mock<IFgsSetupPricingMatrixReadRepository> _matrices = new();
    private readonly Mock<IFgsSetupPricingMatrixMaterialTierReadRepository> _tiers = new();

    [Fact]
    public async Task CreateValidator_WhenActiveOtherItemsExist_RejectsAsMutuallyExclusive()
    {
        SetupValidMatrix();
        _tiers.Setup(x => x.ExistsActiveOtherItemsForMatrixAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await Validator().ValidateAsync(
            new CreateFgsSetupPricingMatrixMaterialTierCommand(
                new FgsSetupPricingMatrixMaterialTierCreateDto(1, 0m, 100m, 10m)));

        result.Errors.Should().Contain(e =>
            e.PropertyName == "Dto.PricingMatrixId" && e.ErrorMessage.Contains("cannot coexist"));
    }

    [Fact]
    public async Task CreateValidator_WhenMaterialTierIsValid_Passes()
    {
        SetupValidMatrix();
        _tiers.Setup(x => x.ExistsActiveOtherItemsForMatrixAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _tiers.Setup(x => x.ExistsByFromCostAsync(
            1, 0m, null, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var result = await Validator().ValidateAsync(
            new CreateFgsSetupPricingMatrixMaterialTierCommand(
                new FgsSetupPricingMatrixMaterialTierCreateDto(1, 0m, 100m, 10m)));

        result.IsValid.Should().BeTrue();
    }

    private void SetupValidMatrix() =>
        _matrices.Setup(x => x.GetFlagsByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FgsSetupPricingMatrixFlagsDto(1, false, false, 1, true));

    private CreateFgsSetupPricingMatrixMaterialTierCommandValidator Validator() =>
        new(_matrices.Object, _tiers.Object);
}
