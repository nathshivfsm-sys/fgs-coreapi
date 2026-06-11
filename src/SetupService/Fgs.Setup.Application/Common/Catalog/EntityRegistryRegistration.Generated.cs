using Fgs.Foundation.CatalogCrud;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Setup.Application.Features.Generated.Descriptors;

namespace Fgs.Setup.Application.Common.Catalog;

public static class EntityRegistryRegistration
{
    public static void RegisterAll(IEntityRegistry registry)
    {
        registry.Register(FgsBillingCategoryDescriptor.Create());
        registry.Register(FgsBusinessTypeDescriptor.Create());
        registry.Register(FgsInventoryCategoryDescriptor.Create());
        registry.Register(FgsInventoryItemDescriptor.Create());
        registry.Register(FgsInventoryItemAlternateDescriptor.Create());
        registry.Register(FgsInventoryItemDependencyDescriptor.Create());
        registry.Register(FgsInventoryItemTypeDescriptor.Create());
        registry.Register(FgsInventorySubCategoryDescriptor.Create());
        registry.Register(FgsJobTypeDescriptor.Create());
        registry.Register(FgsJobTypeCategoryDescriptor.Create());
        registry.Register(FgsJobTypeSubCategoryDescriptor.Create());
        registry.Register(FgsLeadSourceDescriptor.Create());
        registry.Register(FgsResolutionCodeDescriptor.Create());
        registry.Register(FgsSetupCommunicationTemplateDescriptor.Create());
        registry.Register(FgsSetupDescriptionDescriptor.Create());
        registry.Register(FgsSetupGLBreakDescriptor.Create());
        registry.Register(FgsSetupLaborRateTypeDescriptor.Create());
        registry.Register(FgsSetupPaymentMethodDescriptor.Create());
        registry.Register(FgsSetupPaymentTermDescriptor.Create());
        registry.Register(FgsSetupPostalCodeDescriptor.Create());
        registry.Register(FgsSetupPricingMatrixDescriptor.Create());
        registry.Register(FgsSetupPricingMatrixLaborDescriptor.Create());
        registry.Register(FgsSetupPricingMatrixLaborTierDescriptor.Create());
        registry.Register(FgsSetupPricingMatrixMaterialTierDescriptor.Create());
        registry.Register(FgsSetupPricingMatrixOtherDescriptor.Create());
        registry.Register(FgsSetupTaxDescriptor.Create());
        registry.Register(FgsSetupTaxAuthorityDescriptor.Create());
        registry.Register(FgsSetupTaxDetailDescriptor.Create());
        registry.Register(FgsSetupTechSkillLevelDescriptor.Create());
        registry.Register(FgsSetupTechTradeDescriptor.Create());
        registry.Register(FgsSetupTimeSlotDescriptor.Create());
        registry.Register(FgsSetupTitleOfCourtesyDescriptor.Create());
        registry.Register(FgsSetupZoneDescriptor.Create());
        registry.Register(FgsTagDescriptor.Create());
        registry.Register(FgsVehicleDescriptor.Create());
        registry.Register(FgsVehicleMaintenanceDescriptor.Create());
        registry.Register(FgsVendorDescriptor.Create());
        registry.Register(FgsVendorInventoryItemDescriptor.Create());
        registry.Register(FgsWarehouseDescriptor.Create());
    }
}
