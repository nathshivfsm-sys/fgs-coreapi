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
}
