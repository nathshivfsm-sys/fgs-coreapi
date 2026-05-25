using Fgs.User.Application.TenantProvisioning;
using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Provisioning;

namespace Fgs.User.Tests.Infrastructure;

public sealed class TenantDataSeedingEngineTests
{
    [Fact]
    public void BuildSelectExpression_WithDirectColumn_QuotesSourceColumn()
    {
        var column = new GloSeedTableColumnMapping
        {
            Id = 1,
            SourceColumnName = "Code",
            TargetColumnName = "Code"
        };

        TenantDataSeedingEngine.BuildSelectExpression(column).Should().Be("\"Code\"");
    }

    [Theory]
    [InlineData(SeedTransformationTypes.TenantId, "@tenantId")]
    [InlineData(SeedTransformationTypes.CompanyId, "@companyId")]
    public void BuildSelectExpression_WithParameterTransformations_ReturnsParameterReference(
        string transformationType,
        string expected)
    {
        var column = new GloSeedTableColumnMapping
        {
            Id = 2,
            TargetColumnName = "TenantId",
            TransformationType = transformationType
        };

        TenantDataSeedingEngine.BuildSelectExpression(column).Should().Be(expected);
    }

    [Fact]
    public void BuildSelectExpression_WithSeedCreatedBy_ReturnsDataSeedLiteral()
    {
        var column = new GloSeedTableColumnMapping
        {
            Id = 3,
            TargetColumnName = SeedTransformationTypes.TargetColumns.CreatedBy,
            TransformationType = SeedTransformationTypes.SeedCreatedBy
        };

        TenantDataSeedingEngine.BuildSelectExpression(column)
            .Should().Be($"'{SeedTransformationTypes.SeedCreatedByValue}'");
    }

    [Fact]
    public void BuildSelectExpression_WithStaticCreatedBy_ReturnsDataSeedLiteral()
    {
        var column = new GloSeedTableColumnMapping
        {
            Id = 4,
            TargetColumnName = SeedTransformationTypes.TargetColumns.CreatedBy,
            TransformationType = SeedTransformationTypes.Static,
            StaticValue = "ignored"
        };

        TenantDataSeedingEngine.BuildSelectExpression(column)
            .Should().Be($"'{SeedTransformationTypes.SeedCreatedByValue}'");
    }

    [Fact]
    public void BuildSelectExpression_WithStaticOtherColumn_ReturnsStaticValueLiteral()
    {
        var column = new GloSeedTableColumnMapping
        {
            Id = 5,
            TargetColumnName = "Status",
            TransformationType = SeedTransformationTypes.Static,
            StaticValue = "Active"
        };

        TenantDataSeedingEngine.BuildSelectExpression(column).Should().Be("'Active'");
    }

    [Fact]
    public void BuildSelectExpression_WithCurrentTimestamp_ReturnsNowFunction()
    {
        var column = new GloSeedTableColumnMapping
        {
            Id = 6,
            TargetColumnName = "CreatedOn",
            TransformationType = SeedTransformationTypes.CurrentTimestamp
        };

        TenantDataSeedingEngine.BuildSelectExpression(column)
            .Should().Be(SeedTransformationTypes.SqlFunctions.CurrentTimestamp);
    }

    [Fact]
    public void BuildSelectExpression_WhenTransformationMissingAndNoSourceColumn_Throws()
    {
        var column = new GloSeedTableColumnMapping
        {
            Id = 7,
            TargetColumnName = "Code"
        };

        var act = () => TenantDataSeedingEngine.BuildSelectExpression(column);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage(string.Format(
                SeedTransformationTypes.ErrorMessages.SourceColumnRequiredFormat,
                7));
    }

    [Fact]
    public void BuildSelectExpression_WithUnsupportedTransformation_Throws()
    {
        var column = new GloSeedTableColumnMapping
        {
            Id = 8,
            TargetColumnName = "Code",
            TransformationType = "UNKNOWN"
        };

        var act = () => TenantDataSeedingEngine.BuildSelectExpression(column);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage(string.Format(
                SeedTransformationTypes.ErrorMessages.UnsupportedTransformationFormat,
                "UNKNOWN",
                8));
    }

    [Fact]
    public void QuoteIdentifier_EscapesEmbeddedQuotes()
    {
        TenantDataSeedingEngine.QuoteIdentifier("Col\"Name").Should().Be("\"Col\"\"Name\"");
    }

    [Theory]
    [InlineData(null, "NULL")]
    [InlineData("O'Brien", "'O''Brien'")]
    public void ToSqlLiteral_EscapesQuotesAndHandlesNull(string? value, string expected)
    {
        TenantDataSeedingEngine.ToSqlLiteral(value).Should().Be(expected);
    }
}
