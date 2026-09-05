# Files & attachments

- **Owner:** FileService (`file`)
- **Purpose:** File metadata + object storage for tenants
- **Entities:** `FgsFile`
- **APIs:** `/api/v1/attachment`, `/api/v1/tenantstorage` (anonymous internal patterns)
- **Deps:** User tenant client; S3-style storage via credentials
- **Clone:** `AttachmentController`, File Application Features
