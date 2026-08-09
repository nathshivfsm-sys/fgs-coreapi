-- Rollback for 20260730115450_AddTruckStockTemplates
DO $$
BEGIN
    DROP TABLE IF EXISTS inventory."FgsTruckStockTemplateItem";
    DROP TABLE IF EXISTS inventory."FgsTruckStockTemplate";
END $$;
