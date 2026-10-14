using System.Text.Json;
using Fgs.User.Application.Features.GridPreferences.Commands.DeleteFgsUserGridPreference;
using Fgs.User.Application.Features.GridPreferences.Commands.SaveFgsUserGridPreference;
using Fgs.User.Application.Features.GridPreferences.Queries.GetFgsUserGridPreference;

namespace Fgs.User.Tests.Application;

public sealed class FgsUserGridPreferenceValidatorTests
{
    [Fact]
    public async Task Save_WithMissingNonObjectOrTooLargeConfiguration_Fails()
    {
        var validator = new SaveFgsUserGridPreferenceCommandValidator();
        var tooLong = $"{{\"note\":\"{new string('x', 65_536)}\"}}";

        var blankGrid = await validator.ValidateAsync(
            new SaveFgsUserGridPreferenceCommand("  ", Parse("{}")));
        var longGrid = await validator.ValidateAsync(
            new SaveFgsUserGridPreferenceCommand(new string('g', 101), Parse("{}")));
        var missing = await validator.ValidateAsync(
            new SaveFgsUserGridPreferenceCommand("WorkOrderList", default));
        var nullConfiguration = await validator.ValidateAsync(
            new SaveFgsUserGridPreferenceCommand("WorkOrderList", Parse("null")));
        var array = await validator.ValidateAsync(
            new SaveFgsUserGridPreferenceCommand("WorkOrderList", Parse("[\"name\"]")));
        var oversized = await validator.ValidateAsync(
            new SaveFgsUserGridPreferenceCommand("WorkOrderList", Parse(tooLong)));
        var plainText = await validator.ValidateAsync(
            new SaveFgsUserGridPreferenceCommand("WorkOrderList", JsonSerializer.SerializeToElement("not-json")));
        var stringifiedArray = await validator.ValidateAsync(
            new SaveFgsUserGridPreferenceCommand("WorkOrderList", JsonSerializer.SerializeToElement("[]")));

        blankGrid.IsValid.Should().BeFalse();
        blankGrid.Errors.Should().Contain(e => e.ErrorMessage == "Grid code is required.");
        longGrid.IsValid.Should().BeFalse();
        longGrid.Errors.Should().Contain(e => e.ErrorMessage == "Grid code must be 100 characters or fewer.");
        missing.IsValid.Should().BeFalse();
        missing.Errors.Should().Contain(e => e.ErrorMessage == "Grid configuration is required.");
        nullConfiguration.IsValid.Should().BeFalse();
        nullConfiguration.Errors.Should().Contain(e => e.ErrorMessage == "Grid configuration is required.");
        array.IsValid.Should().BeFalse();
        array.Errors.Should().Contain(e => e.ErrorMessage == "Grid configuration must be a JSON object.");
        oversized.IsValid.Should().BeFalse();
        oversized.Errors.Should().Contain(e => e.ErrorMessage == "Grid configuration must be 65536 characters or fewer.");
        plainText.IsValid.Should().BeFalse();
        plainText.Errors.Should().Contain(e => e.ErrorMessage == "Grid configuration must be valid JSON.");
        stringifiedArray.IsValid.Should().BeFalse();
        stringifiedArray.Errors.Should().Contain(e => e.ErrorMessage == "Grid configuration must be a JSON object.");
    }

    [Fact]
    public async Task Save_WithEmptyOrSelectedGridConfiguration_Succeeds()
    {
        var validator = new SaveFgsUserGridPreferenceCommandValidator();
        var selected = """
            {
              "columns": [{ "field": "name", "width": 180, "visible": true, "order": 0 }],
              "sorting": [{ "field": "name", "direction": "asc" }]
            }
            """;

        var empty = await validator.ValidateAsync(
            new SaveFgsUserGridPreferenceCommand("WorkOrderList", Parse("{}")));
        var distinct = await validator.ValidateAsync(
            new SaveFgsUserGridPreferenceCommand(new string('g', 100), Parse(selected)));
        var stringified = await validator.ValidateAsync(
            new SaveFgsUserGridPreferenceCommand(
                "WorkOrderList",
                JsonSerializer.SerializeToElement("""{"columns":[{"field":"name","width":180}]}""")));

        empty.IsValid.Should().BeTrue();
        distinct.IsValid.Should().BeTrue();
        stringified.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task GetAndDelete_WithBlankGridCode_Fail(string gridCode)
    {
        var get = await new GetFgsUserGridPreferenceQueryValidator()
            .ValidateAsync(new GetFgsUserGridPreferenceQuery(gridCode));
        var delete = await new DeleteFgsUserGridPreferenceCommandValidator()
            .ValidateAsync(new DeleteFgsUserGridPreferenceCommand(gridCode));

        get.IsValid.Should().BeFalse();
        delete.IsValid.Should().BeFalse();
        get.Errors.Should().Contain(e => e.ErrorMessage == "Grid code is required.");
        delete.Errors.Should().Contain(e => e.ErrorMessage == "Grid code is required.");
    }

    [Fact]
    public async Task GetAndDelete_WithTooLongGridCode_Fail()
    {
        var gridCode = new string('g', 101);
        var get = await new GetFgsUserGridPreferenceQueryValidator()
            .ValidateAsync(new GetFgsUserGridPreferenceQuery(gridCode));
        var delete = await new DeleteFgsUserGridPreferenceCommandValidator()
            .ValidateAsync(new DeleteFgsUserGridPreferenceCommand(gridCode));

        get.IsValid.Should().BeFalse();
        delete.IsValid.Should().BeFalse();
        get.Errors.Should().Contain(e => e.ErrorMessage == "Grid code must be 100 characters or fewer.");
        delete.Errors.Should().Contain(e => e.ErrorMessage == "Grid code must be 100 characters or fewer.");
    }

    private static JsonElement Parse(string json) => JsonDocument.Parse(json).RootElement.Clone();
}
