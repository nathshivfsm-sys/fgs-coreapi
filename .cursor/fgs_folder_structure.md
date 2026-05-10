# FSM Enterprise Folder Structure & Architecture Guide

## Purpose

This document defines the recommended production-grade folder structure for the FSM SaaS platform.

This guide is intended for:
- Developers
- AI coding agents
- DevOps engineers
- Architects
- Future maintainers

The structure is designed for:
- Multi-tenant SaaS architecture
- Scalability
- Clean microservice separation
- Docker/Kubernetes deployment
- CI/CD pipelines
- Enterprise maintainability
- Security
- Extensibility

---

# High-Level Architecture

```text
Client Apps
    ↓
NGINX / Reverse Proxy
    ↓
API Gateway
    ↓
Microservices
    ↓
Shared Infrastructure
    ↓
Database / Cache / Queue / Storage
```

---

# Recommended Root Folder Structure

```text
/FSM
 ├── src
 ├── frontend
 ├── mobile
 ├── infrastructure
 ├── database
 ├── shared
 ├── tests
 ├── docs
 ├── scripts
 ├── tools
 ├── deployments
 ├── .github
 ├── docker-compose.yml
 ├── README.md
 └── LICENSE
```

---

# Root Folder Explanations

| Folder | Purpose |
|---|---|
| src | Backend microservices source code |
| frontend | Web frontend applications |
| mobile | Mobile applications |
| infrastructure | Infrastructure configuration |
| database | Database scripts and schemas |
| shared | Shared libraries/packages |
| tests | Integration and system tests |
| docs | Architecture and business documents |
| scripts | Utility scripts |
| tools | Developer tools |
| deployments | Environment deployment configs |
| .github | CI/CD workflows |

---

# Backend Services Structure

```text
/src
 ├── Gateway
 │
 ├── Services
 │    ├── PlatformService
 │    ├── CRMService
 │    ├── InventoryService
 │    ├── PurchasingService
 │    ├── FinanceService
 │    ├── WorkOrderService
 │    ├── SchedulingService
 │    ├── NotificationService
 │    ├── ReportingService
 │    └── FileService
 │
 └── SharedKernel
```

---

# Why Separate Services?

| Service | Responsibility |
|---|---|
| PlatformService | Tenants, identity, subscriptions |
| CRMService | Customers and contacts |
| InventoryService | Inventory and warehouses |
| PurchasingService | Vendors and purchase orders |
| FinanceService | Invoices and payments |
| WorkOrderService | Service work execution |
| SchedulingService | Dispatching and technician scheduling |
| NotificationService | Email/SMS/push notifications |
| ReportingService | Analytics and reporting |
| FileService | File uploads and document storage |

Benefits:
- Independent scaling
- Better security isolation
- Easier deployments
- Cleaner ownership
- Easier AI-assisted development
- Better long-term maintainability

---

# API Gateway Structure

```text
/src/Gateway
 ├── ApiGateway
 │     ├── Controllers
 │     ├── Middleware
 │     ├── Authentication
 │     ├── Authorization
 │     ├── TenantResolution
 │     ├── RateLimiting
 │     ├── Swagger
 │     └── Configuration
```

---

# API Gateway Responsibilities

| Responsibility | Description |
|---|---|
| Authentication | Validate JWT/auth tokens |
| Authorization | Validate permissions |
| Tenant Resolution | Resolve current tenant |
| Request Routing | Route to services |
| Rate Limiting | Prevent abuse |
| Logging | Centralized request logs |
| Swagger | API documentation |

IMPORTANT:
Business logic should NOT be implemented in the API Gateway.

---

# Platform Service Structure

```text
/src/Services/PlatformService
 ├── Tenancy
 ├── Identity
 ├── Subscription
 ├── Licensing
 ├── FeatureManagement
 ├── Administration
 ├── Audit
 └── Configuration
```

---

# Platform Service Responsibilities

## Tenancy
Stores:
- FSGTenant
- FSGTenantCompany
- Tenant domains
- Tenant environments

## Identity
Stores:
- Users
- Roles
- Permissions
- External identities
- Login providers

## Subscription
Stores:
- Plans
- Billing subscriptions
- Feature access

## FeatureManagement
Stores:
- Feature flags
- Tenant feature enablement

---

# Why Tenant and TenantCompany Belong Here

Tenant information is platform-level infrastructure.

It is NOT:
- CRM data
- Customer data
- Accounting data
- Operational data

A tenant owns all business data across the system.

Structure:

```text
Tenant
 └── TenantCompany
       └── Business Data
```

Example:

```text
Tenant:
  ABC Facility Management

Tenant Companies:
  ABC Mechanical
  ABC Electrical
  ABC Houston
```

---

# NGINX Folder Structure

NGINX belongs inside Infrastructure.

Recommended location:

```text
/infrastructure/nginx
```

Structure:

```text
/infrastructure/nginx
 ├── nginx.conf
 ├── conf.d
 │     ├── api.conf
 │     ├── frontend.conf
 │     ├── websocket.conf
 │     └── tenant-routing.conf
 │
 ├── ssl
 │     ├── cert.pem
 │     └── key.pem
 │
 ├── docker
 │     └── Dockerfile
 │
 └── logs
```

---

# NGINX Responsibilities

| Responsibility | Description |
|---|---|
| Reverse Proxy | Route incoming traffic |
| SSL Termination | Handle HTTPS |
| Load Balancing | Multiple API instances |
| Security Headers | Add security protections |
| Static File Hosting | Frontend hosting |
| WebSocket Proxy | Real-time communication |
| Rate Limiting | Prevent abuse |

IMPORTANT:
NGINX should NOT contain business logic.

Do NOT implement:
- User validation
- Permission logic
- Tenant business rules
- Database access

NGINX should only route traffic.

---

# Infrastructure Structure

```text
/infrastructure
 ├── nginx
 ├── docker
 ├── kubernetes
 ├── monitoring
 ├── logging
 ├── observability
 ├── terraform
 └── security
```

---

# Infrastructure Folder Explanations

| Folder | Purpose |
|---|---|
| nginx | Reverse proxy configs |
| docker | Docker containers |
| kubernetes | K8s manifests |
| monitoring | Prometheus/Grafana |
| logging | ELK/centralized logging |
| observability | Tracing and metrics |
| terraform | Infrastructure-as-code |
| security | Security policies |

---

# Shared Kernel Structure

```text
/src/SharedKernel
 ├── Common
 ├── Contracts
 ├── DTOs
 ├── Events
 ├── Exceptions
 ├── Extensions
 ├── Middleware
 ├── Messaging
 ├── Utilities
 └── Validation
```

---

# Why Shared Kernel Exists

Purpose:
- Avoid duplicate code
- Standardize patterns
- Reuse contracts
- Reuse validations
- Reuse middleware

Examples:
- BaseEntity
- AuditEntity
- ApiResponse
- Domain events
- JWT helpers
- Validation utilities

IMPORTANT:
SharedKernel should NEVER contain business-specific logic.

---

# Database Structure

```text
/database
 ├── migrations
 ├── seed
 ├── scripts
 ├── stored-procedures
 ├── views
 ├── functions
 └── backups
```

---

# Recommended Naming Conventions

## Tables

Use:

```text
PascalCase
```

Examples:

```text
FSGTenant
FSGTenantCompany
FSGCustomer
FSGVendor
FSGPurchaseOrder
```

---

# Frontend Structure

```text
/frontend
 ├── admin-portal
 ├── customer-portal
 ├── technician-portal
 └── shared-ui
```

---

# Mobile Structure

```text
/mobile
 ├── technician-app
 ├── customer-app
 └── shared-mobile
```

---

# Tests Structure

```text
/tests
 ├── integration
 ├── unit
 ├── performance
 ├── security
 └── end-to-end
```

---

# Documentation Structure

```text
/docs
 ├── architecture
 ├── api
 ├── database
 ├── deployment
 ├── security
 ├── workflows
 └── business-rules
```

---

# AI Agent Development Guidelines

## Every service should contain:

```text
ServiceName
 ├── Controllers
 ├── Application
 ├── Domain
 ├── Infrastructure
 ├── Persistence
 ├── DTOs
 ├── Validators
 ├── Services
 ├── Repositories
 └── Configuration
```

---

# Layer Responsibilities

| Layer | Responsibility |
|---|---|
| Controllers | HTTP endpoints |
| Application | Use cases |
| Domain | Business rules |
| Infrastructure | External integrations |
| Persistence | Database access |
| DTOs | Request/response models |
| Validators | Validation rules |
| Services | Domain services |
| Repositories | Data repositories |

---

# Multi-Tenant Design Rules

## Every business table should include:

```text
TenantId
CompanyId
```

This ensures:
- Data isolation
- Security
- Scalability
- Reporting separation

---

# Security Recommendations

## Never expose:
- Internal IDs
- Database structure
- Service connection strings
- Secrets

## Store secrets in:

```text
/infrastructure/security
```

or:
- Azure Key Vault
- AWS Secrets Manager
- HashiCorp Vault

---

# Recommended Deployment Strategy

## Development

```text
Docker Compose
```

## Production

```text
Kubernetes
```

---

# Recommended Technologies

| Area | Recommendation |
|---|---|
| Backend | .NET |
| Frontend | React or Angular |
| Mobile | Flutter or React Native |
| Database | PostgreSQL |
| Cache | Redis |
| Messaging | RabbitMQ |
| Reverse Proxy | NGINX |
| Containerization | Docker |
| Orchestration | Kubernetes |
| CI/CD | GitHub Actions |
| Monitoring | Grafana + Prometheus |

---

# Final Architectural Principles

## 1. Keep business logic inside services

Do NOT place business logic in:
- NGINX
- API Gateway
- Frontend
- Database triggers

---

## 2. Keep services independent

Each service should:
- Own its own domain
- Have clear boundaries
- Avoid direct database sharing

---

## 3. Build for SaaS from day one

Everything should support:
- Multiple tenants
- Multiple companies
- Feature flags
- Subscription models

---

## 4. Infrastructure is separate from business code

Infrastructure belongs in:

```text
/infrastructure
```

NOT inside:
- CRM
- Finance
- WorkOrder
- Inventory

---

# Recommended Initial Implementation Order

1. Infrastructure
2. NGINX
3. API Gateway
4. PlatformService
5. Identity
6. Tenant Management
7. CRMService
8. WorkOrderService
9. InventoryService
10. FinanceService

---

# Final Notes for Developers and AI Agents

This architecture is designed for:
- Long-term maintainability
- Enterprise scalability
- Clean domain boundaries
- SaaS multi-tenancy
- Secure deployments
- AI-assisted development

Always maintain:
- Separation of concerns
- Domain ownership
- Infrastructure isolation
- Consistent naming conventions
- Tenant isolation

