# CRM

- **Owner:** CrmService (`crm`)
- **Purpose:** Customers; domain also has leads, estimates, opportunities, activities
- **APIs:** `/api/v1/customer` (+ health)
- **AuthZ:** `CUSTOMER.*`
- **Outbox entity:** `CrmOutboxMessage` (publisher source exists)
- **Maturity:** Partial REST vs domain
