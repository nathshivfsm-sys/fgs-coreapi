---
name: add-unit-tests
description: Add xUnit handler and validator tests matching existing FGS test projects. Use when implementing features or when tests are missing for changed handlers.
---

# Add unit tests

Clone `{Entity}CommandHandlerTests` / `{Entity}ValidatorTests` in the same service Tests project.

## Steps

1. xUnit + Moq + FluentAssertions. No Testcontainers.
2. Mock write/read abstractions and `ITenantContextAccessor`.
3. Cover: happy path, validation fail, not found, duplicate/409.
4. Do not assert log text unless neighbors do.
5. Run `dotnet test` on that Tests csproj.

## Verify

- [ ] Tests fail before the production fix (when bugfixing)
- [ ] No real Redis/Postgres/RabbitMQ
- [ ] Service tests pass
