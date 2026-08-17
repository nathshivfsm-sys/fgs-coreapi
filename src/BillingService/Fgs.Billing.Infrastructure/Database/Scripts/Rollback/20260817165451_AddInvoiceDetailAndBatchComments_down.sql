START TRANSACTION;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    COMMENT ON TABLE billing."FgsInvoiceDetail" IS NULL;
    COMMENT ON TABLE billing."FgsInvoiceBatch" IS NULL;

    COMMENT ON COLUMN billing."FgsInvoiceDetail"."TenantId" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."CompanyId" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."Id" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."InvoiceId" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."ParentLineId" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."LineNumber" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."BillingCategoryId" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."ItemCode" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."ItemDescription" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."IsInventory" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."MasterPartNum" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."InventoryItemId" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."PriceBookItemId" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."LaborRateTypeId" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."TechnicianId" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."Quantity" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."UnitCost" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."ExtendedCost" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."UnitPrice" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."ExtendedPrice" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."IsTaxable" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."GLBreak1Id" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."GLBreak2Id" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."LineAddedFrom" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."LineAddedFromId" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."AddedSource" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."CreatedOn" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."CreatedBy" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."UpdatedOn" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceDetail"."UpdatedBy" IS NULL;

    COMMENT ON COLUMN billing."FgsInvoiceBatch"."TenantId" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceBatch"."CompanyId" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceBatch"."Id" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceBatch"."BatchNumber" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceBatch"."BatchDate" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceBatch"."InvoiceCount" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceBatch"."InvoiceSubtotal" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceBatch"."TotalTax" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceBatch"."InvoiceTotal" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceBatch"."IsClosed" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceBatch"."ClosedOn" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceBatch"."ClosedBy" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceBatch"."CreatedOn" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceBatch"."CreatedBy" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceBatch"."UpdatedOn" IS NULL;
    COMMENT ON COLUMN billing."FgsInvoiceBatch"."UpdatedBy" IS NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM billing."__EFMigrationsHistory" WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments') THEN
    DELETE FROM billing."__EFMigrationsHistory"
    WHERE "MigrationId" = '20260817165451_AddInvoiceDetailAndBatchComments';
    END IF;
END $EF$;
COMMIT;
