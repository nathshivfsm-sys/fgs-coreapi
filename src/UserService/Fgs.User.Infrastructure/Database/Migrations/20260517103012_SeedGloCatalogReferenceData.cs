using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.User.Infrastructure.Database.Migrations;

/// <inheritdoc />
public partial class SeedGloCatalogReferenceData : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "CultureCode",
            schema: "dbo",
            table: "GloLanguage",
            type: "character varying(10)",
            maxLength: 10,
            nullable: false,
            defaultValue: "");

        migrationBuilder.Sql("""ALTER TABLE dbo."GloLanguage" ALTER COLUMN "CultureCode" DROP DEFAULT;""");

        migrationBuilder.Sql(
            """
            DO $seed$
            DECLARE
                v_now timestamptz := timezone('utc', now());
                v_system uuid := '00000000-0000-0000-0000-000000000001';
            BEGIN
                INSERT INTO dbo."GloLocationType" ("Id", "Code", "Name", "IsActive", "CreatedOn", "UpdatedOn")
                VALUES
                    (1, 'BILLING', 'BILLING', true, v_now, v_now),
                    (2, 'SHIPPING', 'SHIPPING', true, v_now, v_now),
                    (3, 'PHYSICAL', 'PHYSICAL', true, v_now, v_now),
                    (4, 'SERVICE', 'SERVICE', true, v_now, v_now),
                    (5, 'MAILING', 'MAILING', true, v_now, v_now),
                    (6, 'HQ', 'HQ', true, v_now, v_now),
                    (7, 'REMITTO', 'REMITTO', true, v_now, v_now),
                    (8, 'JOBSITE', 'JOBSITE', true, v_now, v_now)
                ON CONFLICT ("Code") DO NOTHING;

                INSERT INTO dbo."GloMasterEntityType" ("Id", "Code", "IsDocumentAllowed", "IsActive", "SortOrder", "CreatedOn", "UpdatedOn", "CreatedBy", "UpdatedBy")
                VALUES
                    (1, 'TENANT', true, true, 1, v_now, v_now, v_system, v_system),
                    (2, 'COMPANY', true, true, 2, v_now, v_now, v_system, v_system),
                    (3, 'SERVICELOCATION', true, true, 3, v_now, v_now, v_system, v_system),
                    (4, 'BILLTO', true, true, 4, v_now, v_now, v_system, v_system),
                    (5, 'VENDOR', true, true, 5, v_now, v_now, v_system, v_system),
                    (6, 'SUBCONTRACTOR', true, true, 6, v_now, v_now, v_system, v_system),
                    (7, 'LEAD', true, true, 7, v_now, v_now, v_system, v_system),
                    (8, 'PROPOSAL', true, true, 8, v_now, v_now, v_system, v_system),
                    (9, 'CUSTOMER', true, true, 9, v_now, v_now, v_system, v_system),
                    (10, 'WORKORDER', true, true, 10, v_now, v_now, v_system, v_system),
                    (11, 'INVOICE', true, true, 11, v_now, v_now, v_system, v_system)
                ON CONFLICT ("Code") DO NOTHING;

                INSERT INTO dbo."GloLanguage" ("LanguageCode", "LanguageName", "CultureCode", "IsActive")
                VALUES
                    ('EN', 'English', 'en-US', true),
                    ('ES', 'Spanish', 'es-US', true),
                    ('FR', 'French', 'fr-FR', true)
                ON CONFLICT ("LanguageCode") DO UPDATE
                SET "LanguageName" = EXCLUDED."LanguageName",
                    "CultureCode" = EXCLUDED."CultureCode",
                    "IsActive" = EXCLUDED."IsActive";

                INSERT INTO dbo."GloAccountingIntegrationType" ("Id", "Code", "Name", "IsActive", "CreatedOn", "UpdatedOn")
                VALUES
                    (1, 'NONE', 'No Accounting', true, v_now, v_now),
                    (2, 'QUICKBOOKSONLINE', 'QuickBooks Online', true, v_now, v_now),
                    (3, 'SAGEINTACCT', 'Sage Intacct', true, v_now, v_now)
                ON CONFLICT ("Code") DO NOTHING;

                INSERT INTO dbo."GloTimeCardOption" ("Id", "Code", "Name")
                VALUES
                    (1, 'NONE', 'No formal technician time tracking workflow'),
                    (2, 'DISPATCHARRIVECOMPLETE', 'Tracks dispatch, arrival, and completion timestamps'),
                    (3, 'CHECKINCHECKOUT', 'Technician manually checks in and checks out')
                ON CONFLICT ("Code") DO NOTHING;

                INSERT INTO dbo."GloBusinessType" ("Id", "Code", "Name", "IsActive", "CreatedOn", "UpdatedOn")
                VALUES
                    (1, 'HVAC', 'HVAC', true, v_now, v_now),
                    (2, 'PLUMBING', 'Plumbing', true, v_now, v_now),
                    (3, 'ELECTRICAL', 'Electrical', true, v_now, v_now),
                    (4, 'PESTCONTROL', 'Pest Control', true, v_now, v_now),
                    (5, 'LAWNCARE', 'Lawn Care', true, v_now, v_now),
                    (6, 'TRASHPICKUP', 'Trash Pickup', true, v_now, v_now),
                    (7, 'GARAGEDOOR', 'Garage Door', true, v_now, v_now),
                    (8, 'HOUSECLEANING', 'House Cleaning', true, v_now, v_now),
                    (9, 'PAINTING', 'Painting', true, v_now, v_now)
                ON CONFLICT ("Code") DO NOTHING;

                PERFORM setval(pg_get_serial_sequence('dbo."GloLocationType"', 'Id'), COALESCE((SELECT MAX("Id") FROM dbo."GloLocationType"), 1), true);
                PERFORM setval(pg_get_serial_sequence('dbo."GloMasterEntityType"', 'Id'), COALESCE((SELECT MAX("Id") FROM dbo."GloMasterEntityType"), 1), true);
                PERFORM setval(pg_get_serial_sequence('dbo."GloAccountingIntegrationType"', 'Id'), COALESCE((SELECT MAX("Id") FROM dbo."GloAccountingIntegrationType"), 1), true);
                PERFORM setval(pg_get_serial_sequence('dbo."GloTimeCardOption"', 'Id'), COALESCE((SELECT MAX("Id") FROM dbo."GloTimeCardOption"), 1), true);
                PERFORM setval(pg_get_serial_sequence('dbo."GloBusinessType"', 'Id'), COALESCE((SELECT MAX("Id") FROM dbo."GloBusinessType"), 1), true);
            END $seed$;
            """);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(
            """
            DELETE FROM dbo."GloBusinessType"
            WHERE "Code" IN ('HVAC', 'PLUMBING', 'ELECTRICAL', 'PESTCONTROL', 'LAWNCARE', 'TRASHPICKUP', 'GARAGEDOOR', 'HOUSECLEANING', 'PAINTING');

            DELETE FROM dbo."GloTimeCardOption"
            WHERE "Code" IN ('NONE', 'DISPATCHARRIVECOMPLETE', 'CHECKINCHECKOUT');

            DELETE FROM dbo."GloAccountingIntegrationType"
            WHERE "Code" IN ('NONE', 'QUICKBOOKSONLINE', 'SAGEINTACCT');

            DELETE FROM dbo."GloLanguage"
            WHERE "LanguageCode" IN ('EN', 'ES', 'FR');

            DELETE FROM dbo."GloMasterEntityType"
            WHERE "Code" IN ('TENANT', 'COMPANY', 'SERVICELOCATION', 'BILLTO', 'VENDOR', 'SUBCONTRACTOR', 'LEAD', 'PROPOSAL', 'CUSTOMER', 'WORKORDER', 'INVOICE');

            DELETE FROM dbo."GloLocationType"
            WHERE "Code" IN ('BILLING', 'SHIPPING', 'PHYSICAL', 'SERVICE', 'MAILING', 'HQ', 'REMITTO', 'JOBSITE');
            """);

        migrationBuilder.DropColumn(
            name: "CultureCode",
            schema: "dbo",
            table: "GloLanguage");
    }
}
