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

        TenantSeedSqlBuilder.BuildSelectExpression(column).Should().Be("\"Code\"");
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

        TenantSeedSqlBuilder.BuildSelectExpression(column).Should().Be(expected);
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

        TenantSeedSqlBuilder.BuildSelectExpression(column)
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

        TenantSeedSqlBuilder.BuildSelectExpression(column)
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

        TenantSeedSqlBuilder.BuildSelectExpression(column).Should().Be("'Active'");
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

        TenantSeedSqlBuilder.BuildSelectExpression(column)
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

        var act = () => TenantSeedSqlBuilder.BuildSelectExpression(column);

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

        var act = () => TenantSeedSqlBuilder.BuildSelectExpression(column);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage(string.Format(
                SeedTransformationTypes.ErrorMessages.UnsupportedTransformationFormat,
                "UNKNOWN",
                8));
    }

    [Fact]
    public void BuildBusinessTypeFilterClause_WhenNullable_IncludesNullRows()
    {
        var clause = TenantSeedSqlBuilder.BuildBusinessTypeFilterClause(
            sourceHasBusinessTypeId: true,
            businessTypeColumnIsNullable: true,
            hasBusinessTypeFilter: true);

        clause.Should().Be("(\"BusinessTypeId\" = ANY(@businessTypeIds) OR \"BusinessTypeId\" IS NULL)");
    }

    [Fact]
    public void BuildBusinessTypeFilterClause_WhenRequired_UsesAnyOnly()
    {
        var clause = TenantSeedSqlBuilder.BuildBusinessTypeFilterClause(
            sourceHasBusinessTypeId: true,
            businessTypeColumnIsNullable: false,
            hasBusinessTypeFilter: true);

        clause.Should().Be("\"BusinessTypeId\" = ANY(@businessTypeIds)");
    }

    [Fact]
    public void QuoteIdentifier_EscapesEmbeddedQuotes()
    {
        TenantSeedSqlBuilder.QuoteIdentifier("Col\"Name").Should().Be("\"Col\"\"Name\"");
    }

    [Theory]
    [InlineData(null, "NULL")]
    [InlineData("O'Brien", "'O''Brien'")]
    public void ToSqlLiteral_EscapesQuotesAndHandlesNull(string? value, string expected)
    {
        TenantSeedSqlBuilder.ToSqlLiteral(value).Should().Be(expected);
    }
}
