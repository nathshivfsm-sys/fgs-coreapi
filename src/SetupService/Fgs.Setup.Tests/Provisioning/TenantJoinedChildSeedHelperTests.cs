using Fgs.Setup.Application.Features.TenantProvisioning;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Database.Schemas;
using Fgs.Setup.Infrastructure.Provisioning;

namespace Fgs.Setup.Tests.Provisioning;

public sealed class TenantJoinedChildSeedHelperTests
{
    [Fact]
    public void SelectReferenceMapping_PrefersGloToTargetSchema_OverTenantCacheMappings()
    {
        var mappings = new List<GloSeedTableMapping>
        {
            new()
            {
                Id = 1,
                SeedCode = "TENANT_FgsTenantCompany_setup_cache",
                SeedOrder = 1,
                SourceSchemaName = "tenant",
                SourceTableName = "FgsTenantCompany",
                TargetSchemaName = FgsDatabaseSchemas.Setup,
                TargetTableName = "FgsTenantCompanyCache",
                SourceDatabaseName = "fgs_dev_db",
                TargetDatabaseName = "fgs_dev_db",
            },
            new()
            {
                Id = 2,
                SeedCode = "GLO_INVENTORY_CATEGORY_TO_FGS_INVENTORY_CATEGORY",
                SeedOrder = 105,
                SourceSchemaName = FgsDatabaseSchemas.Glo,
                SourceTableName = "GloInventoryCategory",
                TargetSchemaName = FgsDatabaseSchemas.Inventory,
                TargetTableName = "FgsInventoryCategory",
                SourceDatabaseName = "fgs_dev_db",
                TargetDatabaseName = "fgs_dev_db",
            },
            new()
            {
                Id = 3,
                SeedCode = "ALL_GloUniversalPricingService",
                SeedOrder = 510,
                SourceSchemaName = FgsDatabaseSchemas.Glo,
                SourceTableName = "GloUniversalPricingService",
                TargetSchemaName = FgsDatabaseSchemas.Setup,
                TargetTableName = "FgsUniversalPricingService",
                SourceDatabaseName = "fgs_dev_db",
                TargetDatabaseName = "fgs_dev_db",
            },
        };

        var inventoryRef = TenantJoinedChildSeedHelper.SelectReferenceMapping(
            mappings,
            FgsDatabaseSchemas.Inventory);
        inventoryRef.Should().NotBeNull();
        inventoryRef!.SeedCode.Should().Be("GLO_INVENTORY_CATEGORY_TO_FGS_INVENTORY_CATEGORY");
        inventoryRef.SourceSchemaName.Should().Be(FgsDatabaseSchemas.Glo);

        var setupRef = TenantJoinedChildSeedHelper.SelectReferenceMapping(
            mappings,
            FgsDatabaseSchemas.Setup);
        setupRef.Should().NotBeNull();
        setupRef!.SeedCode.Should().Be("ALL_GloUniversalPricingService");
        setupRef.SourceSchemaName.Should().Be(FgsDatabaseSchemas.Glo);
    }

    [Fact]
    public void SelectReferenceMapping_FallsBackToAnyGloMapping_WhenTargetSchemaAbsent()
    {
        var mappings = new List<GloSeedTableMapping>
        {
            new()
            {
                Id = 1,
                SeedCode = "TENANT_cache",
                SeedOrder = 1,
                SourceSchemaName = "tenant",
                TargetSchemaName = FgsDatabaseSchemas.Setup,
            },
            new()
            {
                Id = 2,
                SeedCode = "ALL_GloBillingCategory",
                SeedOrder = 100,
                SourceSchemaName = FgsDatabaseSchemas.Glo,
                TargetSchemaName = FgsDatabaseSchemas.Setup,
            },
        };

        var result = TenantJoinedChildSeedHelper.SelectReferenceMapping(
            mappings,
            FgsDatabaseSchemas.Inventory);

        result.Should().NotBeNull();
        result!.SeedCode.Should().Be("ALL_GloBillingCategory");
    }

    [Fact]
    public void SelectReferenceMapping_ReturnsNull_WhenMappingsEmpty()
    {
        TenantJoinedChildSeedHelper.SelectReferenceMapping([], FgsDatabaseSchemas.Inventory)
            .Should()
            .BeNull();
    }

    [Fact]
    public void BuildRolePermissionSameDatabaseSql_RemapsRoleAndPermissionByCode()
    {
        var sql = TenantJoinedChildSeedHelper.BuildRolePermissionSameDatabaseSql(
            FgsDatabaseSchemas.Glo,
            FgsDatabaseSchemas.Identity);

        sql.Should().Contain("INSERT INTO \"identity\".\"FgsRolePermission\"");
        sql.Should().Contain("FROM \"glo\".\"GloRolePermission\" grp");
        sql.Should().Contain("INNER JOIN \"glo\".\"GloRole\" gr ON gr.\"Id\" = grp.\"RoleId\"");
        sql.Should().Contain("INNER JOIN \"glo\".\"GloPermission\" gp ON gp.\"Id\" = grp.\"PermissionId\"");
        sql.Should().Contain("AND fr.\"RoleCode\" = gr.\"RoleCode\"");
        sql.Should().Contain("ON fp.\"PermissionCode\" = gp.\"PermissionCode\"");
        sql.Should().Contain("WHERE grp.\"IsActive\" = true");
        sql.Should().Contain("existing.\"FgsRoleId\" = fr.\"Id\"");
        sql.Should().Contain("existing.\"FgsPermissionId\" = fp.\"Id\"");
        sql.Should().NotContain("JOINED_PARENT");
    }

    [Fact]
    public void SelectReferenceMapping_PrefersGloToIdentity_ForRolePermissionSoftPath()
    {
        var mappings = new List<GloSeedTableMapping>
        {
            new()
            {
                Id = 1,
                SeedCode = "TENANT_FgsTenantCompany_identity_cache",
                SeedOrder = 2,
                SourceSchemaName = "tenant",
                TargetSchemaName = FgsDatabaseSchemas.Identity,
            },
            new()
            {
                Id = 2,
                SeedCode = "ALL_GloRole",
                SeedOrder = 15,
                SourceSchemaName = FgsDatabaseSchemas.Glo,
                SourceTableName = "GloRole",
                TargetSchemaName = FgsDatabaseSchemas.Identity,
                TargetTableName = "FgsRole",
            },
            new()
            {
                Id = 3,
                SeedCode = SeedTransformationTypes.SeedCodes.AllGloRolePermission,
                SeedOrder = 17,
                SourceSchemaName = FgsDatabaseSchemas.Glo,
                SourceTableName = "GloRolePermission",
                TargetSchemaName = FgsDatabaseSchemas.Identity,
                TargetTableName = "FgsRolePermission",
            },
        };

        var identityRef = TenantJoinedChildSeedHelper.SelectReferenceMapping(
            mappings,
            FgsDatabaseSchemas.Identity);

        identityRef.Should().NotBeNull();
        identityRef!.SourceSchemaName.Should().Be(FgsDatabaseSchemas.Glo);
        identityRef.SeedCode.Should().BeOneOf("ALL_GloRole", SeedTransformationTypes.SeedCodes.AllGloRolePermission);
    }
}
