using Fgs.User.Application.Features.GridPreferences.Commands.DeleteFgsUserGridPreference;
using Fgs.User.Application.Features.GridPreferences.Commands.SaveFgsUserGridPreference;
using Fgs.User.Application.Features.GridPreferences.Queries.GetFgsUserGridPreference;

namespace Fgs.User.Tests.Application;

public sealed class FgsUserGridPreferenceValidatorTests
{
    [Fact]
    public async Task Save_WithBlankDuplicateAndTooLongCodes_Fails()
    {
        var validator = new SaveFgsUserGridPreferenceCommandValidator();
        var tooLong = new string('x', 101);

        var blankGrid = await validator.ValidateAsync(new SaveFgsUserGridPreferenceCommand("  ", ["name"]));
        var longGrid = await validator.ValidateAsync(new SaveFgsUserGridPreferenceCommand(tooLong, ["name"]));
        var blankColumn = await validator.ValidateAsync(new SaveFgsUserGridPreferenceCommand("WorkOrderList", ["name", " "]));
        var duplicate = await validator.ValidateAsync(
            new SaveFgsUserGridPreferenceCommand("WorkOrderList", ["name", " name "]));
        var longColumn = await validator.ValidateAsync(new SaveFgsUserGridPreferenceCommand("WorkOrderList", [tooLong]));
        var tooMany = await validator.ValidateAsync(
            new SaveFgsUserGridPreferenceCommand("WorkOrderList", Enumerable.Range(0, 101).Select(i => $"c{i}").ToArray()));
        var missingColumns = await validator.ValidateAsync(new SaveFgsUserGridPreferenceCommand("WorkOrderList", null));

        blankGrid.IsValid.Should().BeFalse();
        blankGrid.Errors.Should().Contain(e => e.ErrorMessage == "Grid code is required.");
        longGrid.IsValid.Should().BeFalse();
        longGrid.Errors.Should().Contain(e => e.ErrorMessage == "Grid code must be 100 characters or fewer.");
        blankColumn.IsValid.Should().BeFalse();
        blankColumn.Errors.Should().Contain(e => e.ErrorMessage == "Column code is required.");
        duplicate.IsValid.Should().BeFalse();
        duplicate.Errors.Should().Contain(e => e.ErrorMessage == "Column codes must be distinct.");
        longColumn.IsValid.Should().BeFalse();
        longColumn.Errors.Should().Contain(e => e.ErrorMessage == "Column code must be 100 characters or fewer.");
        tooMany.IsValid.Should().BeFalse();
        tooMany.Errors.Should().Contain(e => e.ErrorMessage == "At most 100 column codes are allowed.");
        missingColumns.IsValid.Should().BeFalse();
        missingColumns.Errors.Should().Contain(e => e.ErrorMessage == "Columns are required.");
    }

    [Fact]
    public async Task Save_WithEmptyOrDistinctColumns_Succeeds()
    {
        var validator = new SaveFgsUserGridPreferenceCommandValidator();

        var empty = await validator.ValidateAsync(new SaveFgsUserGridPreferenceCommand("WorkOrderList", []));
        var distinct = await validator.ValidateAsync(
            new SaveFgsUserGridPreferenceCommand(new string('g', 100), ["name", "city", new string('c', 100)]));

        empty.IsValid.Should().BeTrue();
        distinct.IsValid.Should().BeTrue();
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
}
