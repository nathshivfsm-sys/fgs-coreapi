START TRANSACTION;

---------------------------------------------------------------------------
-- Expand crm.CrmServiceLocation with address, defaults, templates,
-- communication preferences, status, and audit columns.
---------------------------------------------------------------------------

ALTER TABLE crm."CrmServiceLocation"
    ADD COLUMN IF NOT EXISTS "Name" character varying(200) NOT NULL DEFAULT '',
    ADD COLUMN IF NOT EXISTS "DisplayName" character varying(200) NOT NULL DEFAULT '',
    ADD COLUMN IF NOT EXISTS "ServiceLocationTypeId" smallint NOT NULL DEFAULT 0,
    ADD COLUMN IF NOT EXISTS "AddressLine1" character varying(200),
    ADD COLUMN IF NOT EXISTS "AddressLine2" character varying(200),
    ADD COLUMN IF NOT EXISTS "AddressLine3" character varying(200),
    ADD COLUMN IF NOT EXISTS "AddressLine4" character varying(200),
    ADD COLUMN IF NOT EXISTS "City" character varying(100),
    ADD COLUMN IF NOT EXISTS "State" character varying(100),
    ADD COLUMN IF NOT EXISTS "County" character varying(100),
    ADD COLUMN IF NOT EXISTS "Country" character varying(100),
    ADD COLUMN IF NOT EXISTS "PostalCode" character varying(20),
    ADD COLUMN IF NOT EXISTS "FormattedAddress" character varying(1000),
    ADD COLUMN IF NOT EXISTS "Latitude" numeric(18,10),
    ADD COLUMN IF NOT EXISTS "Longitude" numeric(18,10),
    ADD COLUMN IF NOT EXISTS "PlaceId" character varying(500),
    ADD COLUMN IF NOT EXISTS "DefaultPaymentMethodId" bigint,
    ADD COLUMN IF NOT EXISTS "DefaultMaterialPricingMatrixId" bigint,
    ADD COLUMN IF NOT EXISTS "DefaultLaborPricingMatrixId" bigint,
    ADD COLUMN IF NOT EXISTS "DefaultOtherPricingMatrixId" bigint,
    ADD COLUMN IF NOT EXISTS "InvoiceEmailTemplateId" bigint,
    ADD COLUMN IF NOT EXISTS "EstimateEmailTemplateId" bigint,
    ADD COLUMN IF NOT EXISTS "InvoiceSmsTemplateId" bigint,
    ADD COLUMN IF NOT EXISTS "EstimateSmsTemplateId" bigint,
    ADD COLUMN IF NOT EXISTS "TaxExempt" boolean NOT NULL DEFAULT FALSE,
    ADD COLUMN IF NOT EXISTS "EmailAllowed" boolean NOT NULL DEFAULT TRUE,
    ADD COLUMN IF NOT EXISTS "SmsAllowed" boolean NOT NULL DEFAULT TRUE,
    ADD COLUMN IF NOT EXISTS "IsActive" boolean NOT NULL DEFAULT TRUE,
    ADD COLUMN IF NOT EXISTS "CreatedOn" timestamptz NOT NULL DEFAULT (now()),
    ADD COLUMN IF NOT EXISTS "CreatedBy" character varying(100),
    ADD COLUMN IF NOT EXISTS "UpdatedOn" timestamptz,
    ADD COLUMN IF NOT EXISTS "UpdatedBy" character varying(100);

-- ============================================================================
-- COMMENTS
-- ============================================================================

COMMENT ON TABLE crm."CrmServiceLocation"
IS 'Physical customer locations where field service work is performed.';

COMMENT ON COLUMN crm."CrmServiceLocation"."TenantId"
IS 'Tenant identifier.';

COMMENT ON COLUMN crm."CrmServiceLocation"."CompanyId"
IS 'Company identifier.';

COMMENT ON COLUMN crm."CrmServiceLocation"."Id"
IS 'Primary key.';

COMMENT ON COLUMN crm."CrmServiceLocation"."CustomerId"
IS 'Customer that owns this service location.';

COMMENT ON COLUMN crm."CrmServiceLocation"."LocationSequence"
IS 'Sequential location number within a customer.';

COMMENT ON COLUMN crm."CrmServiceLocation"."LocationNumber"
IS 'Business identifier for the service location.';

COMMENT ON COLUMN crm."CrmServiceLocation"."Name"
IS 'Internal service location name.';

COMMENT ON COLUMN crm."CrmServiceLocation"."DisplayName"
IS 'Display name shown to users and customers.';

COMMENT ON COLUMN crm."CrmServiceLocation"."ServiceLocationTypeId"
IS 'Lookup to service location type.';

COMMENT ON COLUMN crm."CrmServiceLocation"."AddressLine1"
IS 'Primary street address.';

COMMENT ON COLUMN crm."CrmServiceLocation"."AddressLine2"
IS 'Secondary street address.';

COMMENT ON COLUMN crm."CrmServiceLocation"."AddressLine3"
IS 'Additional address information.';

COMMENT ON COLUMN crm."CrmServiceLocation"."AddressLine4"
IS 'Additional address information.';

COMMENT ON COLUMN crm."CrmServiceLocation"."City"
IS 'City.';

COMMENT ON COLUMN crm."CrmServiceLocation"."State"
IS 'State or province.';

COMMENT ON COLUMN crm."CrmServiceLocation"."County"
IS 'County or district.';

COMMENT ON COLUMN crm."CrmServiceLocation"."Country"
IS 'Country.';

COMMENT ON COLUMN crm."CrmServiceLocation"."PostalCode"
IS 'Postal or ZIP code.';

COMMENT ON COLUMN crm."CrmServiceLocation"."FormattedAddress"
IS 'Formatted address returned by mapping provider.';

COMMENT ON COLUMN crm."CrmServiceLocation"."Latitude"
IS 'Latitude coordinate.';

COMMENT ON COLUMN crm."CrmServiceLocation"."Longitude"
IS 'Longitude coordinate.';

COMMENT ON COLUMN crm."CrmServiceLocation"."PlaceId"
IS 'Google or mapping provider Place Id.';

COMMENT ON COLUMN crm."CrmServiceLocation"."DefaultPaymentMethodId"
IS 'Default payment method for this location.';

COMMENT ON COLUMN crm."CrmServiceLocation"."DefaultMaterialPricingMatrixId"
IS 'Default material pricing matrix.';

COMMENT ON COLUMN crm."CrmServiceLocation"."DefaultLaborPricingMatrixId"
IS 'Default labor pricing matrix.';

COMMENT ON COLUMN crm."CrmServiceLocation"."DefaultOtherPricingMatrixId"
IS 'Default miscellaneous pricing matrix.';

COMMENT ON COLUMN crm."CrmServiceLocation"."InvoiceEmailTemplateId"
IS 'Default invoice email template.';

COMMENT ON COLUMN crm."CrmServiceLocation"."EstimateEmailTemplateId"
IS 'Default estimate email template.';

COMMENT ON COLUMN crm."CrmServiceLocation"."InvoiceSmsTemplateId"
IS 'Default invoice SMS template.';

COMMENT ON COLUMN crm."CrmServiceLocation"."EstimateSmsTemplateId"
IS 'Default estimate SMS template.';

COMMENT ON COLUMN crm."CrmServiceLocation"."TaxExempt"
IS 'Indicates whether this service location is tax exempt.';

COMMENT ON COLUMN crm."CrmServiceLocation"."EmailAllowed"
IS 'Whether email communication is permitted.';

COMMENT ON COLUMN crm."CrmServiceLocation"."SmsAllowed"
IS 'Whether SMS communication is permitted.';

COMMENT ON COLUMN crm."CrmServiceLocation"."IsActive"
IS 'Indicates whether this service location is active.';

COMMENT ON COLUMN crm."CrmServiceLocation"."CreatedOn"
IS 'Record creation timestamp.';

COMMENT ON COLUMN crm."CrmServiceLocation"."CreatedBy"
IS 'User that created the record.';

COMMENT ON COLUMN crm."CrmServiceLocation"."UpdatedOn"
IS 'Last update timestamp.';

COMMENT ON COLUMN crm."CrmServiceLocation"."UpdatedBy"
IS 'User that last updated the record.';

-- ============================================================================
-- INDEXES
-- ============================================================================

CREATE INDEX IF NOT EXISTS "IX_CrmServiceLocation_Name"
ON crm."CrmServiceLocation"
(
    "TenantId",
    "CompanyId",
    "Name"
);

CREATE INDEX IF NOT EXISTS "IX_CrmServiceLocation_DisplayName"
ON crm."CrmServiceLocation"
(
    "TenantId",
    "CompanyId",
    "DisplayName"
);

CREATE INDEX IF NOT EXISTS "IX_CrmServiceLocation_City"
ON crm."CrmServiceLocation"
(
    "TenantId",
    "CompanyId",
    "City"
);

CREATE INDEX IF NOT EXISTS "IX_CrmServiceLocation_State"
ON crm."CrmServiceLocation"
(
    "TenantId",
    "CompanyId",
    "State"
);

CREATE INDEX IF NOT EXISTS "IX_CrmServiceLocation_PostalCode"
ON crm."CrmServiceLocation"
(
    "TenantId",
    "CompanyId",
    "PostalCode"
);

CREATE INDEX IF NOT EXISTS "IX_CrmServiceLocation_PlaceId"
ON crm."CrmServiceLocation"
(
    "TenantId",
    "CompanyId",
    "PlaceId"
);

CREATE INDEX IF NOT EXISTS "IX_CrmServiceLocation_IsActive"
ON crm."CrmServiceLocation"
(
    "TenantId",
    "CompanyId",
    "IsActive"
);

INSERT INTO crm."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260804042818_ExpandCrmServiceLocation', '10.0.8')
ON CONFLICT ("MigrationId") DO NOTHING;

COMMIT;
