# Project map

```text
src/
  FGS.slnx                 # unified solution
  Directory.Build.props    # net10.0, EF/MediatR versions
  Shared/                  # Kernel, Foundation, Contracts, Persistence,
                           # MultiTenancy, Security, Messaging, Credentials, Observability
  Gateway/                 # NGINX + docker-compose (local)
  BffService/
  UserService/ SetupService/ FileService/ AuditService/ NotificationService/
  InventoryService/ AssetService/
  BillingService/ CrmService/ SchedulingService/ ServiceAgreementService/
  ReportingService/ IntegrationService/ CommunicationService/  # mostly scaffold
  ConsumerService/                                             # dedicated messaging worker

docs/ai/                   # this knowledge base
docs/architecture/         # human architecture notes
docs/api/                  # Postman
deployment/aws/            # EC2, terraform, Datadog fragments
.github/workflows/         # build + deploy
.cursor/rules/             # agent rules
.cursor/skills/            # agent skills
.cursor/*.md               # older notes/templates (kept)
```

Per-service `*.slnx` also exist under each service folder.
