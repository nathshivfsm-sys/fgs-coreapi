-- =============================================================================
-- Migration: 20260522150000_FgsFile
-- Creates FgsFile with storage, entity lookup, and tag search indexes.
-- Pair with: Database/Migrations/20260522150000_FgsFile.cs
-- =============================================================================

START TRANSACTION;

CREATE TABLE IF NOT EXISTS dbo."FgsFile"
(
    "Id" bigint NOT NULL GENERATED ALWAYS AS IDENTITY,

    "TenantId" uuid NOT NULL,
    "CompanyId" bigint NOT NULL,

    "EntityType" character varying(50) NOT NULL,
    "EntityId" bigint NOT NULL,

    "BucketName" character varying(255) NOT NULL,
    "ObjectKey" character varying(2000) NOT NULL,
    "ThumbnailObjectKey" character varying(2000),

    "OriginalFileName" character varying(500) NOT NULL,
    "StoredFileName" character varying(500) NOT NULL,

    "ContentType" character varying(255),
    "FileExtension" character varying(20),

    "FileSizeBytes" bigint NOT NULL,

    "Description" text,

    "Tags" text[],

    "IsVisibleToCustomer" boolean NOT NULL DEFAULT true,
    "IsVisibleToFieldTechnician" boolean NOT NULL DEFAULT true,

    "UploadedByUserId" bigint,
    "UploadedByName" character varying(255) NOT NULL,
    "UploadedByType" character varying(50) NOT NULL,

    "CreatedOn" timestamptz NOT NULL DEFAULT now(),
    "CreatedBy" character varying(100),

    "UpdatedOn" timestamptz,
    "UpdatedBy" character varying(100),

    CONSTRAINT "PK_FgsFile"
        PRIMARY KEY ("Id"),

    CONSTRAINT "FK_FgsFile_FgsTenantCompany_TenantId_CompanyId"
        FOREIGN KEY ("TenantId", "CompanyId")
        REFERENCES dbo."FgsTenantCompany" ("TenantId", "CompanyNumber")
);

CREATE UNIQUE INDEX IF NOT EXISTS "UX_FgsFile_Bucket_ObjectKey"
    ON dbo."FgsFile" ("BucketName", "ObjectKey");

CREATE INDEX IF NOT EXISTS "IX_FgsFile_Entity"
    ON dbo."FgsFile" ("TenantId", "CompanyId", "EntityType", "EntityId");

CREATE INDEX IF NOT EXISTS "IX_FgsFile_Tags"
    ON dbo."FgsFile"
    USING GIN ("Tags");

CREATE INDEX IF NOT EXISTS "IX_FgsFile_TenantId_CompanyId"
    ON dbo."FgsFile" ("TenantId", "CompanyId");

INSERT INTO dbo."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260522150000_FgsFile', '10.0.8')
ON CONFLICT ("MigrationId") DO NOTHING;

COMMIT;
