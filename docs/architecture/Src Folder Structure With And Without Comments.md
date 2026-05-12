# FSM Final Folder Structure (With Comments)

```text
src/
│
├── Shared/                                  # Reusable framework-level code shared across all services
│   │
│   ├── Kernel/                              # Core architectural/domain primitives
│   │   ├── BaseEntities/                    # Base entity classes
│   │   ├── BaseInterfaces/                  # Core interfaces
│   │   ├── BaseModels/                      # Shared base models
│   │   └── DomainEvents/                    # Domain event contracts and handlers
│   │
│   ├── Common/                              # Generic reusable helper code
│   │   ├── Constants/                       # Shared constants
│   │   ├── Enums/                           # Shared enums
│   │   ├── Helpers/                         # Helper classes
│   │   ├── Extensions/                      # Extension methods
│   │   └── Utilities/                       # Utility classes
│   │
│   ├── Contracts/                           # Shared DTOs/contracts/events
│   │   ├── DTOs/                            # Data transfer objects
│   │   ├── Requests/                        # API request contracts
│   │   ├── Responses/                       # API response contracts
│   │   └── Events/                          # Integration/domain events
│   │
│   ├── Infrastructure/                      # Shared infrastructure abstractions
│   │   ├── Persistence/                     # Shared DB infrastructure
│   │   ├── Caching/                         # Cache abstractions/providers
│   │   ├── Messaging/                       # Messaging abstractions only
│   │   │   ├── IEmailSender/
│   │   │   ├── ISmsSender/
│   │   │   └── IPushSender/
│   │   │
│   │   ├── Storage/                         # Storage abstractions
│   │   ├── Security/                        # Shared security infrastructure
│   │   └── Logging/                         # Shared logging infrastructure
│   │
│   └── Localization/                        # Localization resources
│
│
├── CRMService/                              # CRM/business relationship domain
│   ├── Customers/
│   ├── Contacts/
│   ├── Leads/
│   ├── Accounts/
│   └── Opportunities/
│
│
├── FileService/                             # File/document management
│   ├── FileStorage/
│   ├── Attachments/
│   ├── Documents/
│   └── Media/
│
│
├── FinanceService/                          # Financial and billing domain
│   ├── Invoices/
│   ├── Payments/
│   ├── Taxes/
│   ├── Accounting/
│   └── Billing/
│
│
├── InventoryService/                        # Inventory and warehouse domain
│   ├── Items/
│   ├── Warehouses/
│   ├── Stock/
│   ├── Transfers/
│   └── Adjustments/
│
│
├── PlatformService/                         # Cross-platform shared business capabilities
│   │
│   ├── Audit/                               # Audit/history tracking
│   │
│   ├── BackgroundJobs/                      # Queued/background processing
│   │
│   ├── Configuration/                       # Platform/global configuration
│   │
│   ├── Integrations/                        # External integrations
│   │   ├── QuickBooks/
│   │   ├── Stripe/
│   │   ├── Twilio/
│   │   └── SendGrid/
│   │
│   ├── Notifications/                       # Enterprise notification subsystem
│   │   ├── Templates/                       # Email/SMS/push templates
│   │   ├── Channels/                        # Notification channels
│   │   ├── Providers/                       # Provider implementations
│   │   ├── Preferences/                     # User notification preferences
│   │   ├── Queues/                          # Notification queues
│   │   ├── History/                         # Notification history/logging
│   │   └── Workers/                         # Background workers
│   │
│   ├── Reporting/                           # Reporting/analytics subsystem
│   │   ├── Dashboards/
│   │   ├── Reports/
│   │   ├── Analytics/
│   │   ├── Exports/
│   │   └── KPIs/
│   │
│   ├── Setup/                               # Shared master/setup/reference data
│   │   │
│   │   ├── CRM/
│   │   │   ├── CustomerTypes/
│   │   │   ├── LeadSources/
│   │   │   └── Industries/
│   │   │
│   │   ├── Finance/
│   │   │   ├── PaymentMethods/
│   │   │   ├── PaymentTerms/
│   │   │   ├── TaxCodes/
│   │   │   └── FiscalPeriods/
│   │   │
│   │   ├── Geographic/
│   │   │   ├── Countries/
│   │   │   ├── States/
│   │   │   ├── Cities/
│   │   │   ├── PostalCodes/
│   │   │   └── TimeZones/
│   │   │
│   │   ├── Inventory/
│   │   │   ├── ItemCategories/
│   │   │   ├── UnitsOfMeasure/
│   │   │   └── WarehouseTypes/
│   │   │
│   │   ├── Pricing/
│   │   │   ├── PriceSheets/
│   │   │   ├── RateCards/
│   │   │   └── MarkupRules/
│   │   │
│   │   ├── Scheduling/
│   │   │   ├── TimeSlots/
│   │   │   ├── BusinessHours/
│   │   │   ├── Holidays/
│   │   │   └── DispatchZones/
│   │   │
│   │   ├── System/
│   │   │   ├── Languages/
│   │   │   ├── Currencies/
│   │   │   ├── FeatureFlags/
│   │   │   └── TenantSettings/
│   │   │
│   │   └── WorkOrder/
│   │       ├── PriorityTypes/
│   │       ├── ResolutionCodes/
│   │       ├── Statuses/
│   │       └── CancellationReasons/
│   │
│   ├── Templates/                           # Shared system templates
│   │
│   └── Workflow/                            # Workflow/orchestration engine
│
│
├── ProposalService/                         # Estimates/proposals/quotes domain
│   ├── Proposals/
│   ├── ProposalItems/
│   ├── Pricing/
│   ├── Approvals/
│   └── Signatures/
│
│
├── PurchasingService/                       # Purchasing/procurement domain
│   ├── Vendors/
│   ├── PurchaseOrders/
│   ├── Receipts/
│   ├── Returns/
│   └── Procurement/
│
│
├── SchedulingService/                       # Dispatch/scheduling domain
│   ├── Dispatch/
│   ├── Calendars/
│   ├── TimeSlots/
│   ├── Routes/
│   └── Assignments/
│
│
├── ServiceAgreementService/                 # Service agreements/contracts domain
│   ├── Contracts/
│   ├── SLAs/
│   ├── PreventiveMaintenance/
│   ├── Coverage/
│   ├── Renewals/
│   └── RecurringServices/
│
│
├── UserService/                             # User/security domain
│   ├── Users/
│   ├── Roles/
│   ├── Permissions/
│   ├── Authentication/
│   └── Authorization/
│
│
└── WorkOrderService/                        # Core FSM work order domain
    ├── WorkOrders/
    ├── WorkOrderTasks/
    ├── Technicians/
    ├── Labor/
    ├── Resolution/
    ├── Checklists/
    ├── Assets/
    └── Completion/
```

---

# FSM Final Folder Structure (Without Comments)

```text
src/
│
├── Shared/
│   │
│   ├── Kernel/
│   │   ├── BaseEntities/
│   │   ├── BaseInterfaces/
│   │   ├── BaseModels/
│   │   └── DomainEvents/
│   │
│   ├── Common/
│   │   ├── Constants/
│   │   ├── Enums/
│   │   ├── Helpers/
│   │   ├── Extensions/
│   │   └── Utilities/
│   │
│   ├── Contracts/
│   │   ├── DTOs/
│   │   ├── Requests/
│   │   ├── Responses/
│   │   └── Events/
│   │
│   ├── Infrastructure/
│   │   ├── Persistence/
│   │   ├── Caching/
│   │   ├── Messaging/
│   │   │   ├── IEmailSender/
│   │   │   ├── ISmsSender/
│   │   │   └── IPushSender/
│   │   │
│   │   ├── Storage/
│   │   ├── Security/
│   │   └── Logging/
│   │
│   └── Localization/
│
├── CRMService/
│   ├── Customers/
│   ├── Contacts/
│   ├── Leads/
│   ├── Accounts/
│   └── Opportunities/
│
├── FileService/
│   ├── FileStorage/
│   ├── Attachments/
│   ├── Documents/
│   └── Media/
│
├── FinanceService/
│   ├── Invoices/
│   ├── Payments/
│   ├── Taxes/
│   ├── Accounting/
│   └── Billing/
│
├── InventoryService/
│   ├── Items/
│   ├── Warehouses/
│   ├── Stock/
│   ├── Transfers/
│   └── Adjustments/
│
├── PlatformService/
│   │
│   ├── Audit/
│   ├── BackgroundJobs/
│   ├── Configuration/
│   │
│   ├── Integrations/
│   │   ├── QuickBooks/
│   │   ├── Stripe/
│   │   ├── Twilio/
│   │   └── SendGrid/
│   │
│   ├── Notifications/
│   │   ├── Templates/
│   │   ├── Channels/
│   │   ├── Providers/
│   │   ├── Preferences/
│   │   ├── Queues/
│   │   ├── History/
│   │   └── Workers/
│   │
│   ├── Reporting/
│   │   ├── Dashboards/
│   │   ├── Reports/
│   │   ├── Analytics/
│   │   ├── Exports/
│   │   └── KPIs/
│   │
│   ├── Setup/
│   │   │
│   │   ├── CRM/
│   │   │   ├── CustomerTypes/
│   │   │   ├── LeadSources/
│   │   │   └── Industries/
│   │   │
│   │   ├── Finance/
│   │   │   ├── PaymentMethods/
│   │   │   ├── PaymentTerms/
│   │   │   ├── TaxCodes/
│   │   │   └── FiscalPeriods/
│   │   │
│   │   ├── Geographic/
│   │   │   ├── Countries/
│   │   │   ├── States/
│   │   │   ├── Cities/
│   │   │   ├── PostalCodes/
│   │   │   └── TimeZones/
│   │   │
│   │   ├── Inventory/
│   │   │   ├── ItemCategories/
│   │   │   ├── UnitsOfMeasure/
│   │   │   └── WarehouseTypes/
│   │   │
│   │   ├── Pricing/
│   │   │   ├── PriceSheets/
│   │   │   ├── RateCards/
│   │   │   └── MarkupRules/
│   │   │
│   │   ├── Scheduling/
│   │   │   ├── TimeSlots/
│   │   │   ├── BusinessHours/
│   │   │   ├── Holidays/
│   │   │   └── DispatchZones/
│   │   │
│   │   ├── System/
│   │   │   ├── Languages/
│   │   │   ├── Currencies/
│   │   │   ├── FeatureFlags/
│   │   │   └── TenantSettings/
│   │   │
│   │   └── WorkOrder/
│   │       ├── PriorityTypes/
│   │       ├── ResolutionCodes/
│   │       ├── Statuses/
│   │       └── CancellationReasons/
│   │
│   ├── Templates/
│   └── Workflow/
│
├── ProposalService/
│   ├── Proposals/
│   ├── ProposalItems/
│   ├── Pricing/
│   ├── Approvals/
│   └── Signatures/
│
├── PurchasingService/
│   ├── Vendors/
│   ├── PurchaseOrders/
│   ├── Receipts/
│   ├── Returns/
│   └── Procurement/
│
├── SchedulingService/
│   ├── Dispatch/
│   ├── Calendars/
│   ├── TimeSlots/
│   ├── Routes/
│   └── Assignments/
│
├── ServiceAgreementService/
│   ├── Contracts/
│   ├── SLAs/
│   ├── PreventiveMaintenance/
│   ├── Coverage/
│   ├── Renewals/
│   └── RecurringServices/
│
├── UserService/
│   ├── Users/
│   ├── Roles/
│   ├── Permissions/
│   ├── Authentication/
│   └── Authorization/
│
└── WorkOrderService/
    ├── WorkOrders/
    ├── WorkOrderTasks/
    ├── Technicians/
    ├── Labor/
    ├── Resolution/
    ├── Checklists/
    ├── Assets/
    └── Completion/
```

