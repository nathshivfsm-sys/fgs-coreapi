# Inventory

- **Owner:** InventoryService (`inventory`)
- **Purpose:** Items, categories, vendors, stock, serials, POs, truck stock
- **APIs:** `/api/v1/inventoryitem`, vendor, location, stock, purchaseorder, …
- **AuthZ:** `INVENTORYITEM.*`
- **Outbox:** `InventoryOutboxMessage`
- **Note:** Setup historically seeded inventory — avoid new cross-schema writes
- **Clone:** InventoryItem features/controllers
