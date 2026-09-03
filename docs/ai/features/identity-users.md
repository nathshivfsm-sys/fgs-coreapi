# Identity & users

- **Owner:** UserService (`identity`)
- **Purpose:** Users, invites, roles/permissions, auth profile
- **Entities:** `FgsUser`, `FgsInvitation`, `FgsRole`, `FgsPermission`, `FgsUserRole`, …
- **APIs:** `/api/v1/user`, role/permission/data-access controllers, `/api/v1/internal/users`
- **AuthZ:** `USER.*` permissions; invite/signup anonymous
- **Events:** invite email via outbox (`user.CompanySignupInviteEmail`)
- **Clone:** `UserController`, `InviteFgsUser`
- **Change often:** `Features/Users`, `Features/Auth`, `Features/Roles`
