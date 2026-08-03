using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Provisioning;

namespace Fgs.Setup.Tests.Provisioning;

public sealed class TenantSeedSqlBuilderTests
{
    [Fact]
    public void CombineWhereClauses_WithSourceFilter_AppendsAssignablePredicate()
    {
        var mapping = new GloSeedTableMapping
        {
            SeedCode = "ALL_GloRole"
        };

        var clause = TenantSeedSqlBuilder.CombineWhereClauses(
            TenantSeedSqlBuilder.BuildSourceFilterClause(mapping));

        clause.Should().Be("\"IsAssignable\" = true AND \"IsActive\" = true");
    }

    [Fact]
    public void BuildInsertSelectSql_WithSourceFilter_IncludesWhereClause()
    {
        var mapping = new GloSeedTableMapping
        {
            SeedCode = "ALL_GloRole"
        };

        var columns = new List<GloSeedTableColumnMapping>
        {
            new()
            {
                TargetColumnName = "TenantId",
                TransformationType = "TENANT_ID",
                ColumnOrder = 1
            },
            new()
            {
                SourceColumnName = "RoleCode",
                TargetColumnName = "RoleCode",
                ColumnOrder = 2
            }
        };

        var sql = TenantSeedSqlBuilder.BuildInsertSelectSql(
            TenantSeedSqlBuilder.QualifyTable("identity", "FgsRole"),
            TenantSeedSqlBuilder.QualifyTable("glo", "GloRole"),
            columns,
            TenantSeedSqlBuilder.CombineWhereClauses(
                TenantSeedSqlBuilder.BuildSourceFilterClause(mapping)));

        sql.Should().Contain("FROM \"glo\".\"GloRole\"");
        sql.Should().Contain("WHERE \"IsAssignable\" = true AND \"IsActive\" = true");
        sql.Should().Contain("INSERT INTO \"identity\".\"FgsRole\"");
        sql.Should().Contain("ON CONFLICT DO NOTHING");
    }

    [Fact]
    public void BuildInsertSelectSql_AlwaysUsesOnConflictDoNothing()
    {
        var columns = new List<GloSeedTableColumnMapping>
        {
            new()
            {
                TargetColumnName = "TenantId",
                TransformationType = "TENANT_ID",
                ColumnOrder = 1
            },
            new()
            {
                SourceColumnName = "TagCode",
                TargetColumnName = "TagCode",
                ColumnOrder = 2
            }
        };

        var sql = TenantSeedSqlBuilder.BuildInsertSelectSql(
            TenantSeedSqlBuilder.QualifyTable("setup", "FgsTag"),
            TenantSeedSqlBuilder.QualifyTable("glo", "GloTag"),
            columns,
            additionalWhereClause: null);

        sql.TrimEnd().Should().EndWith("ON CONFLICT DO NOTHING");
    }

    [Fact]
    public void BuildSourceFilterClause_WithUnknownSeedCode_ReturnsNull()
    {
        var mapping = new GloSeedTableMapping
        {
            SeedCode = "ALL_GloTag"
        };

        TenantSeedSqlBuilder.BuildSourceFilterClause(mapping).Should().BeNull();
    }
}
