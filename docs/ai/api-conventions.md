# API conventions

- Versioned route attribute: `api/v{version}/{template}` via `FgsVersionedRoute`
- Public prefix: `/api/v1/...`
- Envelope: `ApiResponse<T>` (`Success`, `StatusCode`, `Data`, `Errors`)
- Prefer `FgsApiControllerBase.FromApiResponse` for new controllers
- Existing Setup/Asset/Inventory often: `StatusCode(response.StatusCode, response)`
- List query: `page`, `pageSize`, `sortBy`, `sortDirection`, `search`, `bool? isActive`
- Lookup: `GET /{resource}/lookup?activeOnly=true`
- Soft delete: usually `PATCH` `{ "isActive": false }`
- Headers: `X-Tenant-Id`, `X-Company-Id`, `X-Api-Version`, correlation header from Foundation middleware
- Gateway routes: `src/Gateway/conf.d/includes/api-v1-routes.conf`
- BFF: cross-domain only (`/api/v1/bff/...`); simple CRUD stays on owning services
- Errors: middleware maps exceptions → `ApiResponse` fail (not raw ProblemDetails everywhere)
