DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'file') THEN
        CREATE SCHEMA file;
    END IF;
END $EF$;
CREATE TABLE IF NOT EXISTS file."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM file."__EFMigrationsHistory" WHERE "MigrationId" = '20260603225607_InitialFile') THEN
        IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'file') THEN
            CREATE SCHEMA file;
        END IF;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM file."__EFMigrationsHistory" WHERE "MigrationId" = '20260603225607_InitialFile') THEN
    CREATE TABLE file."FgsFile" (
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "Id" bigint GENERATED ALWAYS AS IDENTITY,
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
        "IsVisibleToCustomer" boolean NOT NULL DEFAULT TRUE,
        "IsVisibleToFieldTechnician" boolean NOT NULL DEFAULT TRUE,
        "UploadedByUserId" bigint,
        "UploadedByName" character varying(255) NOT NULL,
        "UploadedByType" character varying(50) NOT NULL,
        "CreatedOn" timestamptz NOT NULL DEFAULT (now()),
        "CreatedBy" text,
        "UpdatedOn" timestamp with time zone,
        "UpdatedBy" text,
        CONSTRAINT "PK_FgsFile" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM file."__EFMigrationsHistory" WHERE "MigrationId" = '20260603225607_InitialFile') THEN
    CREATE INDEX "IX_FgsFile_Entity" ON file."FgsFile" ("TenantId", "CompanyId", "EntityType", "EntityId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM file."__EFMigrationsHistory" WHERE "MigrationId" = '20260603225607_InitialFile') THEN
    CREATE INDEX "IX_FgsFile_Tags" ON file."FgsFile" USING gin ("Tags");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM file."__EFMigrationsHistory" WHERE "MigrationId" = '20260603225607_InitialFile') THEN
    CREATE INDEX "IX_FgsFile_TenantId_CompanyId" ON file."FgsFile" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM file."__EFMigrationsHistory" WHERE "MigrationId" = '20260603225607_InitialFile') THEN
    CREATE UNIQUE INDEX "UX_FgsFile_Bucket_ObjectKey" ON file."FgsFile" ("BucketName", "ObjectKey");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM file."__EFMigrationsHistory" WHERE "MigrationId" = '20260603225607_InitialFile') THEN
    INSERT INTO file."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260603225607_InitialFile', '10.0.8');
    END IF;
END $EF$;
COMMIT;

