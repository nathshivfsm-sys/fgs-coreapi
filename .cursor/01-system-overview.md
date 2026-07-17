# System Overview

## Architecture

Browser/Mobile → Nginx → BFF → Application Layer (Use Cases) → CRM /
Dispatch / Billing / Inventory / Identity → Outbox → RabbitMQ

## Principles

-   DDD
-   Clean Architecture
-   Microservices
-   CQRS where appropriate
-   Outbox Pattern
-   Event Driven Architecture
