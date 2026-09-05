# CRM

- **Owner:** CrmService (`crm`)
- **Purpose:** Customers; domain also has leads, estimates, opportunities, activities
- **APIs:** `/api/v1/customer` (+ health)
- **AuthZ:** `CUSTOMER.*`
- **Outbox entity:** `CrmOutboxMessage` (no worker yet; wire via `AddFgsOutboxPublisher` when adding a writer)
- **Maturity:** Partial REST vs domain
