namespace Fgs.Audit.Domain.Enums;

/// <summary>
/// Defines the type of business record associated with an event.
/// Used together with EntityId to identify the audited record.
/// </summary>
public enum AuditRecordType
{
    CUSTOMER,
    CONTACT,
    LOCATION,
    CALL,
    WORK_ORDER,
    APPOINTMENT,
    ESTIMATE,
    INVOICE,
    PAYMENT,
    ASSET,
    CONTRACT,
    INVENTORY_ITEM,
    PURCHASE_ORDER,
    TECHNICIAN,
    TASK,
    USER,
    PRICEBOOK,
    JOB_TYPE,
    ATTACHMENT,
    NOTE,
    SYSTEM
}
