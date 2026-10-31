# Inventory

- **Owner:** InventoryService (`inventory`)
- **Purpose:** Items, categories, vendors, stock, serials, POs, truck stock, fleet vehicles
- **APIs:** `/api/v1/inventoryitem`, vendor, location, stock, purchaseorder, truckstocktemplate, vehicle, vehiclemaintenance, …
- **AuthZ:** `INVENTORYITEM.*` (including vehicle create/edit)
- **Outbox:** `InventoryOutboxMessage`
- **Vehicles:** `FgsVehicle` / `FgsVehicleMaintenance` live in `inventory` (moved from Setup). `InventoryLocationId` is an EF FK to `FgsInventoryLocation`. `VehicleMaintenanceTypeId` remains a scalar to `glo.GloVehicleMaintenanceType` (Setup Glo lookup APIs unchanged).
- **Note:** Setup historically seeded inventory — avoid new cross-schema writes
- **Clone:** InventoryItem / TruckStockTemplate features/controllers
