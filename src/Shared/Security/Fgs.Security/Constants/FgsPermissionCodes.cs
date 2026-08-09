namespace Fgs.Security.Constants;

/// <summary>
/// Well-known permission codes seeded in UserService (<c>FgsPermission_Seed.sql</c>).
/// </summary>
public static class FgsPermissionCodes
{
    public const string SetupView = "SETUP.VIEW";
    public const string SetupCreate = "SETUP.CREATE";
    public const string SetupEdit = "SETUP.EDIT";
    public const string SetupDelete = "SETUP.DELETE";

    public const string UserView = "USER.VIEW";
    public const string UserCreate = "USER.CREATE";
    public const string UserEdit = "USER.EDIT";
    public const string UserDelete = "USER.DELETE";

    public const string AssetView = "ASSET.VIEW";
    public const string AssetCreate = "ASSET.CREATE";
    public const string AssetEdit = "ASSET.EDIT";
    public const string AssetDelete = "ASSET.DELETE";

    public const string InventoryItemView = "INVENTORYITEM.VIEW";
    public const string InventoryItemCreate = "INVENTORYITEM.CREATE";
    public const string InventoryItemEdit = "INVENTORYITEM.EDIT";
    public const string InventoryItemDelete = "INVENTORYITEM.DELETE";

    public const string CustomerView = "CUSTOMER.VIEW";
    public const string CustomerCreate = "CUSTOMER.CREATE";
    public const string CustomerEdit = "CUSTOMER.EDIT";
    public const string CustomerDelete = "CUSTOMER.DELETE";

    public const string InvoiceView = "INVOICE.VIEW";
    public const string InvoiceCreate = "INVOICE.CREATE";
    public const string InvoiceEdit = "INVOICE.EDIT";
    public const string InvoiceDelete = "INVOICE.DELETE";

    public const string ServiceAgreementView = "SERVICEAGREEMENT.VIEW";
    public const string ServiceAgreementCreate = "SERVICEAGREEMENT.CREATE";
    public const string ServiceAgreementEdit = "SERVICEAGREEMENT.EDIT";
    public const string ServiceAgreementDelete = "SERVICEAGREEMENT.DELETE";
}
