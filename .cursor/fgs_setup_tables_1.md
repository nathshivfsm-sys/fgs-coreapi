# FSM (Field Good Service) — Final Setup & Global Tables Specification

## Official Naming Convention

### Global Tables

Use:

```text
Glo<TableName>
```

Examples:
- GloCountry
- GloStateProvince
- GloAccountingIntegrationType
- GloBusinessType
- GloTimeCardOption
- GloLanguage
- GloMasterEntityType
- GloLocationType
- GloCredentialCategory
- GloCredentialProviderType

---

### Tenant Setup Tables

Use:

```text
FgsSetup<TableName>
```

Examples:
- FgsSetupZone
- FgsSetupPriceSheet
- FgsSetupPaymentMethod

---

## Naming Rules

- Use PascalCase for ALL tables
- Use PascalCase for ALL columns
- Use singular table names
- Use BIGSERIAL surrogate primary keys
- Avoid composite primary keys except junction tables
- Use TenantId and CompanyId on tenant tables only
- Global tables DO NOT use TenantId or CompanyId

---

## Future Convention Requirement

All future tables in this FSM platform must follow:

- PascalCase naming
- Glo prefix for global tables
- FgsSetup prefix for tenant setup tables

### FGS vs table names

The **FGS** acronym (Field Good Service) is used for the product, solution names, and the PostgreSQL schema name **`fgs`**. **Table names** use the **`Glo*`** and **`FgsSetup*`** prefixes in PascalCase as above (do not spell setup tables as `FSGSetup*`).

---

# GLOBAL TABLES

---

# GloCountry

Purpose:
Stores supported countries.

## Columns

| Column | Type |
|---|---|
| GloCountryId | BIGSERIAL PK |
| CountryCode | VARCHAR(10) |
| CountryName | VARCHAR(200) |
| IsActive | BOOLEAN |
| CreatedOn | TIMESTAMP |

---

# GloStateProvince

Purpose:
Stores states/provinces associated with countries.

## Columns

| Column | Type |
|---|---|
| GloStateProvinceId | BIGSERIAL PK |
| GloCountryId | BIGINT FK |
| RegionCode | VARCHAR(25) |
| RegionName | VARCHAR(200) |
| IsActive | BOOLEAN |
| CreatedOn | TIMESTAMP |

---

# GloAccountingIntegrationType

Purpose:
Global catalog of accounting system / package choices (e.g. none, QuickBooks Online, Sage Intacct). Tenant and company **selection** is stored on `FgsTenantCompanyConfiguration.AccountingIntegrationTypeId` referencing this table.

## Columns

| Column | Type |
|---|---|
| Id | BIGSERIAL PK |
| Code | VARCHAR(100) | Unique code (e.g. NONE, QUICKBOOKSONLINE) |
| Name | VARCHAR(200) | Display name |
| IsActive | BOOLEAN |
| CreatedOn | TIMESTAMP |

---

# GloMasterEntityType

Purpose:
**Single global master catalog** for all **entity kinds** in the FSM platform. Rows describe both (1) **structural / polymorphic** kinds used with an **`EntityCode`** (e.g. `FgsLocation.MasterEntityTypeId` → this table), and (2) **business / feature** entity kinds used for metadata-driven features (communication templates, documents, media, workflows).

## Columns

| Column | Type | Notes |
|---|---|---|
| Id | INT PK (identity or serial) | Referenced as **`INT`** from `FgsLocation.MasterEntityTypeId` in current platform DDL |
| Code | VARCHAR(100) | Unique system code (e.g. `TENANT`, `CUSTOMER`, `WORKORDER`) |
| Name | VARCHAR(200) | Display name (often same as `Code` where appropriate) |
| SupportsDocuments | BOOLEAN | Default false; true when this entity kind participates in document features |
| SupportsMedia | BOOLEAN | Default false; true when this entity kind participates in media features |
| SupportsCommunication | BOOLEAN | Default false; true when this entity kind participates in communication templates |
| IsActive | BOOLEAN | |
| CreatedOn | TIMESTAMP | UTC |
| UpdatedOn | TIMESTAMP | UTC, nullable |

## Example codes

Structural / link kinds: `TENANT`, `COMPANY`, `SERVICELOCATION`, `BILLTO`, `VENDOR`, `SUBCONTRACTOR`, `LEAD`, `PROPOSAL`, `CUSTOMER`, `WORKORDER`, `INVOICE` (extend as needed).

Business / feature anchor examples: `Customer`, `WorkOrder`, `Invoice`, `Proposal`, `PurchaseOrder`, `ServiceAsset`, `Payment`, `Technician`, `JobBooking` (normalize to **`Code`**/`Name` in implementation).

---

# TECHNICIAN & DISPATCH SETUP

---

# FgsSetupTechTrade

Purpose:
Stores technician trades.

Examples:
- HVAC
- Plumbing
- Electrical

## Key Features

- Unique trade codes
- Used for dispatching
- Used for capacity planning
- Trade-level filtering

---

# FgsSetupTechSkillLevel

Purpose:
Stores technician skill levels.

Examples:
- Helper
- Journeyman
- Master

## Key Features

- Independent from trade
- Supports labor pricing
- Supports technician qualification

---

# FgsSetupTimeSlot

Purpose:
Stores scheduling/dispatch time slots.

## Key Features

- BeginTime
- EndTime
- MarkTechArrivedLateAfter
- MarkWorkOrderDelayedCompletionAfter
- Capacity planning support
- Mobile visibility
- Customer portal visibility

---

# GEOGRAPHIC SETUP

---

# FgsSetupZone

Purpose:
Stores dispatch/service zones.

## Key Features

- Geographic organization
- Dispatch filtering
- Service territory management

---

# FgsSetupPostalCode

Purpose:
Maps ZIP/postal codes.

## Key Features

- Compatible with US ZIPs
- Compatible with Canadian postal codes
- Zone assignment
- Tax assignment
- Trip charge assignment

---

# TAX SETUP

---

# FgsSetupTax

Purpose:
Stores tax definitions.

## Key Features

- Tax code setup
- Effective-dated rates
- Multi-region support
- US & Canada support

---

# FgsSetupTaxAuthority

Purpose:
Stores tax authorities/jurisdictions.

## Key Features

- City tax
- County tax
- State/province tax
- RegionCode support

---

# FgsSetupTaxDetail

Purpose:
Stores effective-dated tax percentage breakdowns.

## Key Features

- Multiple authorities per tax
- EffectiveFromDate
- EffectiveToDate
- PostgreSQL exclusion constraint support
- Prevents overlapping active dates

---

# CUSTOMER & DESCRIPTION SETUP

---

# FgsSetupTitleOfCourtesy

Purpose:
Stores courtesy titles.

Examples:
- Mr
- Mrs
- Dr
- Ms

---

# FgsSetupDescription

Purpose:
Stores reusable standard descriptions.

## DescriptionTypeCode Examples

- REASONFORCALL
- SERVICESUMMARY
- SERVICEAGREEMENTNOTE
- SERVICERECOMMENDATION

## Key Features

- Trade filtering support
- Sort ordering
- Standardized technician notes

---

# SERVICE ASSET SETUP

---

# FgsSetupServiceAssetType

Purpose:
Stores service asset/equipment types.

Examples:
- Furnace
- Water Heater
- RTU
- Generator

---

# FgsSetupServiceAssetManufacturer

Purpose:
Stores manufacturers.

Examples:
- Carrier
- Trane
- Rheem

---

# FgsSetupServiceAssetModelSerialDescription

Purpose:
Stores model/serial descriptions.

## Key Features

- Model descriptions
- Standardized naming
- Service asset metadata

---

# FgsSetupServiceAssetMedia

Purpose:
Stores media/documents associated with service assets.

## Key Features

- Photos
- PDFs
- Installation documents
- Equipment media

---

# PRICING SETUP

---

# FgsSetupPriceSheet

Purpose:
Master pricing sheet configuration.

## Key Features

- Effective date range
- Mobile visibility
- Labor/material/other linkage

---

# FgsSetupPriceSheetLabor

Purpose:
Stores labor pricing definitions.

## Key Features

- Skill-level pricing
- Rate types
- Discounts
- Regular/Overtime/Doubletime support

---

# FgsSetupPriceSheetLaborTier

Purpose:
Stores labor pricing tiers.

## Key Features

- Minute-based billing
- Flexible duration structure
- Progressive pricing

Example:
- First 60 minutes
- Next 30 minutes
- Next 15 minutes
- Every minute after

---

# FgsSetupPriceSheetMaterial

Purpose:
Stores material pricing calculator configuration.

## Key Features

- Discount support
- T&M pricing engine
- Material markup calculation

---

# FgsSetupPriceSheetMaterialRange

Purpose:
Stores material markup ranges.

## Key Features

- Range-based markup
- Cost-based pricing
- Contractor pricing support

---

# FgsSetupPriceSheetOther

Purpose:
Stores pricing rules for miscellaneous billing categories.

Examples:
- Subcontractor
- Permit
- Disposal
- Rental
- Shipping

## Key Features

- Simple markup
- Discount support
- No tier engine

---

# ACCOUNTING & ORGANIZATIONAL SETUP

---

# FgsSetupGLBreak

Purpose:
Stores organizational/accounting GL break structure.

## Key Features

- Break 1 / Break 2 support
- Trade association
- Optional location association
- Optional logo association
- Organizational filtering

## Example Uses

- HVAC Division
- Plumbing Division
- Commercial Division

---

# COMMUNICATION SETUP

---

# FgsSetupCommunicationTemplate

Purpose:
Stores communication templates.

## Supported Types

- EMAIL
- SMS

## Example Uses

- Invoice email
- Dispatch notification
- Booking confirmation
- Payment request
- Survey request
- Marketing campaign

## Key Features

- Merge token support
- Multiple ReplyToEmails
- Mobile visibility
- Entity-driven templates

## Branding Logic

Logo resolution order:

1. GL Break Logo
2. Company Logo
3. Tenant Logo

---

# FgsSetupCommunicationToken

Purpose:
Stores dynamic merge-field tokens.

## Example Tokens

- CustomerName
- InvoiceNumber
- PaymentLink
- ETA
- BookingDate

## Key Features

- Metadata-driven communications
- Dynamic token expansion
- Future low-code extensibility

---

# PAYMENT & TERMS SETUP

---

# FgsSetupPaymentMethod

Purpose:
Stores company-supported payment methods.

## PaymentMethodType Examples

- CREDITCARD
- DIGITALWALLET
- ACH
- CASH
- CHECK
- AR
- FINANCING

## Example Payment Methods

- Credit Card
- Apple Pay
- Google Pay
- ACH
- Cash
- Check
- Accounts Receivable
- Financing

## Key Features

- Mobile visibility
- Customer portal visibility
- Company-specific payment configuration

---

# FgsSetupPaymentTerm

Purpose:
Stores AR/AP payment terms and due-date calculation rules.

## Example Terms

- Due On Receipt
- COD
- Net 15
- Net 30
- Net 60
- End Of Month

## DueDateMethod Examples

- DUE_ON_RECEIPT
- DAYS_AFTER_INVOICE_DATE
- END_OF_MONTH
- FIXED_DAY_NEXT_MONTH

## Key Features

- Due-date calculation methods
- NumberOfDays support
- AR support
- AP support
- Mobile visibility
- Flexible commercial billing rules

---

# OVERALL PLATFORM ARCHITECTURE NOTES

## Architectural Direction

The FSM platform is designed as:

- metadata-driven
- highly configurable
- tenant-aware
- scalable
- mobile-first
- communication-enabled
- future automation ready

---

## Key Design Principles

### 1. Global vs Tenant Separation

Global:
- platform metadata
- shared reference data

Tenant:
- customer business configuration
- operational setup

---

### 2. Surrogate Primary Keys

Use:

```text
BIGSERIAL PRIMARY KEY
```

instead of composite primary keys.

Reasons:
- smaller foreign keys
- simpler joins
- cleaner APIs
- ORM compatibility
- better scalability

---

### 3. Communication Engine

Supports:
- email
- SMS
- customer notifications
- booking workflows
- marketing campaigns
- surveys
- future automation

---

### 4. T&M Pricing Engine

Supports:
- labor tiers
- minute billing
- material markup
- discounts
- overtime
- doubletime
- flexible pricing rules

---

### 5. Geographic Support

Supports:
- USA
- Canada
- ZIP codes
- Postal codes
- taxes
- dispatch zones

---

### 6. Future Expansion Ready

Architecture supports future:
- customer portal
- AI dispatching
- booking widgets
- workflow engine
- document management
- media management
- accounting integration
- inventory integration
- financing integration
- payment gateway integration

