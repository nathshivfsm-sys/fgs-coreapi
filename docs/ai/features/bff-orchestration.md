# BFF orchestration

- **Owner:** BffService
- **Purpose:** Cross-domain workflows (signup), GraphQL stub at `/api/v1/bff/graphql`
- **No domain DB**
- **Refit:** User signup/tenant, Setup, auth profile
- **Rule:** do not move owning-service CRUD into BFF
- **Clone:** `SignupController` + Application signup feature
