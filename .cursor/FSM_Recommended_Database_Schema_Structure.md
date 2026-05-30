# Recommended FSM Database Schema Structure

This document outlines the recommended enterprise database schema structure for FSM.

## Schema Summary

| Schema | Purpose |
|---|---|
| glo | Global platform-wide configuration and master content |
| identity | Authentication, authorization, tokens, sessions, and security |
| tenant | Tenant/company/account management |
| crm | Customers, leads, contacts, and customer relationships |
| dispatch | Jobs, scheduling, technicians, appointments, and work orders |
| billing | Estimates, invoices, payments, GL, accounting, and financial data |
| inventory | Inventory, warehouses, vendors, purchasing, and stock |
| shared | Shared reusable infrastructure/domain-neutral entities |
| audit | Auditing, history tracking, and change logging |
| integration | Third-party integrations and external system connectivity |
| reporting | Reporting/materialized views/analytics tables |
| automation | Workflow automation, background jobs, and rules engine |
| notification | Email, SMS, push notifications, templates, and delivery tracking |

---

## glo Schema

**Purpose:** Global platform-wide configuration and master content

### Stores

- Global configuration
- Feature flags
- Terms & conditions
- Privacy policy
- System templates
- Global lookup/reference tables
- Platform branding defaults

---

## identity Schema

**Purpose:** Authentication, authorization, and security domain

### Stores

- Users
- Roles and permissions
- API keys
- Sessions
- Password reset tokens
- SMS/email verification tokens
- Invite links
- MFA/security data

---

## tenant Schema

**Purpose:** Tenant/account/company management

### Stores

- Tenants
- Companies
- Subscription plans
- Tenant onboarding
- Ownership transfers
- Subscription lifecycle management

---

## crm Schema

**Purpose:** Customer relationship management

### Stores

- Customers
- Contacts
- Leads
- Customer locations
- Customer communication history
- Customer preferences and tags

---

## dispatch Schema

**Purpose:** Operational dispatching and field service workflows

### Stores

- Jobs
- Work orders
- Appointments
- Scheduling
- Technician assignments
- Routes
- Time tracking
- Field checklists

---

## billing Schema

**Purpose:** Accounting and financial domain

### Stores

- Estimates
- Invoices
- Payments
- Taxes
- GL accounts
- GL breaks
- Billing categories
- Financial reporting data

---

## inventory Schema

**Purpose:** Inventory and procurement management

### Stores

- Inventory items
- Warehouses
- Stock transactions
- Vendors
- Purchase orders
- Receipts
- Inventory adjustments
- Transfers

---

## shared Schema

**Purpose:** Shared reusable infrastructure/domain-neutral entities

### Stores

- Files and attachments
- Locations/addresses
- Notes
- Tags
- Outbox messages
- Notifications
- Comments
- Cross-domain reusable entities

---

## audit Schema

**Purpose:** Audit trails and historical tracking

### Stores

- Audit logs
- Entity history
- Security events
- Login audits
- Compliance records

---

## integration Schema

**Purpose:** Third-party integration management

### Stores

- QuickBooks integration
- Stripe integration
- Twilio integration
- Webhook tracking
- Sync jobs
- External system credentials

---

## reporting Schema

**Purpose:** Analytics and reporting optimization

### Stores

- Materialized views
- Reporting snapshots
- Analytics aggregations
- Denormalized reporting datasets

---

## automation Schema

**Purpose:** Workflow automation and background processing

### Stores

- Automation rules
- Workflow definitions
- Scheduled jobs
- Async processing
- Business process automation

---

## notification Schema

**Purpose:** Messaging and customer communication domain

### Stores

- Email messages
- SMS messages
- Push notifications
- Notification templates
- Delivery tracking
- Retry handling

---

# Architecture Guidelines

- Schemas should represent business domains and bounded contexts.
- All schema creation should be managed through EF Core migrations.
- Use schema-based organization instead of large dbo prefixes.
- Shared infrastructure entities should live in the shared schema.
- Security and authentication entities should live in the identity schema.
- Global platform-wide settings should live in the glo schema.
- Reporting schema should contain read-optimized and denormalized structures only.
