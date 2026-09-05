START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON TABLE billing."FgsInvoiceDetail" IS 'Stores individual invoice line items, including labor, service, equipment, material, and other billable items, along with pricing, cost, tax, accounting, technician, and source information.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON TABLE billing."FgsInvoiceBatch" IS 'Stores invoice batch records used to group and summarize invoices for a tenant and company.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."UpdatedOn" IS 'Date and time when the invoice detail line was last updated.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."UpdatedBy" IS 'Identifies the user who last updated the invoice detail line.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."UnitPrice" IS 'Sales price per unit, hour, or other quantity basis for the invoice line.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."UnitCost" IS 'Cost per unit, hour, or other quantity basis for the invoice line.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."TenantId" IS 'Identifies the tenant that owns the invoice detail.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."TechnicianId" IS 'Identifies the technician associated with the invoice line, when applicable.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."Quantity" IS 'Quantity used to calculate the extended cost and extended sales price of the invoice line. For labor, this represents the number of hours.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."PriceBookItemId" IS 'Identifies the Price Book item from which the invoice line was selected or populated, when applicable.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."ParentLineId" IS 'Identifies the parent invoice detail line when this line is associated with another invoice line, such as a child or related line.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."MasterPartNum" IS 'Master part number associated with the item when applicable.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."LineNumber" IS 'Sequential line number used to identify and order the detail lines within an invoice.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."LineAddedFromId" IS 'Identifies the specific source record from which the invoice line was added.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."LineAddedFrom" IS 'Identifies the type of source document or transaction from which the invoice line was added, such as an Estimate or Work Order.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."LaborRateTypeId" IS 'Identifies the labor rate type used to determine labor pricing when the invoice line is a labor item.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."ItemDescription" IS 'Description of the item, service, labor, or charge displayed on the invoice.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."ItemCode" IS 'Code identifying the service, material, equipment, or other item associated with the invoice line.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."IsTaxable" IS 'Indicates whether the invoice line is subject to applicable sales tax calculation.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."IsInventory" IS 'Indicates whether the invoice line represents an inventory item.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."InvoiceId" IS 'Identifies the invoice to which this detail line belongs.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."InventoryItemId" IS 'Identifies the inventory item associated with the invoice detail when the line represents an inventory item.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."GLBreak2Id" IS 'Identifies the second general ledger break or accounting classification assigned to the invoice line.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."GLBreak1Id" IS 'Identifies the first general ledger break or accounting classification assigned to the invoice line.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."ExtendedPrice" IS 'Total sales price of the invoice line calculated from the applicable quantity and unit price.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."ExtendedCost" IS 'Total cost of the invoice line calculated from the applicable quantity and unit cost.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."CreatedOn" IS 'Date and time when the invoice detail line was created.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."CreatedBy" IS 'Identifies the user who created the invoice detail line.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."CompanyId" IS 'Identifies the company within the tenant that owns the invoice detail.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."BillingCategoryId" IS 'Identifies the billing category that determines the type and behavior of the invoice line, such as Labor, Service, Equipment, Material, or Other.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."AddedSource" IS 'Identifies the source or mechanism through which the invoice line was added to the invoice.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."Id" IS 'Unique identifier for the invoice detail line.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceBatch"."UpdatedOn" IS 'Date and time when the invoice batch was last updated.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceBatch"."UpdatedBy" IS 'Identifies the user who last updated the invoice batch.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceBatch"."TotalTax" IS 'Total tax amount across all invoices included in the batch.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceBatch"."TenantId" IS 'Identifies the tenant that owns the invoice batch.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceBatch"."IsClosed" IS 'Indicates whether the invoice batch has been closed and is no longer available for further batch processing.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceBatch"."InvoiceTotal" IS 'Total invoice amount across all invoices included in the batch, including applicable taxes.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceBatch"."InvoiceSubtotal" IS 'Sum of the subtotals for all invoices included in the batch before tax.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceBatch"."InvoiceCount" IS 'Number of invoices included in the batch.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceBatch"."CreatedOn" IS 'Date and time when the invoice batch was created.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceBatch"."CreatedBy" IS 'Identifies the user who created the invoice batch.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceBatch"."CompanyId" IS 'Identifies the company within the tenant that owns the invoice batch.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceBatch"."ClosedOn" IS 'Date and time when the invoice batch was closed.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceBatch"."ClosedBy" IS 'Identifies the user who closed the invoice batch.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceBatch"."BatchNumber" IS 'Unique batch number used to identify the invoice batch within the tenant and company.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceBatch"."BatchDate" IS 'Date assigned to the invoice batch.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON COLUMN billing."FgsInvoiceBatch"."Id" IS 'Unique identifier for the invoice batch.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    INSERT INTO billing."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260817165451_AddInvoiceDetailAndBatchComments', '10.0.8');
    END IF;
END $EF$;
COMMIT;

