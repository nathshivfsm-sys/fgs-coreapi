# FGS AI knowledge

Token-efficient reference for agents. **Code wins** over older notes.

## Hierarchy

| Layer | Path | Role |
|-------|------|------|
| Rules | `.cursor/rules/*.mdc` | Persistent conventions |
| Skills | `.cursor/skills/*/SKILL.md` | How to perform a task |
| This folder | `docs/ai/` | What the repo contains |

## Workflow

```text
Requirement → services.md / features/* → skill → clone neighbor → rules → implement → tests → build → review
```

## Index

| Doc | Contents |
|-----|----------|
| [architecture.md](architecture.md) | Style, layers, shared libs |
| [project-map.md](project-map.md) | Folders and solution |
| [services.md](services.md) | Microservice maturity |
| [database.md](database.md) | Schemas and ownership |
| [api-conventions.md](api-conventions.md) | Routes, envelopes |
| [authentication.md](authentication.md) | Entra JWT |
| [authorization.md](authorization.md) | Permissions / RBAC |
| [multi-tenancy.md](multi-tenancy.md) | Headers + filters |
| [messaging.md](messaging.md) | RabbitMQ |
| [outbox.md](outbox.md) | Outbox + service-owned workers |
| [testing.md](testing.md) | xUnit |
| [deployment.md](deployment.md) | NGINX, EC2, CI |
| [configuration.md](configuration.md) | Config section names |
| [features/](features/) | Business domains |
| [knowledge-validation.md](knowledge-validation.md) | Validation report |

## Also keep (do not delete)

- `.cursor/*.md` — historical ADRs/templates (may conflict; prefer this folder)
- `docs/architecture/*` — deeper human docs
- `.cursor/SETUP_ENTITY_CRUD_TEMPLATE.md` — Setup CRUD
