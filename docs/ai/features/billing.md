# Billing

- **Owner:** BillingService (`billing`)
- **Purpose:** Invoices and payments (domain richer than HTTP)
- **Entities:** `FgsInvoice*`, `FgsPayment*`, …
- **APIs:** `/api/v1/invoice` (+ health)
- **AuthZ:** `INVOICE.*`
- **Maturity:** Partial — extend carefully; clone Invoice features before inventing APIs
