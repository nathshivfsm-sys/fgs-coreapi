namespace Fgs.ServiceAgreement.Domain.Enums;

public enum BillingScheduleStatus : short
{
    Pending = 1,
    InvoiceCreated = 2,
    Invoiced = 3,
    Skipped = 4,
    Cancelled = 5
}
