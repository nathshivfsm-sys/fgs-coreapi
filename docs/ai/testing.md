# Testing

- Framework: **xUnit** (+ Moq, FluentAssertions, Coverlet)
- Layout: feature-aligned `*HandlerTests`, `*ValidatorTests`
- No Testcontainers / `WebApplicationFactory` suite found
- Shared library tests: Security, Messaging, Credentials, Foundation
- CI: `reusable-build-service.yml` runs the service `test_project` on PR

Prefer unit tests at Application boundary. Do not invent an integration harness unless requested.
