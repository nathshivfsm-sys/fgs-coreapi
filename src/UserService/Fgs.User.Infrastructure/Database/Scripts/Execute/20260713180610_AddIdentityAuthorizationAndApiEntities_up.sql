START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    DROP INDEX identity."IX_FgsUserRole_GloRoleId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    DROP INDEX identity."IX_FgsUserRole_UserId_FgsRoleId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    DROP INDEX identity."IX_FgsUserRole_UserId_GloRoleId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    ALTER TABLE identity."FgsUserRole" DROP CONSTRAINT "CK_FgsUserRole_OnlyOneRole";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    ALTER TABLE identity."FgsUserRole" DROP COLUMN "GloRoleId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    ALTER TABLE identity."FgsRole" DROP COLUMN "GloRoleId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    COMMENT ON TABLE identity."FgsUserRole" IS 'Assigns one or more security roles to users within a company.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    COMMENT ON TABLE identity."FgsRole" IS 'Stores built-in platform roles and company-defined custom roles used by the authorization system.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    UPDATE identity."FgsUserRole" SET "FgsRoleId" = 0 WHERE "FgsRoleId" IS NULL;
    ALTER TABLE identity."FgsUserRole" ALTER COLUMN "FgsRoleId" SET NOT NULL;
    ALTER TABLE identity."FgsUserRole" ALTER COLUMN "FgsRoleId" SET DEFAULT 0;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    COMMENT ON COLUMN identity."FgsUserRole"."CreatedOn" IS 'Date and time the role assignment was created.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    ALTER TABLE identity."FgsUserRole" ADD "CreatedBy" character varying(100) NOT NULL DEFAULT '';
    COMMENT ON COLUMN identity."FgsUserRole"."CreatedBy" IS 'User or system that assigned the role.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    COMMENT ON COLUMN identity."FgsRole"."UpdatedOn" IS 'Date and time the role was last modified.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    COMMENT ON COLUMN identity."FgsRole"."RoleCode" IS 'Unique system identifier for the role. Used internally by the application and should not be editable after creation.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    COMMENT ON COLUMN identity."FgsRole"."Name" IS 'Display name shown to administrators and users.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    ALTER TABLE identity."FgsRole" ALTER COLUMN "IsActive" SET DEFAULT TRUE;
    COMMENT ON COLUMN identity."FgsRole"."IsActive" IS 'Indicates whether the role is available for assignment. Built-in roles should always remain active.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    COMMENT ON COLUMN identity."FgsRole"."Description" IS 'Optional description explaining the purpose of the role.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    COMMENT ON COLUMN identity."FgsRole"."CreatedOn" IS 'Date and time the role was created.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    ALTER TABLE identity."FgsRole" ADD "DisplayOrder" smallint NOT NULL DEFAULT 1;
    COMMENT ON COLUMN identity."FgsRole"."DisplayOrder" IS 'Controls the display order of roles within the user interface.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    ALTER TABLE identity."FgsRole" ADD "IsBuiltIn" boolean NOT NULL DEFAULT FALSE;
    COMMENT ON COLUMN identity."FgsRole"."IsBuiltIn" IS 'Indicates whether the role is provided by the platform. Built-in roles cannot be edited, deleted, or deactivated but may be cloned.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    ALTER TABLE identity."FgsRole" ADD "ParentRoleId" bigint;
    COMMENT ON COLUMN identity."FgsRole"."ParentRoleId" IS 'Original role from which this role was cloned. NULL for built-in roles or roles created from scratch.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE TABLE identity."FgsApiClient" (
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "ClientId" uuid NOT NULL DEFAULT (gen_random_uuid()),
        "ApplicationName" character varying(100) NOT NULL,
        "Description" character varying(255),
        "ContactName" character varying(100),
        "ContactEmail" character varying(300),
        "RateLimitPerMinute" integer NOT NULL DEFAULT 60,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" character varying(100),
        "UpdatedOn" timestamptz,
        "UpdatedBy" character varying(100),
        "IsActive" boolean NOT NULL DEFAULT TRUE,
        CONSTRAINT "PK_FgsApiClient" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_FgsApiClient_FgsTenantCompanyCache" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES identity."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT
    );
    COMMENT ON TABLE identity."FgsApiClient" IS 'Stores developer applications created by tenant administrators for third-party integrations. Represents an application, not a credential.';
    COMMENT ON COLUMN identity."FgsApiClient"."ClientId" IS 'Public client identifier used by external applications during authentication.';
    COMMENT ON COLUMN identity."FgsApiClient"."ApplicationName" IS 'Display name of the application registered by the customer.';
    COMMENT ON COLUMN identity."FgsApiClient"."Description" IS 'Optional description explaining the purpose of the application.';
    COMMENT ON COLUMN identity."FgsApiClient"."ContactName" IS 'Primary contact responsible for the application.';
    COMMENT ON COLUMN identity."FgsApiClient"."ContactEmail" IS 'Email address of the application owner or support contact.';
    COMMENT ON COLUMN identity."FgsApiClient"."RateLimitPerMinute" IS 'Maximum number of API requests permitted per minute for this application.';
    COMMENT ON COLUMN identity."FgsApiClient"."IsActive" IS 'Indicates whether the application is permitted to authenticate and access the API.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE TABLE identity."FgsApiEvent" (
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "EventCode" character varying(100) NOT NULL,
        "EventCategory" character varying(50) NOT NULL,
        "Name" character varying(100) NOT NULL,
        "Description" character varying(255),
        "EventVersion" smallint NOT NULL DEFAULT 1,
        "DisplayOrder" smallint NOT NULL DEFAULT 1,
        "IsActive" boolean NOT NULL DEFAULT TRUE,
        "CreatedOn" timestamptz NOT NULL,
        CONSTRAINT "PK_FgsApiEvent" PRIMARY KEY ("Id")
    );
    COMMENT ON TABLE identity."FgsApiEvent" IS 'Master catalog of public API events that external applications may subscribe to through webhooks.';
    COMMENT ON COLUMN identity."FgsApiEvent"."EventCode" IS 'Unique event identifier exposed through the public API. Example: workorder.completed.';
    COMMENT ON COLUMN identity."FgsApiEvent"."EventCategory" IS 'Logical category used to organize events, such as WorkOrder, Estimate, Invoice, Customer or Payment.';
    COMMENT ON COLUMN identity."FgsApiEvent"."Name" IS 'Display name of the API event.';
    COMMENT ON COLUMN identity."FgsApiEvent"."Description" IS 'Description of when the event is published.';
    COMMENT ON COLUMN identity."FgsApiEvent"."EventVersion" IS 'Version number of the public event contract. Used to support backward-compatible changes to webhook payloads and API event schemas.';
    COMMENT ON COLUMN identity."FgsApiEvent"."DisplayOrder" IS 'Controls the display order within the Developer Portal.';
    COMMENT ON COLUMN identity."FgsApiEvent"."IsActive" IS 'Indicates whether the event is available for webhook subscriptions.';
    COMMENT ON COLUMN identity."FgsApiEvent"."CreatedOn" IS 'Date and time the API event was created.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE TABLE identity."FgsApiWebhook" (
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "Name" character varying(100) NOT NULL,
        "Description" character varying(255),
        "EndpointUrl" character varying(500) NOT NULL,
        "AuthenticationType" character varying(30) NOT NULL,
        "AuthenticationValue" character varying(500),
        "Secret" character varying(255),
        "TimeoutSeconds" smallint NOT NULL DEFAULT 30,
        "MaximumRetryCount" smallint NOT NULL DEFAULT 5,
        "LastSuccessfulDeliveryOn" timestamptz,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" character varying(100),
        "UpdatedOn" timestamptz,
        "UpdatedBy" character varying(100),
        "IsActive" boolean NOT NULL DEFAULT TRUE,
        CONSTRAINT "PK_FgsApiWebhook" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_FgsApiWebhook_FgsTenantCompanyCache" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES identity."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT
    );
    COMMENT ON TABLE identity."FgsApiWebhook" IS 'Stores webhook endpoints registered by tenant administrators for receiving API event notifications.';
    COMMENT ON COLUMN identity."FgsApiWebhook"."Name" IS 'Display name of the webhook endpoint.';
    COMMENT ON COLUMN identity."FgsApiWebhook"."Description" IS 'Optional description explaining the purpose of the webhook endpoint.';
    COMMENT ON COLUMN identity."FgsApiWebhook"."EndpointUrl" IS 'HTTPS endpoint that receives webhook event notifications.';
    COMMENT ON COLUMN identity."FgsApiWebhook"."AuthenticationType" IS 'Authentication method used when invoking the webhook endpoint, such as None, BearerToken, BasicAuthentication or CustomHeader.';
    COMMENT ON COLUMN identity."FgsApiWebhook"."AuthenticationValue" IS 'Authentication value associated with the selected authentication type.';
    COMMENT ON COLUMN identity."FgsApiWebhook"."Secret" IS 'Shared secret used to sign webhook requests and verify message authenticity.';
    COMMENT ON COLUMN identity."FgsApiWebhook"."TimeoutSeconds" IS 'Maximum number of seconds to wait for the webhook endpoint to respond before the request is considered failed.';
    COMMENT ON COLUMN identity."FgsApiWebhook"."MaximumRetryCount" IS 'Maximum number of retry attempts after a webhook delivery failure.';
    COMMENT ON COLUMN identity."FgsApiWebhook"."LastSuccessfulDeliveryOn" IS 'Date and time the most recent webhook event was successfully delivered to this endpoint.';
    COMMENT ON COLUMN identity."FgsApiWebhook"."IsActive" IS 'Indicates whether the webhook endpoint is enabled and eligible to receive events.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE TABLE identity."FgsDataAccess" (
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "DataAccessCode" character varying(50) NOT NULL,
        "Name" character varying(100) NOT NULL,
        "Description" character varying(255),
        "IsBuiltIn" boolean NOT NULL DEFAULT FALSE,
        "DisplayOrder" smallint NOT NULL DEFAULT 1,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" character varying(100),
        "UpdatedOn" timestamptz,
        "UpdatedBy" character varying(100),
        "IsActive" boolean NOT NULL DEFAULT TRUE,
        CONSTRAINT "PK_FgsDataAccess" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_FgsDataAccess_FgsTenantCompanyCache" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES identity."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT
    );
    COMMENT ON TABLE identity."FgsDataAccess" IS 'Stores reusable data access profiles that define the scope of data a role can access.';
    COMMENT ON COLUMN identity."FgsDataAccess"."DataAccessCode" IS 'Unique system identifier for the data access profile.';
    COMMENT ON COLUMN identity."FgsDataAccess"."Name" IS 'Display name of the data access profile.';
    COMMENT ON COLUMN identity."FgsDataAccess"."Description" IS 'Optional description explaining the purpose of the data access profile.';
    COMMENT ON COLUMN identity."FgsDataAccess"."IsBuiltIn" IS 'Indicates whether the data access profile was provided by the platform. Built-in profiles cannot be edited but may be cloned.';
    COMMENT ON COLUMN identity."FgsDataAccess"."DisplayOrder" IS 'Controls the display order within the user interface.';
    COMMENT ON COLUMN identity."FgsDataAccess"."IsActive" IS 'Indicates whether the data access profile is available for assignment.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE TABLE identity."FgsPermission" (
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "PermissionCode" character varying(100) NOT NULL,
        "Module" character varying(50) NOT NULL,
        "Resource" character varying(50) NOT NULL,
        "Action" character varying(50) NOT NULL,
        "Name" character varying(100) NOT NULL,
        "Description" character varying(255),
        "DisplayOrder" smallint NOT NULL DEFAULT 1,
        "IsActive" boolean NOT NULL DEFAULT TRUE,
        "CreatedOn" timestamptz NOT NULL,
        CONSTRAINT "PK_FgsPermission" PRIMARY KEY ("Id")
    );
    COMMENT ON TABLE identity."FgsPermission" IS 'Master catalog of all permissions supported by the platform. Permissions are seeded by the application and assigned to security roles.';
    COMMENT ON COLUMN identity."FgsPermission"."PermissionCode" IS 'Unique system identifier for the permission. Example: WORKORDER.CREATE.';
    COMMENT ON COLUMN identity."FgsPermission"."Module" IS 'Functional module that owns the permission. Example: Work Orders, Billing, CRM.';
    COMMENT ON COLUMN identity."FgsPermission"."Resource" IS 'Business resource protected by the permission. Example: WorkOrder, Invoice, Customer.';
    COMMENT ON COLUMN identity."FgsPermission"."Action" IS 'Operation allowed by the permission. Example: View, Create, Edit, Delete, Approve, Dispatch.';
    COMMENT ON COLUMN identity."FgsPermission"."Name" IS 'User-friendly permission name displayed in the application.';
    COMMENT ON COLUMN identity."FgsPermission"."Description" IS 'Optional description explaining the purpose of the permission.';
    COMMENT ON COLUMN identity."FgsPermission"."DisplayOrder" IS 'Controls the display order of permissions within the user interface.';
    COMMENT ON COLUMN identity."FgsPermission"."IsActive" IS 'Indicates whether the permission is available for assignment.';
    COMMENT ON COLUMN identity."FgsPermission"."CreatedOn" IS 'Date and time the permission was created.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE TABLE identity."FgsApiRequestLog" (
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "FgsApiClientId" bigint NOT NULL,
        "RequestId" uuid NOT NULL,
        "Resource" character varying(100),
        "HttpMethod" character varying(10) NOT NULL,
        "Endpoint" character varying(255) NOT NULL,
        "HttpStatusCode" smallint NOT NULL,
        "DurationMilliseconds" integer NOT NULL,
        "ClientIpAddress" character varying(50),
        "UserAgent" character varying(500),
        "ErrorCode" character varying(100),
        "ErrorMessage" character varying(500),
        "RequestedOn" timestamptz NOT NULL,
        CONSTRAINT "PK_FgsApiRequestLog" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_FgsApiRequestLog_FgsApiClient" FOREIGN KEY ("FgsApiClientId") REFERENCES identity."FgsApiClient" ("Id") ON DELETE RESTRICT,
        CONSTRAINT "FK_FgsApiRequestLog_FgsTenantCompanyCache" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES identity."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT
    );
    COMMENT ON TABLE identity."FgsApiRequestLog" IS 'Stores API request metadata for monitoring, troubleshooting, rate limiting and analytics.';
    COMMENT ON COLUMN identity."FgsApiRequestLog"."RequestId" IS 'Unique identifier used to correlate request processing across services.';
    COMMENT ON COLUMN identity."FgsApiRequestLog"."Resource" IS 'Business resource targeted by the API request, such as WorkOrder, Customer, Estimate or Invoice. Used for reporting, analytics, monitoring and rate limiting.';
    COMMENT ON COLUMN identity."FgsApiRequestLog"."HttpMethod" IS 'HTTP method used by the request, such as GET, POST, PUT or DELETE.';
    COMMENT ON COLUMN identity."FgsApiRequestLog"."Endpoint" IS 'API endpoint requested by the client.';
    COMMENT ON COLUMN identity."FgsApiRequestLog"."HttpStatusCode" IS 'HTTP response status code returned to the client.';
    COMMENT ON COLUMN identity."FgsApiRequestLog"."DurationMilliseconds" IS 'Total request processing time in milliseconds.';
    COMMENT ON COLUMN identity."FgsApiRequestLog"."ClientIpAddress" IS 'IP address from which the API request originated.';
    COMMENT ON COLUMN identity."FgsApiRequestLog"."UserAgent" IS 'User-Agent header supplied by the client application.';
    COMMENT ON COLUMN identity."FgsApiRequestLog"."ErrorCode" IS 'Application-specific error code returned for failed requests.';
    COMMENT ON COLUMN identity."FgsApiRequestLog"."ErrorMessage" IS 'Brief error message associated with the failed request.';
    COMMENT ON COLUMN identity."FgsApiRequestLog"."RequestedOn" IS 'Date and time the API request was received.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE TABLE identity."FgsApiSecret" (
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "FgsApiClientId" bigint NOT NULL,
        "Name" character varying(100) NOT NULL,
        "SecretHash" character varying(500) NOT NULL,
        "ExpiresOn" timestamptz,
        "LastUsedOn" timestamptz,
        "RevokedOn" timestamptz,
        "RevokedBy" character varying(100),
        "IsActive" boolean NOT NULL DEFAULT TRUE,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" character varying(100) NOT NULL,
        CONSTRAINT "PK_FgsApiSecret" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_FgsApiSecret_FgsApiClient" FOREIGN KEY ("FgsApiClientId") REFERENCES identity."FgsApiClient" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_FgsApiSecret_FgsTenantCompanyCache" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES identity."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT
    );
    COMMENT ON TABLE identity."FgsApiSecret" IS 'Stores hashed API secrets associated with API clients. Supports secret rotation, expiration, revocation and auditing.';
    COMMENT ON COLUMN identity."FgsApiSecret"."Name" IS 'User-friendly name used to identify the secret, such as Production, Sandbox or July 2026 Rotation.';
    COMMENT ON COLUMN identity."FgsApiSecret"."SecretHash" IS 'Cryptographic hash of the API secret. The original secret is never stored and cannot be recovered.';
    COMMENT ON COLUMN identity."FgsApiSecret"."ExpiresOn" IS 'Date and time the secret expires. NULL indicates the secret does not expire.';
    COMMENT ON COLUMN identity."FgsApiSecret"."LastUsedOn" IS 'Date and time the secret was most recently used for successful authentication.';
    COMMENT ON COLUMN identity."FgsApiSecret"."RevokedOn" IS 'Date and time the secret was revoked.';
    COMMENT ON COLUMN identity."FgsApiSecret"."RevokedBy" IS 'User or system that revoked the secret.';
    COMMENT ON COLUMN identity."FgsApiSecret"."IsActive" IS 'Indicates whether the secret is currently valid for API authentication.';
    COMMENT ON COLUMN identity."FgsApiSecret"."CreatedOn" IS 'Date and time the secret was created.';
    COMMENT ON COLUMN identity."FgsApiSecret"."CreatedBy" IS 'User or system that created the secret.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE TABLE identity."FgsApiWebhookSubscription" (
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "FgsApiWebhookId" bigint NOT NULL,
        "FgsApiEventId" bigint NOT NULL,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" character varying(100) NOT NULL,
        CONSTRAINT "PK_FgsApiWebhookSubscription" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_FgsApiWebhookSubscription_FgsApiEvent" FOREIGN KEY ("FgsApiEventId") REFERENCES identity."FgsApiEvent" ("Id") ON DELETE RESTRICT,
        CONSTRAINT "FK_FgsApiWebhookSubscription_FgsApiWebhook" FOREIGN KEY ("FgsApiWebhookId") REFERENCES identity."FgsApiWebhook" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_FgsApiWebhookSubscription_FgsTenantCompanyCache" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES identity."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT
    );
    COMMENT ON TABLE identity."FgsApiWebhookSubscription" IS 'Assigns one or more API events to webhook endpoints for event delivery.';
    COMMENT ON COLUMN identity."FgsApiWebhookSubscription"."CreatedOn" IS 'Date and time the webhook subscription was created.';
    COMMENT ON COLUMN identity."FgsApiWebhookSubscription"."CreatedBy" IS 'User or system that created the webhook subscription.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE TABLE identity."FgsDataAccessScope" (
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "FgsDataAccessId" bigint NOT NULL,
        "ScopeType" character varying(50) NOT NULL,
        "Operator" character varying(20) NOT NULL,
        "ScopeValue" character varying(255),
        "DisplayOrder" smallint NOT NULL DEFAULT 1,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" character varying(100) NOT NULL,
        CONSTRAINT "PK_FgsDataAccessScope" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_FgsDataAccessScope_FgsDataAccess" FOREIGN KEY ("FgsDataAccessId") REFERENCES identity."FgsDataAccess" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_FgsDataAccessScope_FgsTenantCompanyCache" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES identity."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT
    );
    COMMENT ON TABLE identity."FgsDataAccessScope" IS 'Stores one or more scope rules that define the records included in a Data Access profile.';
    COMMENT ON COLUMN identity."FgsDataAccessScope"."ScopeType" IS 'Business entity used to restrict data, such as Company, BusinessUnit, Region, Warehouse, Technician or WorkOrder.';
    COMMENT ON COLUMN identity."FgsDataAccessScope"."Operator" IS 'Comparison operator used by the rule, such as ALL, IN, EQUALS, ASSIGNED_TO_CURRENT_USER or MANAGER_OF_CURRENT_USER.';
    COMMENT ON COLUMN identity."FgsDataAccessScope"."ScopeValue" IS 'Comparison value used by the rule. NULL when the operator does not require a value.';
    COMMENT ON COLUMN identity."FgsDataAccessScope"."DisplayOrder" IS 'Controls the order in which scope rules are evaluated and displayed.';
    COMMENT ON COLUMN identity."FgsDataAccessScope"."CreatedOn" IS 'Date and time the scope rule was created.';
    COMMENT ON COLUMN identity."FgsDataAccessScope"."CreatedBy" IS 'User or system that created the scope rule.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE TABLE identity."FgsRoleDataAccess" (
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "FgsRoleId" bigint NOT NULL,
        "FgsDataAccessId" bigint NOT NULL,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" character varying(100) NOT NULL,
        CONSTRAINT "PK_FgsRoleDataAccess" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_FgsRoleDataAccess_FgsDataAccess" FOREIGN KEY ("FgsDataAccessId") REFERENCES identity."FgsDataAccess" ("Id") ON DELETE RESTRICT,
        CONSTRAINT "FK_FgsRoleDataAccess_FgsRole" FOREIGN KEY ("FgsRoleId") REFERENCES identity."FgsRole" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_FgsRoleDataAccess_FgsTenantCompanyCache" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES identity."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT
    );
    COMMENT ON TABLE identity."FgsRoleDataAccess" IS 'Assigns one or more data access profiles to security roles within a company.';
    COMMENT ON COLUMN identity."FgsRoleDataAccess"."CreatedOn" IS 'Date and time the data access profile was assigned to the role.';
    COMMENT ON COLUMN identity."FgsRoleDataAccess"."CreatedBy" IS 'User or system that assigned the data access profile to the role.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE TABLE identity."FgsRolePermission" (
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "FgsRoleId" bigint NOT NULL,
        "FgsPermissionId" bigint NOT NULL,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" character varying(100) NOT NULL,
        CONSTRAINT "PK_FgsRolePermission" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_FgsRolePermission_FgsPermission" FOREIGN KEY ("FgsPermissionId") REFERENCES identity."FgsPermission" ("Id") ON DELETE RESTRICT,
        CONSTRAINT "FK_FgsRolePermission_FgsRole" FOREIGN KEY ("FgsRoleId") REFERENCES identity."FgsRole" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_FgsRolePermission_FgsTenantCompanyCache" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES identity."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT
    );
    COMMENT ON TABLE identity."FgsRolePermission" IS 'Assigns permissions to security roles within a company.';
    COMMENT ON COLUMN identity."FgsRolePermission"."CreatedOn" IS 'Date and time the permission was assigned to the role.';
    COMMENT ON COLUMN identity."FgsRolePermission"."CreatedBy" IS 'User or system that assigned the permission to the role.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE UNIQUE INDEX "IX_FgsUserRole_TenantId_CompanyId_UserId_FgsRoleId" ON identity."FgsUserRole" ("TenantId", "CompanyId", "UserId", "FgsRoleId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE INDEX "IX_FgsRole_ParentRoleId" ON identity."FgsRole" ("ParentRoleId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE INDEX "IX_FgsRole_TenantId_CompanyId_IsBuiltIn" ON identity."FgsRole" ("TenantId", "CompanyId", "IsBuiltIn");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE INDEX "IX_FgsRole_TenantId_CompanyId_Name" ON identity."FgsRole" ("TenantId", "CompanyId", "Name");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE INDEX "IX_FgsApiClient_TenantId_CompanyId" ON identity."FgsApiClient" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE UNIQUE INDEX "UX_FgsApiClient_ClientId" ON identity."FgsApiClient" ("ClientId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE UNIQUE INDEX "UX_FgsApiClient_TenantId_CompanyId_ApplicationName" ON identity."FgsApiClient" ("TenantId", "CompanyId", "ApplicationName");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE INDEX "IX_FgsApiEvent_EventCategory" ON identity."FgsApiEvent" ("EventCategory");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE UNIQUE INDEX "IX_FgsApiEvent_EventCode" ON identity."FgsApiEvent" ("EventCode");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE INDEX "IX_FgsApiRequestLog_FgsApiClientId" ON identity."FgsApiRequestLog" ("FgsApiClientId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE INDEX "IX_FgsApiRequestLog_HttpStatusCode" ON identity."FgsApiRequestLog" ("HttpStatusCode");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE INDEX "IX_FgsApiRequestLog_RequestedOn" ON identity."FgsApiRequestLog" ("RequestedOn");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE UNIQUE INDEX "IX_FgsApiRequestLog_RequestId" ON identity."FgsApiRequestLog" ("RequestId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE INDEX "IX_FgsApiRequestLog_TenantId_CompanyId" ON identity."FgsApiRequestLog" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE INDEX "IX_FgsApiSecret_FgsApiClientId" ON identity."FgsApiSecret" ("FgsApiClientId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE INDEX "IX_FgsApiSecret_TenantId_CompanyId" ON identity."FgsApiSecret" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE UNIQUE INDEX "UX_FgsApiSecret_TenantId_CompanyId_Client_Name" ON identity."FgsApiSecret" ("TenantId", "CompanyId", "FgsApiClientId", "Name");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE INDEX "IX_FgsApiWebhook_IsActive" ON identity."FgsApiWebhook" ("TenantId", "CompanyId", "IsActive");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE INDEX "IX_FgsApiWebhook_Name" ON identity."FgsApiWebhook" ("TenantId", "CompanyId", "Name");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE INDEX "IX_FgsApiWebhook_TenantId_CompanyId" ON identity."FgsApiWebhook" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE INDEX "IX_FgsApiWebhookSubscription_FgsApiEventId" ON identity."FgsApiWebhookSubscription" ("FgsApiEventId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE INDEX "IX_FgsApiWebhookSubscription_FgsApiWebhookId" ON identity."FgsApiWebhookSubscription" ("FgsApiWebhookId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE INDEX "IX_FgsApiWebhookSubscription_TenantId_CompanyId" ON identity."FgsApiWebhookSubscription" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE UNIQUE INDEX "IX_FgsApiWebhookSubscription_TenantId_CompanyId_Webhook_Event" ON identity."FgsApiWebhookSubscription" ("TenantId", "CompanyId", "FgsApiWebhookId", "FgsApiEventId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE UNIQUE INDEX "IX_FgsDataAccess_TenantId_CompanyId_DataAccessCode" ON identity."FgsDataAccess" ("TenantId", "CompanyId", "DataAccessCode");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE INDEX "IX_FgsDataAccess_TenantId_CompanyId_IsBuiltIn" ON identity."FgsDataAccess" ("TenantId", "CompanyId", "IsBuiltIn");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE INDEX "IX_FgsDataAccess_TenantId_CompanyId_Name" ON identity."FgsDataAccess" ("TenantId", "CompanyId", "Name");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE INDEX "IX_FgsDataAccessScope_FgsDataAccessId" ON identity."FgsDataAccessScope" ("FgsDataAccessId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE INDEX "IX_FgsDataAccessScope_ScopeType" ON identity."FgsDataAccessScope" ("ScopeType");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE INDEX "IX_FgsDataAccessScope_TenantId_CompanyId" ON identity."FgsDataAccessScope" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE INDEX "IX_FgsPermission_Module" ON identity."FgsPermission" ("Module");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE INDEX "IX_FgsPermission_Module_Resource" ON identity."FgsPermission" ("Module", "Resource");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE UNIQUE INDEX "IX_FgsPermission_PermissionCode" ON identity."FgsPermission" ("PermissionCode");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE INDEX "IX_FgsPermission_Resource" ON identity."FgsPermission" ("Resource");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE INDEX "IX_FgsRoleDataAccess_FgsDataAccessId" ON identity."FgsRoleDataAccess" ("FgsDataAccessId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE INDEX "IX_FgsRoleDataAccess_FgsRoleId" ON identity."FgsRoleDataAccess" ("FgsRoleId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE INDEX "IX_FgsRoleDataAccess_TenantId_CompanyId" ON identity."FgsRoleDataAccess" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE UNIQUE INDEX "IX_FgsRoleDataAccess_TenantId_CompanyId_Role_DataAccess" ON identity."FgsRoleDataAccess" ("TenantId", "CompanyId", "FgsRoleId", "FgsDataAccessId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE INDEX "IX_FgsRolePermission_FgsPermissionId" ON identity."FgsRolePermission" ("FgsPermissionId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE INDEX "IX_FgsRolePermission_FgsRoleId" ON identity."FgsRolePermission" ("FgsRoleId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE INDEX "IX_FgsRolePermission_TenantId_CompanyId" ON identity."FgsRolePermission" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    CREATE UNIQUE INDEX "IX_FgsRolePermission_TenantId_CompanyId_Role_Permission" ON identity."FgsRolePermission" ("TenantId", "CompanyId", "FgsRoleId", "FgsPermissionId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    ALTER TABLE identity."FgsRole" ADD CONSTRAINT "FK_FgsRole_FgsTenantCompanyCache" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES identity."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    ALTER TABLE identity."FgsRole" ADD CONSTRAINT "FK_FgsRole_ParentRole" FOREIGN KEY ("ParentRoleId") REFERENCES identity."FgsRole" ("Id") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    ALTER TABLE identity."FgsUser" ADD CONSTRAINT "FK_FgsUser_FgsTenantCompanyCache" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES identity."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    ALTER TABLE identity."FgsUserRole" ADD CONSTRAINT "FK_FgsUserRole_FgsTenantCompanyCache" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES identity."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    -- Drop the temporary EF default used while converting FgsRoleId to NOT NULL
    ALTER TABLE identity."FgsUserRole" ALTER COLUMN "FgsRoleId" DROP DEFAULT;

    -- Complete DDL column comments not emitted by EF for key/audit/FK identity columns
    COMMENT ON COLUMN identity."FgsRole"."Id" IS 'Unique identifier for the role.';
    COMMENT ON COLUMN identity."FgsRole"."TenantId" IS 'Tenant that owns the role.';
    COMMENT ON COLUMN identity."FgsRole"."CompanyId" IS 'Company that owns the role. Roles are scoped to a company within a tenant.';
    COMMENT ON COLUMN identity."FgsRole"."CreatedBy" IS 'User or system that created the role.';
    COMMENT ON COLUMN identity."FgsRole"."UpdatedBy" IS 'User or system that last modified the role.';

    COMMENT ON COLUMN identity."FgsUserRole"."Id" IS 'Unique identifier for the user role assignment.';
    COMMENT ON COLUMN identity."FgsUserRole"."TenantId" IS 'Tenant that owns the role assignment.';
    COMMENT ON COLUMN identity."FgsUserRole"."CompanyId" IS 'Company within the tenant where the role assignment applies.';
    COMMENT ON COLUMN identity."FgsUserRole"."UserId" IS 'User receiving the assigned security role.';
    COMMENT ON COLUMN identity."FgsUserRole"."FgsRoleId" IS 'Security role assigned to the user.';

    COMMENT ON COLUMN identity."FgsPermission"."Id" IS 'Unique identifier for the permission.';

    COMMENT ON COLUMN identity."FgsRolePermission"."Id" IS 'Unique identifier for the role permission assignment.';
    COMMENT ON COLUMN identity."FgsRolePermission"."TenantId" IS 'Tenant that owns the role permission assignment.';
    COMMENT ON COLUMN identity."FgsRolePermission"."CompanyId" IS 'Company within the tenant where the role permission assignment applies.';
    COMMENT ON COLUMN identity."FgsRolePermission"."FgsRoleId" IS 'Security role receiving the permission.';
    COMMENT ON COLUMN identity."FgsRolePermission"."FgsPermissionId" IS 'Permission assigned to the role.';

    COMMENT ON COLUMN identity."FgsDataAccess"."Id" IS 'Unique identifier for the data access profile.';
    COMMENT ON COLUMN identity."FgsDataAccess"."TenantId" IS 'Tenant that owns the data access profile.';
    COMMENT ON COLUMN identity."FgsDataAccess"."CompanyId" IS 'Company that owns the data access profile.';
    COMMENT ON COLUMN identity."FgsDataAccess"."CreatedOn" IS 'Date and time the data access profile was created.';
    COMMENT ON COLUMN identity."FgsDataAccess"."CreatedBy" IS 'User or system that created the data access profile.';
    COMMENT ON COLUMN identity."FgsDataAccess"."UpdatedOn" IS 'Date and time the data access profile was last modified.';
    COMMENT ON COLUMN identity."FgsDataAccess"."UpdatedBy" IS 'User or system that last modified the data access profile.';

    COMMENT ON COLUMN identity."FgsDataAccessScope"."Id" IS 'Unique identifier for the scope rule.';
    COMMENT ON COLUMN identity."FgsDataAccessScope"."TenantId" IS 'Tenant that owns the scope rule.';
    COMMENT ON COLUMN identity."FgsDataAccessScope"."CompanyId" IS 'Company that owns the scope rule.';
    COMMENT ON COLUMN identity."FgsDataAccessScope"."FgsDataAccessId" IS 'Data access profile that owns this scope rule.';

    COMMENT ON COLUMN identity."FgsRoleDataAccess"."Id" IS 'Unique identifier for the role data access assignment.';
    COMMENT ON COLUMN identity."FgsRoleDataAccess"."TenantId" IS 'Tenant that owns the role data access assignment.';
    COMMENT ON COLUMN identity."FgsRoleDataAccess"."CompanyId" IS 'Company within the tenant where the data access assignment applies.';
    COMMENT ON COLUMN identity."FgsRoleDataAccess"."FgsRoleId" IS 'Security role receiving the data access profile.';
    COMMENT ON COLUMN identity."FgsRoleDataAccess"."FgsDataAccessId" IS 'Data access profile assigned to the security role.';

    COMMENT ON COLUMN identity."FgsApiEvent"."Id" IS 'Unique identifier for the API event.';

    COMMENT ON COLUMN identity."FgsApiWebhook"."Id" IS 'Unique identifier for the webhook endpoint.';
    COMMENT ON COLUMN identity."FgsApiWebhook"."TenantId" IS 'Tenant that owns the webhook endpoint.';
    COMMENT ON COLUMN identity."FgsApiWebhook"."CompanyId" IS 'Company that owns the webhook endpoint.';
    COMMENT ON COLUMN identity."FgsApiWebhook"."CreatedOn" IS 'Date and time the webhook endpoint was created.';
    COMMENT ON COLUMN identity."FgsApiWebhook"."CreatedBy" IS 'User or system that created the webhook endpoint.';
    COMMENT ON COLUMN identity."FgsApiWebhook"."UpdatedOn" IS 'Date and time the webhook endpoint was last modified.';
    COMMENT ON COLUMN identity."FgsApiWebhook"."UpdatedBy" IS 'User or system that last modified the webhook endpoint.';

    COMMENT ON COLUMN identity."FgsApiWebhookSubscription"."Id" IS 'Unique identifier for the webhook event subscription.';
    COMMENT ON COLUMN identity."FgsApiWebhookSubscription"."TenantId" IS 'Tenant that owns the webhook subscription.';
    COMMENT ON COLUMN identity."FgsApiWebhookSubscription"."CompanyId" IS 'Company that owns the webhook subscription.';
    COMMENT ON COLUMN identity."FgsApiWebhookSubscription"."FgsApiWebhookId" IS 'Webhook endpoint receiving the subscribed event.';
    COMMENT ON COLUMN identity."FgsApiWebhookSubscription"."FgsApiEventId" IS 'Public API event delivered to the webhook endpoint.';

    COMMENT ON COLUMN identity."FgsApiRequestLog"."Id" IS 'Unique identifier for the API request log entry.';
    COMMENT ON COLUMN identity."FgsApiRequestLog"."TenantId" IS 'Tenant that initiated the API request.';
    COMMENT ON COLUMN identity."FgsApiRequestLog"."CompanyId" IS 'Company that initiated the API request.';
    COMMENT ON COLUMN identity."FgsApiRequestLog"."FgsApiClientId" IS 'API client that submitted the request.';

    COMMENT ON COLUMN identity."FgsApiClient"."Id" IS 'Unique identifier for the API client application.';
    COMMENT ON COLUMN identity."FgsApiClient"."TenantId" IS 'Tenant that owns the API client application.';
    COMMENT ON COLUMN identity."FgsApiClient"."CompanyId" IS 'Company that owns the API client application.';
    COMMENT ON COLUMN identity."FgsApiClient"."CreatedOn" IS 'Date and time the application was registered.';
    COMMENT ON COLUMN identity."FgsApiClient"."CreatedBy" IS 'User or system that registered the application.';
    COMMENT ON COLUMN identity."FgsApiClient"."UpdatedOn" IS 'Date and time the application was last modified.';
    COMMENT ON COLUMN identity."FgsApiClient"."UpdatedBy" IS 'User or system that last modified the application.';

    COMMENT ON COLUMN identity."FgsApiSecret"."Id" IS 'Unique identifier for the API secret.';
    COMMENT ON COLUMN identity."FgsApiSecret"."TenantId" IS 'Tenant that owns the API secret.';
    COMMENT ON COLUMN identity."FgsApiSecret"."CompanyId" IS 'Company that owns the API secret.';
    COMMENT ON COLUMN identity."FgsApiSecret"."FgsApiClientId" IS 'API client application that owns this secret.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM identity."__EFMigrationsHistory" WHERE "MigrationId" = '20260713180610_AddIdentityAuthorizationAndApiEntities') THEN
    INSERT INTO identity."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260713180610_AddIdentityAuthorizationAndApiEntities', '10.0.8');
    END IF;
END $EF$;
COMMIT;

