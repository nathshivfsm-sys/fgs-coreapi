using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.User.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class SchemaCleanupAlignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsSetupCommunicationToken",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsTenantCompanyConfiguration",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_FgsCredentialProvider_TenantId_Code",
                schema: "dbo",
                table: "FgsCredentialProvider");

            migrationBuilder.DropColumn(
                name: "PaymentMethodType",
                schema: "dbo",
                table: "FgsSetupPaymentMethod");

            migrationBuilder.DropColumn(
                name: "LogoLocationId",
                schema: "dbo",
                table: "FgsSetupGLBreak");

            migrationBuilder.DropColumn(
                name: "GloMasterEntityTypeId",
                schema: "dbo",
                table: "FgsSetupCommunicationTemplate");

            migrationBuilder.DropColumn(
                name: "KmsKeyArn",
                schema: "dbo",
                table: "FgsCredentialSecret");

            migrationBuilder.DropColumn(
                name: "RegionName",
                schema: "dbo",
                table: "FgsCredentialSecret");

            migrationBuilder.DropColumn(
                name: "Remarks",
                schema: "dbo",
                table: "FgsCredentialSecret");

            migrationBuilder.DropColumn(
                name: "SecretArn",
                schema: "dbo",
                table: "FgsCredentialSecret");

            migrationBuilder.DropColumn(
                name: "VaultProvider",
                schema: "dbo",
                table: "FgsCredentialSecret");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "dbo",
                table: "FgsSetupGLBreak");

            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "text",
                nullable: true);

            migrationBuilder.RenameColumn(
                name: "EntityTypeId",
                schema: "dbo",
                table: "FgsLocation",
                newName: "MasterEntityTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_FgsLocation_EntityTypeId",
                schema: "dbo",
                table: "FgsLocation",
                newName: "IX_FgsLocation_MasterEntityTypeId");

            migrationBuilder.DropColumn(
                name: "RotationEnabled",
                schema: "dbo",
                table: "FgsCredentialSecret");

            migrationBuilder.AddColumn<bool>(
                name: "IsRevoked",
                schema: "dbo",
                table: "FgsCredentialSecret",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.RenameColumn(
                name: "RotatedOn",
                schema: "dbo",
                table: "FgsCredentialSecret",
                newName: "LastRotatedOn");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExpiresOn",
                schema: "dbo",
                table: "FgsCredentialSecret",
                type: "timestamptz",
                nullable: true);

            migrationBuilder.DropColumn(
                name: "LastValidatedOn",
                schema: "dbo",
                table: "FgsCredentialSecret");

            migrationBuilder.Sql(
                """
                -- PostgreSQL does not allow subqueries in ALTER COLUMN ... USING.
                -- Convert legacy bigint CompanyId (FgsTenantCompany.Id) to uuid (CompanyGuid) via temp column + UPDATE.

                DROP INDEX IF EXISTS dbo."IX_FgsSetupZone_TenantId_CompanyId";
                DROP INDEX IF EXISTS dbo."IX_FgsSetupTitleOfCourtesy_TenantId_CompanyId";
                DROP INDEX IF EXISTS dbo."IX_FgsSetupTimeSlot_TenantId_CompanyId";
                DROP INDEX IF EXISTS dbo."IX_FgsSetupTechTrade_TenantId_CompanyId";
                DROP INDEX IF EXISTS dbo."IX_FgsSetupTechSkillLevel_TenantId_CompanyId";
                DROP INDEX IF EXISTS dbo."IX_FgsSetupTaxDetail_TenantId_CompanyId";
                DROP INDEX IF EXISTS dbo."IX_FgsSetupTaxAuthority_TenantId_CompanyId";
                DROP INDEX IF EXISTS dbo."IX_FgsSetupTax_TenantId_CompanyId";
                DROP INDEX IF EXISTS dbo."IX_FgsSetupServiceAssetType_TenantId_CompanyId";
                DROP INDEX IF EXISTS dbo."IX_FgsSetupServiceAssetModelSerialDescription_TenantId_Company~";
                DROP INDEX IF EXISTS dbo."IX_FgsSetupServiceAssetMedia_TenantId_CompanyId";
                DROP INDEX IF EXISTS dbo."IX_FgsSetupServiceAssetManufacturer_TenantId_CompanyId";
                DROP INDEX IF EXISTS dbo."IX_FgsSetupPriceSheetOther_TenantId_CompanyId";
                DROP INDEX IF EXISTS dbo."IX_FgsSetupPriceSheetMaterialRange_TenantId_CompanyId";
                DROP INDEX IF EXISTS dbo."IX_FgsSetupPriceSheetMaterial_TenantId_CompanyId";
                DROP INDEX IF EXISTS dbo."IX_FgsSetupPriceSheetLaborTier_TenantId_CompanyId";
                DROP INDEX IF EXISTS dbo."IX_FgsSetupPriceSheetLabor_TenantId_CompanyId";
                DROP INDEX IF EXISTS dbo."IX_FgsSetupPriceSheet_TenantId_CompanyId";
                DROP INDEX IF EXISTS dbo."IX_FgsSetupPostalCode_TenantId_CompanyId";
                DROP INDEX IF EXISTS dbo."IX_FgsSetupPaymentTerm_TenantId_CompanyId";
                DROP INDEX IF EXISTS dbo."IX_FgsSetupPaymentMethod_TenantId_CompanyId";
                DROP INDEX IF EXISTS dbo."IX_FgsSetupGLBreak_TenantId_CompanyId";
                DROP INDEX IF EXISTS dbo."IX_FgsSetupDescription_TenantId_CompanyId";
                DROP INDEX IF EXISTS dbo."IX_FgsSetupCommunicationTemplate_TenantId_CompanyId";

                ALTER TABLE dbo."FgsSetupZone" ADD COLUMN "__EF_CompanyIdUuid" uuid;
                UPDATE dbo."FgsSetupZone" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
                ALTER TABLE dbo."FgsSetupZone" DROP COLUMN "CompanyId";
                ALTER TABLE dbo."FgsSetupZone" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
                ALTER TABLE dbo."FgsSetupZone" ALTER COLUMN "CompanyId" SET NOT NULL;

                ALTER TABLE dbo."FgsSetupTitleOfCourtesy" ADD COLUMN "__EF_CompanyIdUuid" uuid;
                UPDATE dbo."FgsSetupTitleOfCourtesy" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
                ALTER TABLE dbo."FgsSetupTitleOfCourtesy" DROP COLUMN "CompanyId";
                ALTER TABLE dbo."FgsSetupTitleOfCourtesy" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
                ALTER TABLE dbo."FgsSetupTitleOfCourtesy" ALTER COLUMN "CompanyId" SET NOT NULL;

                ALTER TABLE dbo."FgsSetupTimeSlot" ADD COLUMN "__EF_CompanyIdUuid" uuid;
                UPDATE dbo."FgsSetupTimeSlot" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
                ALTER TABLE dbo."FgsSetupTimeSlot" DROP COLUMN "CompanyId";
                ALTER TABLE dbo."FgsSetupTimeSlot" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
                ALTER TABLE dbo."FgsSetupTimeSlot" ALTER COLUMN "CompanyId" SET NOT NULL;

                ALTER TABLE dbo."FgsSetupTechTrade" ADD COLUMN "__EF_CompanyIdUuid" uuid;
                UPDATE dbo."FgsSetupTechTrade" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
                ALTER TABLE dbo."FgsSetupTechTrade" DROP COLUMN "CompanyId";
                ALTER TABLE dbo."FgsSetupTechTrade" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
                ALTER TABLE dbo."FgsSetupTechTrade" ALTER COLUMN "CompanyId" SET NOT NULL;

                ALTER TABLE dbo."FgsSetupTechSkillLevel" ADD COLUMN "__EF_CompanyIdUuid" uuid;
                UPDATE dbo."FgsSetupTechSkillLevel" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
                ALTER TABLE dbo."FgsSetupTechSkillLevel" DROP COLUMN "CompanyId";
                ALTER TABLE dbo."FgsSetupTechSkillLevel" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
                ALTER TABLE dbo."FgsSetupTechSkillLevel" ALTER COLUMN "CompanyId" SET NOT NULL;

                ALTER TABLE dbo."FgsSetupTaxDetail" ADD COLUMN "__EF_CompanyIdUuid" uuid;
                UPDATE dbo."FgsSetupTaxDetail" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
                ALTER TABLE dbo."FgsSetupTaxDetail" DROP COLUMN "CompanyId";
                ALTER TABLE dbo."FgsSetupTaxDetail" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
                ALTER TABLE dbo."FgsSetupTaxDetail" ALTER COLUMN "CompanyId" SET NOT NULL;

                ALTER TABLE dbo."FgsSetupTaxAuthority" ADD COLUMN "__EF_CompanyIdUuid" uuid;
                UPDATE dbo."FgsSetupTaxAuthority" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
                ALTER TABLE dbo."FgsSetupTaxAuthority" DROP COLUMN "CompanyId";
                ALTER TABLE dbo."FgsSetupTaxAuthority" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
                ALTER TABLE dbo."FgsSetupTaxAuthority" ALTER COLUMN "CompanyId" SET NOT NULL;

                ALTER TABLE dbo."FgsSetupTax" ADD COLUMN "__EF_CompanyIdUuid" uuid;
                UPDATE dbo."FgsSetupTax" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
                ALTER TABLE dbo."FgsSetupTax" DROP COLUMN "CompanyId";
                ALTER TABLE dbo."FgsSetupTax" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
                ALTER TABLE dbo."FgsSetupTax" ALTER COLUMN "CompanyId" SET NOT NULL;

                ALTER TABLE dbo."FgsSetupServiceAssetType" ADD COLUMN "__EF_CompanyIdUuid" uuid;
                UPDATE dbo."FgsSetupServiceAssetType" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
                ALTER TABLE dbo."FgsSetupServiceAssetType" DROP COLUMN "CompanyId";
                ALTER TABLE dbo."FgsSetupServiceAssetType" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
                ALTER TABLE dbo."FgsSetupServiceAssetType" ALTER COLUMN "CompanyId" SET NOT NULL;

                ALTER TABLE dbo."FgsSetupServiceAssetModelSerialDescription" ADD COLUMN "__EF_CompanyIdUuid" uuid;
                UPDATE dbo."FgsSetupServiceAssetModelSerialDescription" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
                ALTER TABLE dbo."FgsSetupServiceAssetModelSerialDescription" DROP COLUMN "CompanyId";
                ALTER TABLE dbo."FgsSetupServiceAssetModelSerialDescription" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
                ALTER TABLE dbo."FgsSetupServiceAssetModelSerialDescription" ALTER COLUMN "CompanyId" SET NOT NULL;

                ALTER TABLE dbo."FgsSetupServiceAssetMedia" ADD COLUMN "__EF_CompanyIdUuid" uuid;
                UPDATE dbo."FgsSetupServiceAssetMedia" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
                ALTER TABLE dbo."FgsSetupServiceAssetMedia" DROP COLUMN "CompanyId";
                ALTER TABLE dbo."FgsSetupServiceAssetMedia" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
                ALTER TABLE dbo."FgsSetupServiceAssetMedia" ALTER COLUMN "CompanyId" SET NOT NULL;

                ALTER TABLE dbo."FgsSetupServiceAssetManufacturer" ADD COLUMN "__EF_CompanyIdUuid" uuid;
                UPDATE dbo."FgsSetupServiceAssetManufacturer" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
                ALTER TABLE dbo."FgsSetupServiceAssetManufacturer" DROP COLUMN "CompanyId";
                ALTER TABLE dbo."FgsSetupServiceAssetManufacturer" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
                ALTER TABLE dbo."FgsSetupServiceAssetManufacturer" ALTER COLUMN "CompanyId" SET NOT NULL;

                ALTER TABLE dbo."FgsSetupPriceSheetOther" ADD COLUMN "__EF_CompanyIdUuid" uuid;
                UPDATE dbo."FgsSetupPriceSheetOther" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
                ALTER TABLE dbo."FgsSetupPriceSheetOther" DROP COLUMN "CompanyId";
                ALTER TABLE dbo."FgsSetupPriceSheetOther" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
                ALTER TABLE dbo."FgsSetupPriceSheetOther" ALTER COLUMN "CompanyId" SET NOT NULL;

                ALTER TABLE dbo."FgsSetupPriceSheetMaterialRange" ADD COLUMN "__EF_CompanyIdUuid" uuid;
                UPDATE dbo."FgsSetupPriceSheetMaterialRange" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
                ALTER TABLE dbo."FgsSetupPriceSheetMaterialRange" DROP COLUMN "CompanyId";
                ALTER TABLE dbo."FgsSetupPriceSheetMaterialRange" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
                ALTER TABLE dbo."FgsSetupPriceSheetMaterialRange" ALTER COLUMN "CompanyId" SET NOT NULL;

                ALTER TABLE dbo."FgsSetupPriceSheetMaterial" ADD COLUMN "__EF_CompanyIdUuid" uuid;
                UPDATE dbo."FgsSetupPriceSheetMaterial" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
                ALTER TABLE dbo."FgsSetupPriceSheetMaterial" DROP COLUMN "CompanyId";
                ALTER TABLE dbo."FgsSetupPriceSheetMaterial" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
                ALTER TABLE dbo."FgsSetupPriceSheetMaterial" ALTER COLUMN "CompanyId" SET NOT NULL;

                ALTER TABLE dbo."FgsSetupPriceSheetLaborTier" ADD COLUMN "__EF_CompanyIdUuid" uuid;
                UPDATE dbo."FgsSetupPriceSheetLaborTier" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
                ALTER TABLE dbo."FgsSetupPriceSheetLaborTier" DROP COLUMN "CompanyId";
                ALTER TABLE dbo."FgsSetupPriceSheetLaborTier" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
                ALTER TABLE dbo."FgsSetupPriceSheetLaborTier" ALTER COLUMN "CompanyId" SET NOT NULL;

                ALTER TABLE dbo."FgsSetupPriceSheetLabor" ADD COLUMN "__EF_CompanyIdUuid" uuid;
                UPDATE dbo."FgsSetupPriceSheetLabor" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
                ALTER TABLE dbo."FgsSetupPriceSheetLabor" DROP COLUMN "CompanyId";
                ALTER TABLE dbo."FgsSetupPriceSheetLabor" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
                ALTER TABLE dbo."FgsSetupPriceSheetLabor" ALTER COLUMN "CompanyId" SET NOT NULL;

                ALTER TABLE dbo."FgsSetupPriceSheet" ADD COLUMN "__EF_CompanyIdUuid" uuid;
                UPDATE dbo."FgsSetupPriceSheet" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
                ALTER TABLE dbo."FgsSetupPriceSheet" DROP COLUMN "CompanyId";
                ALTER TABLE dbo."FgsSetupPriceSheet" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
                ALTER TABLE dbo."FgsSetupPriceSheet" ALTER COLUMN "CompanyId" SET NOT NULL;

                ALTER TABLE dbo."FgsSetupPostalCode" ADD COLUMN "__EF_CompanyIdUuid" uuid;
                UPDATE dbo."FgsSetupPostalCode" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
                ALTER TABLE dbo."FgsSetupPostalCode" DROP COLUMN "CompanyId";
                ALTER TABLE dbo."FgsSetupPostalCode" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
                ALTER TABLE dbo."FgsSetupPostalCode" ALTER COLUMN "CompanyId" SET NOT NULL;

                ALTER TABLE dbo."FgsSetupPaymentTerm" ADD COLUMN "__EF_CompanyIdUuid" uuid;
                UPDATE dbo."FgsSetupPaymentTerm" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
                ALTER TABLE dbo."FgsSetupPaymentTerm" DROP COLUMN "CompanyId";
                ALTER TABLE dbo."FgsSetupPaymentTerm" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
                ALTER TABLE dbo."FgsSetupPaymentTerm" ALTER COLUMN "CompanyId" SET NOT NULL;

                ALTER TABLE dbo."FgsSetupPaymentMethod" ADD COLUMN "__EF_CompanyIdUuid" uuid;
                UPDATE dbo."FgsSetupPaymentMethod" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
                ALTER TABLE dbo."FgsSetupPaymentMethod" DROP COLUMN "CompanyId";
                ALTER TABLE dbo."FgsSetupPaymentMethod" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
                ALTER TABLE dbo."FgsSetupPaymentMethod" ALTER COLUMN "CompanyId" SET NOT NULL;

                ALTER TABLE dbo."FgsSetupGLBreak" ADD COLUMN "__EF_CompanyIdUuid" uuid;
                UPDATE dbo."FgsSetupGLBreak" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
                ALTER TABLE dbo."FgsSetupGLBreak" DROP COLUMN "CompanyId";
                ALTER TABLE dbo."FgsSetupGLBreak" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
                ALTER TABLE dbo."FgsSetupGLBreak" ALTER COLUMN "CompanyId" SET NOT NULL;

                ALTER TABLE dbo."FgsSetupDescription" ADD COLUMN "__EF_CompanyIdUuid" uuid;
                UPDATE dbo."FgsSetupDescription" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
                ALTER TABLE dbo."FgsSetupDescription" DROP COLUMN "CompanyId";
                ALTER TABLE dbo."FgsSetupDescription" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
                ALTER TABLE dbo."FgsSetupDescription" ALTER COLUMN "CompanyId" SET NOT NULL;

                ALTER TABLE dbo."FgsSetupCommunicationTemplate" ADD COLUMN "__EF_CompanyIdUuid" uuid;
                UPDATE dbo."FgsSetupCommunicationTemplate" z SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = z."TenantId" AND c."Id" = z."CompanyId";
                ALTER TABLE dbo."FgsSetupCommunicationTemplate" DROP COLUMN "CompanyId";
                ALTER TABLE dbo."FgsSetupCommunicationTemplate" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
                ALTER TABLE dbo."FgsSetupCommunicationTemplate" ALTER COLUMN "CompanyId" SET NOT NULL;

                ALTER TABLE dbo."FgsCredentialProvider" ADD COLUMN "__EF_CompanyIdUuid" uuid;
                UPDATE dbo."FgsCredentialProvider" p SET "__EF_CompanyIdUuid" = c."CompanyGuid" FROM dbo."FgsTenantCompany" c WHERE c."TenantId" = p."TenantId" AND p."CompanyId" IS NOT NULL AND c."Id" = p."CompanyId";
                UPDATE dbo."FgsCredentialProvider" p SET "__EF_CompanyIdUuid" = d."CompanyGuid" FROM (
                    SELECT DISTINCT ON (t."TenantId") t."TenantId", t."CompanyGuid" FROM dbo."FgsTenantCompany" t ORDER BY t."TenantId", t."Id"
                ) d WHERE p."__EF_CompanyIdUuid" IS NULL AND p."TenantId" = d."TenantId";
                ALTER TABLE dbo."FgsCredentialProvider" DROP COLUMN "CompanyId";
                ALTER TABLE dbo."FgsCredentialProvider" RENAME COLUMN "__EF_CompanyIdUuid" TO "CompanyId";
                ALTER TABLE dbo."FgsCredentialProvider" ALTER COLUMN "CompanyId" SET NOT NULL;
                """);

            migrationBuilder.AddColumn<int>(
                name: "GloPaymentMethodTypeId",
                schema: "dbo",
                table: "FgsSetupPaymentMethod",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BreakLabel",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressLine3",
                schema: "dbo",
                table: "FgsLocation",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressLine4",
                schema: "dbo",
                table: "FgsLocation",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsLocation",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsLocation",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "SecretName",
                schema: "dbo",
                table: "FgsCredentialSecret",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsCredentialSecret",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "EncryptedDek",
                schema: "dbo",
                table: "FgsCredentialSecret",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EncryptedSecretValue",
                schema: "dbo",
                table: "FgsCredentialSecret",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EncryptionKeyId",
                schema: "dbo",
                table: "FgsCredentialSecret",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsCredentialSecret",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsCredentialProviderConfiguration",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsCredentialProviderConfiguration",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsCredentialProvider",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsCredentialAudit",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsCredentialAudit",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddUniqueConstraint(
                name: "AK_FgsTenantCompany_TenantId_CompanyGuid",
                schema: "dbo",
                table: "FgsTenantCompany",
                columns: new[] { "TenantId", "CompanyGuid" });

            migrationBuilder.CreateTable(
                name: "GloCommunicationToken",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TokenCode = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    SourceTableName = table.Column<string>(type: "text", nullable: false),
                    SourceColumnName = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloCommunicationToken", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GloMasterEntityType",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsDocumentAllowed = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloMasterEntityType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GloPaymentMethodType",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloPaymentMethodType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GloTimeCardOption",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloTimeCardOption", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsTenantServiceSetup",
                schema: "dbo",
                columns: table => new
                {
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    GloTimeCardOptionId = table.Column<int>(type: "integer", nullable: false),
                    AccountingIntegrationTypeId = table.Column<int>(type: "integer", nullable: true),
                    UseExternalTaxCalculationProvider = table.Column<bool>(type: "boolean", nullable: false),
                    EnableCallBookingWidget = table.Column<bool>(type: "boolean", nullable: false),
                    EnablePaymentWidget = table.Column<bool>(type: "boolean", nullable: false),
                    EnableCustomerPortal = table.Column<bool>(type: "boolean", nullable: false),
                    EnableRulesManagement = table.Column<bool>(type: "boolean", nullable: false),
                    EnableAutoArrive = table.Column<bool>(type: "boolean", nullable: false),
                    WorkLocationRadiusForAutoArrive = table.Column<int>(type: "integer", nullable: true),
                    OTStartTime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    OTEndTime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    DTStartTime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    DTEndTime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    BillHoursFromDispatchOrArrive = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SourceCodeRequiredOnWorkOrder = table.Column<bool>(type: "boolean", nullable: false),
                    SourceCodeRequiredOnServiceLocation = table.Column<bool>(type: "boolean", nullable: false),
                    BillToStartNumber = table.Column<long>(type: "bigint", nullable: false),
                    POStartNumber = table.Column<long>(type: "bigint", nullable: false),
                    QuoteStartNumber = table.Column<long>(type: "bigint", nullable: false),
                    WorkOrderStartNumber = table.Column<long>(type: "bigint", nullable: false),
                    InvoiceNumberPrefix = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    QuoteNumberPrefix = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    PONumberPrefix = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    WorkOrderNumberPrefix = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    InvoiceBatchNumberFormat = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsTenantServiceSetup", x => new { x.TenantId, x.CompanyId });
                    table.ForeignKey(
                        name: "FK_FgsTenantServiceSetup_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyGuid" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsTenantServiceSetup_GloTimeCardOption_GloTimeCardOptionId",
                        column: x => x.GloTimeCardOptionId,
                        principalSchema: "dbo",
                        principalTable: "GloTimeCardOption",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupZone_TenantId_CompanyId_Code",
                schema: "dbo",
                table: "FgsSetupZone",
                columns: new[] { "TenantId", "CompanyId", "Code" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTitleOfCourtesy_TenantId_CompanyId_Code",
                schema: "dbo",
                table: "FgsSetupTitleOfCourtesy",
                columns: new[] { "TenantId", "CompanyId", "Code" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTimeSlot_TenantId_CompanyId_Code",
                schema: "dbo",
                table: "FgsSetupTimeSlot",
                columns: new[] { "TenantId", "CompanyId", "Code" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTechTrade_TenantId_CompanyId_TradeCode",
                schema: "dbo",
                table: "FgsSetupTechTrade",
                columns: new[] { "TenantId", "CompanyId", "TradeCode" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTechSkillLevel_TenantId_CompanyId_Code",
                schema: "dbo",
                table: "FgsSetupTechSkillLevel",
                columns: new[] { "TenantId", "CompanyId", "Code" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTaxAuthority_TenantId_CompanyId_Code",
                schema: "dbo",
                table: "FgsSetupTaxAuthority",
                columns: new[] { "TenantId", "CompanyId", "Code" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTax_TenantId_CompanyId_TaxCode",
                schema: "dbo",
                table: "FgsSetupTax",
                columns: new[] { "TenantId", "CompanyId", "TaxCode" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupServiceAssetType_TenantId_CompanyId_Code",
                schema: "dbo",
                table: "FgsSetupServiceAssetType",
                columns: new[] { "TenantId", "CompanyId", "Code" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupServiceAssetManufacturer_TenantId_CompanyId_Code",
                schema: "dbo",
                table: "FgsSetupServiceAssetManufacturer",
                columns: new[] { "TenantId", "CompanyId", "Code" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPriceSheetOther_TenantId_CompanyId_CategoryCode",
                schema: "dbo",
                table: "FgsSetupPriceSheetOther",
                columns: new[] { "TenantId", "CompanyId", "CategoryCode" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPriceSheetMaterial_TenantId_CompanyId_Code",
                schema: "dbo",
                table: "FgsSetupPriceSheetMaterial",
                columns: new[] { "TenantId", "CompanyId", "Code" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPriceSheet_TenantId_CompanyId_Code",
                schema: "dbo",
                table: "FgsSetupPriceSheet",
                columns: new[] { "TenantId", "CompanyId", "Code" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPostalCode_TenantId_CompanyId_PostalCode",
                schema: "dbo",
                table: "FgsSetupPostalCode",
                columns: new[] { "TenantId", "CompanyId", "PostalCode" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPaymentTerm_TenantId_CompanyId_Name",
                schema: "dbo",
                table: "FgsSetupPaymentTerm",
                columns: new[] { "TenantId", "CompanyId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPaymentMethod_GloPaymentMethodTypeId",
                schema: "dbo",
                table: "FgsSetupPaymentMethod",
                column: "GloPaymentMethodTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPaymentMethod_TenantId_CompanyId_GloPaymentMethodTy~",
                schema: "dbo",
                table: "FgsSetupPaymentMethod",
                columns: new[] { "TenantId", "CompanyId", "GloPaymentMethodTypeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupGLBreak_TenantId_CompanyId_Code",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                columns: new[] { "TenantId", "CompanyId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupDescription_TenantId_CompanyId_DescriptionTypeCode",
                schema: "dbo",
                table: "FgsSetupDescription",
                columns: new[] { "TenantId", "CompanyId", "DescriptionTypeCode" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupCommunicationTemplate_TenantId_CompanyId_TemplateTy~",
                schema: "dbo",
                table: "FgsSetupCommunicationTemplate",
                columns: new[] { "TenantId", "CompanyId", "TemplateType", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsLocation_TenantId_CompanyId_MasterEntityTypeId_EntityNum~",
                schema: "dbo",
                table: "FgsLocation",
                columns: new[] { "TenantId", "CompanyId", "MasterEntityTypeId", "EntityNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredentialSecret_CredentialProviderId",
                schema: "dbo",
                table: "FgsCredentialSecret",
                column: "CredentialProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredentialSecret_IsActive",
                schema: "dbo",
                table: "FgsCredentialSecret",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredentialSecret_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsCredentialSecret",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredentialSecret_TenantId_CompanyId_CredentialProviderId",
                schema: "dbo",
                table: "FgsCredentialSecret",
                columns: new[] { "TenantId", "CompanyId", "CredentialProviderId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredentialSecret_TenantId_CompanyId_CredentialProviderId~",
                schema: "dbo",
                table: "FgsCredentialSecret",
                columns: new[] { "TenantId", "CompanyId", "CredentialProviderId", "SecretName", "VersionNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredentialProviderConfiguration_CredentialProviderId",
                schema: "dbo",
                table: "FgsCredentialProviderConfiguration",
                column: "CredentialProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredentialProviderConfiguration_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsCredentialProviderConfiguration",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredentialProviderConfiguration_TenantId_CompanyId_Cred~1",
                schema: "dbo",
                table: "FgsCredentialProviderConfiguration",
                columns: new[] { "TenantId", "CompanyId", "CredentialProviderId", "ConfigurationKey", "Environment" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredentialProviderConfiguration_TenantId_CompanyId_Crede~",
                schema: "dbo",
                table: "FgsCredentialProviderConfiguration",
                columns: new[] { "TenantId", "CompanyId", "CredentialProviderId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredentialProvider_TenantId_CompanyId_Code",
                schema: "dbo",
                table: "FgsCredentialProvider",
                columns: new[] { "TenantId", "CompanyId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredentialAudit_CredentialSecretId",
                schema: "dbo",
                table: "FgsCredentialAudit",
                column: "CredentialSecretId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredentialAudit_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsCredentialAudit",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredentialAudit_TenantId_CompanyId_CredentialSecretId",
                schema: "dbo",
                table: "FgsCredentialAudit",
                columns: new[] { "TenantId", "CompanyId", "CredentialSecretId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredentialAudit_TenantId_CompanyId_CredentialSecretId_Ac~",
                schema: "dbo",
                table: "FgsCredentialAudit",
                columns: new[] { "TenantId", "CompanyId", "CredentialSecretId", "ActionType", "NewVersionNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsTenantServiceSetup_GloTimeCardOptionId",
                schema: "dbo",
                table: "FgsTenantServiceSetup",
                column: "GloTimeCardOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_GloCommunicationToken_TokenCode",
                schema: "dbo",
                table: "GloCommunicationToken",
                column: "TokenCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloMasterEntityType_Code",
                schema: "dbo",
                table: "GloMasterEntityType",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloPaymentMethodType_Code",
                schema: "dbo",
                table: "GloPaymentMethodType",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloTimeCardOption_Code",
                schema: "dbo",
                table: "GloTimeCardOption",
                column: "Code",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsCredentialAudit_FgsCredentialSecret_CredentialSecretId",
                schema: "dbo",
                table: "FgsCredentialAudit",
                column: "CredentialSecretId",
                principalSchema: "dbo",
                principalTable: "FgsCredentialSecret",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsCredentialAudit_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsCredentialAudit",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "dbo",
                principalTable: "FgsTenantCompany",
                principalColumns: new[] { "TenantId", "CompanyGuid" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsCredentialProvider_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsCredentialProvider",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "dbo",
                principalTable: "FgsTenantCompany",
                principalColumns: new[] { "TenantId", "CompanyGuid" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsCredentialProviderConfiguration_FgsCredentialProvider_Cr~",
                schema: "dbo",
                table: "FgsCredentialProviderConfiguration",
                column: "CredentialProviderId",
                principalSchema: "dbo",
                principalTable: "FgsCredentialProvider",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsCredentialProviderConfiguration_FgsTenantCompany_TenantI~",
                schema: "dbo",
                table: "FgsCredentialProviderConfiguration",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "dbo",
                principalTable: "FgsTenantCompany",
                principalColumns: new[] { "TenantId", "CompanyGuid" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsCredentialSecret_FgsCredentialProvider_CredentialProvide~",
                schema: "dbo",
                table: "FgsCredentialSecret",
                column: "CredentialProviderId",
                principalSchema: "dbo",
                principalTable: "FgsCredentialProvider",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsCredentialSecret_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsCredentialSecret",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "dbo",
                principalTable: "FgsTenantCompany",
                principalColumns: new[] { "TenantId", "CompanyGuid" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsLocation_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsLocation",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "dbo",
                principalTable: "FgsTenantCompany",
                principalColumns: new[] { "TenantId", "CompanyGuid" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsLocation_GloMasterEntityType_MasterEntityTypeId",
                schema: "dbo",
                table: "FgsLocation",
                column: "MasterEntityTypeId",
                principalSchema: "dbo",
                principalTable: "GloMasterEntityType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupCommunicationTemplate_FgsTenantCompany_TenantId_Com~",
                schema: "dbo",
                table: "FgsSetupCommunicationTemplate",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "dbo",
                principalTable: "FgsTenantCompany",
                principalColumns: new[] { "TenantId", "CompanyGuid" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupDescription_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupDescription",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "dbo",
                principalTable: "FgsTenantCompany",
                principalColumns: new[] { "TenantId", "CompanyGuid" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupGLBreak_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "dbo",
                principalTable: "FgsTenantCompany",
                principalColumns: new[] { "TenantId", "CompanyGuid" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupPaymentMethod_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupPaymentMethod",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "dbo",
                principalTable: "FgsTenantCompany",
                principalColumns: new[] { "TenantId", "CompanyGuid" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupPaymentMethod_GloPaymentMethodType_GloPaymentMethod~",
                schema: "dbo",
                table: "FgsSetupPaymentMethod",
                column: "GloPaymentMethodTypeId",
                principalSchema: "dbo",
                principalTable: "GloPaymentMethodType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupPaymentTerm_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupPaymentTerm",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "dbo",
                principalTable: "FgsTenantCompany",
                principalColumns: new[] { "TenantId", "CompanyGuid" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupPostalCode_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupPostalCode",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "dbo",
                principalTable: "FgsTenantCompany",
                principalColumns: new[] { "TenantId", "CompanyGuid" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupPriceSheet_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupPriceSheet",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "dbo",
                principalTable: "FgsTenantCompany",
                principalColumns: new[] { "TenantId", "CompanyGuid" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupPriceSheetLabor_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupPriceSheetLabor",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "dbo",
                principalTable: "FgsTenantCompany",
                principalColumns: new[] { "TenantId", "CompanyGuid" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupPriceSheetLaborTier_FgsTenantCompany_TenantId_Compa~",
                schema: "dbo",
                table: "FgsSetupPriceSheetLaborTier",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "dbo",
                principalTable: "FgsTenantCompany",
                principalColumns: new[] { "TenantId", "CompanyGuid" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupPriceSheetMaterial_FgsTenantCompany_TenantId_Compan~",
                schema: "dbo",
                table: "FgsSetupPriceSheetMaterial",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "dbo",
                principalTable: "FgsTenantCompany",
                principalColumns: new[] { "TenantId", "CompanyGuid" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupPriceSheetMaterialRange_FgsTenantCompany_TenantId_C~",
                schema: "dbo",
                table: "FgsSetupPriceSheetMaterialRange",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "dbo",
                principalTable: "FgsTenantCompany",
                principalColumns: new[] { "TenantId", "CompanyGuid" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupPriceSheetOther_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupPriceSheetOther",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "dbo",
                principalTable: "FgsTenantCompany",
                principalColumns: new[] { "TenantId", "CompanyGuid" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupServiceAssetManufacturer_FgsTenantCompany_TenantId_~",
                schema: "dbo",
                table: "FgsSetupServiceAssetManufacturer",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "dbo",
                principalTable: "FgsTenantCompany",
                principalColumns: new[] { "TenantId", "CompanyGuid" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupServiceAssetMedia_FgsTenantCompany_TenantId_Company~",
                schema: "dbo",
                table: "FgsSetupServiceAssetMedia",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "dbo",
                principalTable: "FgsTenantCompany",
                principalColumns: new[] { "TenantId", "CompanyGuid" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupServiceAssetModelSerialDescription_FgsTenantCompany~",
                schema: "dbo",
                table: "FgsSetupServiceAssetModelSerialDescription",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "dbo",
                principalTable: "FgsTenantCompany",
                principalColumns: new[] { "TenantId", "CompanyGuid" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupServiceAssetType_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupServiceAssetType",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "dbo",
                principalTable: "FgsTenantCompany",
                principalColumns: new[] { "TenantId", "CompanyGuid" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupTax_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupTax",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "dbo",
                principalTable: "FgsTenantCompany",
                principalColumns: new[] { "TenantId", "CompanyGuid" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupTaxAuthority_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupTaxAuthority",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "dbo",
                principalTable: "FgsTenantCompany",
                principalColumns: new[] { "TenantId", "CompanyGuid" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupTaxDetail_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupTaxDetail",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "dbo",
                principalTable: "FgsTenantCompany",
                principalColumns: new[] { "TenantId", "CompanyGuid" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupTechSkillLevel_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupTechSkillLevel",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "dbo",
                principalTable: "FgsTenantCompany",
                principalColumns: new[] { "TenantId", "CompanyGuid" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupTechTrade_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupTechTrade",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "dbo",
                principalTable: "FgsTenantCompany",
                principalColumns: new[] { "TenantId", "CompanyGuid" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupTimeSlot_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupTimeSlot",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "dbo",
                principalTable: "FgsTenantCompany",
                principalColumns: new[] { "TenantId", "CompanyGuid" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupTitleOfCourtesy_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupTitleOfCourtesy",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "dbo",
                principalTable: "FgsTenantCompany",
                principalColumns: new[] { "TenantId", "CompanyGuid" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupZone_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupZone",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "dbo",
                principalTable: "FgsTenantCompany",
                principalColumns: new[] { "TenantId", "CompanyGuid" },
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FgsCredentialAudit_FgsCredentialSecret_CredentialSecretId",
                schema: "dbo",
                table: "FgsCredentialAudit");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsCredentialAudit_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsCredentialAudit");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsCredentialProvider_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsCredentialProvider");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsCredentialProviderConfiguration_FgsCredentialProvider_Cr~",
                schema: "dbo",
                table: "FgsCredentialProviderConfiguration");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsCredentialProviderConfiguration_FgsTenantCompany_TenantI~",
                schema: "dbo",
                table: "FgsCredentialProviderConfiguration");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsCredentialSecret_FgsCredentialProvider_CredentialProvide~",
                schema: "dbo",
                table: "FgsCredentialSecret");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsCredentialSecret_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsCredentialSecret");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsLocation_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsLocation");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsLocation_GloMasterEntityType_MasterEntityTypeId",
                schema: "dbo",
                table: "FgsLocation");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupCommunicationTemplate_FgsTenantCompany_TenantId_Com~",
                schema: "dbo",
                table: "FgsSetupCommunicationTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupDescription_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupDescription");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupGLBreak_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupGLBreak");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupPaymentMethod_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupPaymentMethod");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupPaymentMethod_GloPaymentMethodType_GloPaymentMethod~",
                schema: "dbo",
                table: "FgsSetupPaymentMethod");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupPaymentTerm_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupPaymentTerm");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupPostalCode_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupPostalCode");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupPriceSheet_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupPriceSheet");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupPriceSheetLabor_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupPriceSheetLabor");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupPriceSheetLaborTier_FgsTenantCompany_TenantId_Compa~",
                schema: "dbo",
                table: "FgsSetupPriceSheetLaborTier");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupPriceSheetMaterial_FgsTenantCompany_TenantId_Compan~",
                schema: "dbo",
                table: "FgsSetupPriceSheetMaterial");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupPriceSheetMaterialRange_FgsTenantCompany_TenantId_C~",
                schema: "dbo",
                table: "FgsSetupPriceSheetMaterialRange");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupPriceSheetOther_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupPriceSheetOther");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupServiceAssetManufacturer_FgsTenantCompany_TenantId_~",
                schema: "dbo",
                table: "FgsSetupServiceAssetManufacturer");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupServiceAssetMedia_FgsTenantCompany_TenantId_Company~",
                schema: "dbo",
                table: "FgsSetupServiceAssetMedia");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupServiceAssetModelSerialDescription_FgsTenantCompany~",
                schema: "dbo",
                table: "FgsSetupServiceAssetModelSerialDescription");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupServiceAssetType_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupServiceAssetType");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupTax_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupTax");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupTaxAuthority_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupTaxAuthority");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupTaxDetail_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupTaxDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupTechSkillLevel_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupTechSkillLevel");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupTechTrade_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupTechTrade");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupTimeSlot_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupTimeSlot");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupTitleOfCourtesy_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupTitleOfCourtesy");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupZone_FgsTenantCompany_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupZone");

            migrationBuilder.DropTable(
                name: "FgsTenantServiceSetup",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloCommunicationToken",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloMasterEntityType",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloPaymentMethodType",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloTimeCardOption",
                schema: "dbo");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_FgsTenantCompany_TenantId_CompanyGuid",
                schema: "dbo",
                table: "FgsTenantCompany");

            migrationBuilder.DropIndex(
                name: "IX_FgsSetupZone_TenantId_CompanyId_Code",
                schema: "dbo",
                table: "FgsSetupZone");

            migrationBuilder.DropIndex(
                name: "IX_FgsSetupTitleOfCourtesy_TenantId_CompanyId_Code",
                schema: "dbo",
                table: "FgsSetupTitleOfCourtesy");

            migrationBuilder.DropIndex(
                name: "IX_FgsSetupTimeSlot_TenantId_CompanyId_Code",
                schema: "dbo",
                table: "FgsSetupTimeSlot");

            migrationBuilder.DropIndex(
                name: "IX_FgsSetupTechTrade_TenantId_CompanyId_TradeCode",
                schema: "dbo",
                table: "FgsSetupTechTrade");

            migrationBuilder.DropIndex(
                name: "IX_FgsSetupTechSkillLevel_TenantId_CompanyId_Code",
                schema: "dbo",
                table: "FgsSetupTechSkillLevel");

            migrationBuilder.DropIndex(
                name: "IX_FgsSetupTaxAuthority_TenantId_CompanyId_Code",
                schema: "dbo",
                table: "FgsSetupTaxAuthority");

            migrationBuilder.DropIndex(
                name: "IX_FgsSetupTax_TenantId_CompanyId_TaxCode",
                schema: "dbo",
                table: "FgsSetupTax");

            migrationBuilder.DropIndex(
                name: "IX_FgsSetupServiceAssetType_TenantId_CompanyId_Code",
                schema: "dbo",
                table: "FgsSetupServiceAssetType");

            migrationBuilder.DropIndex(
                name: "IX_FgsSetupServiceAssetManufacturer_TenantId_CompanyId_Code",
                schema: "dbo",
                table: "FgsSetupServiceAssetManufacturer");

            migrationBuilder.DropIndex(
                name: "IX_FgsSetupPriceSheetOther_TenantId_CompanyId_CategoryCode",
                schema: "dbo",
                table: "FgsSetupPriceSheetOther");

            migrationBuilder.DropIndex(
                name: "IX_FgsSetupPriceSheetMaterial_TenantId_CompanyId_Code",
                schema: "dbo",
                table: "FgsSetupPriceSheetMaterial");

            migrationBuilder.DropIndex(
                name: "IX_FgsSetupPriceSheet_TenantId_CompanyId_Code",
                schema: "dbo",
                table: "FgsSetupPriceSheet");

            migrationBuilder.DropIndex(
                name: "IX_FgsSetupPostalCode_TenantId_CompanyId_PostalCode",
                schema: "dbo",
                table: "FgsSetupPostalCode");

            migrationBuilder.DropIndex(
                name: "IX_FgsSetupPaymentTerm_TenantId_CompanyId_Name",
                schema: "dbo",
                table: "FgsSetupPaymentTerm");

            migrationBuilder.DropIndex(
                name: "IX_FgsSetupPaymentMethod_GloPaymentMethodTypeId",
                schema: "dbo",
                table: "FgsSetupPaymentMethod");

            migrationBuilder.DropIndex(
                name: "IX_FgsSetupPaymentMethod_TenantId_CompanyId_GloPaymentMethodTy~",
                schema: "dbo",
                table: "FgsSetupPaymentMethod");

            migrationBuilder.DropIndex(
                name: "IX_FgsSetupGLBreak_TenantId_CompanyId_Code",
                schema: "dbo",
                table: "FgsSetupGLBreak");

            migrationBuilder.DropIndex(
                name: "IX_FgsSetupDescription_TenantId_CompanyId_DescriptionTypeCode",
                schema: "dbo",
                table: "FgsSetupDescription");

            migrationBuilder.DropIndex(
                name: "IX_FgsSetupCommunicationTemplate_TenantId_CompanyId_TemplateTy~",
                schema: "dbo",
                table: "FgsSetupCommunicationTemplate");

            migrationBuilder.DropIndex(
                name: "IX_FgsLocation_TenantId_CompanyId_MasterEntityTypeId_EntityNum~",
                schema: "dbo",
                table: "FgsLocation");

            migrationBuilder.DropIndex(
                name: "IX_FgsCredentialSecret_CredentialProviderId",
                schema: "dbo",
                table: "FgsCredentialSecret");

            migrationBuilder.DropIndex(
                name: "IX_FgsCredentialSecret_IsActive",
                schema: "dbo",
                table: "FgsCredentialSecret");

            migrationBuilder.DropIndex(
                name: "IX_FgsCredentialSecret_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsCredentialSecret");

            migrationBuilder.DropIndex(
                name: "IX_FgsCredentialSecret_TenantId_CompanyId_CredentialProviderId",
                schema: "dbo",
                table: "FgsCredentialSecret");

            migrationBuilder.DropIndex(
                name: "IX_FgsCredentialSecret_TenantId_CompanyId_CredentialProviderId~",
                schema: "dbo",
                table: "FgsCredentialSecret");

            migrationBuilder.DropIndex(
                name: "IX_FgsCredentialProviderConfiguration_CredentialProviderId",
                schema: "dbo",
                table: "FgsCredentialProviderConfiguration");

            migrationBuilder.DropIndex(
                name: "IX_FgsCredentialProviderConfiguration_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsCredentialProviderConfiguration");

            migrationBuilder.DropIndex(
                name: "IX_FgsCredentialProviderConfiguration_TenantId_CompanyId_Cred~1",
                schema: "dbo",
                table: "FgsCredentialProviderConfiguration");

            migrationBuilder.DropIndex(
                name: "IX_FgsCredentialProviderConfiguration_TenantId_CompanyId_Crede~",
                schema: "dbo",
                table: "FgsCredentialProviderConfiguration");

            migrationBuilder.DropIndex(
                name: "IX_FgsCredentialProvider_TenantId_CompanyId_Code",
                schema: "dbo",
                table: "FgsCredentialProvider");

            migrationBuilder.DropIndex(
                name: "IX_FgsCredentialAudit_CredentialSecretId",
                schema: "dbo",
                table: "FgsCredentialAudit");

            migrationBuilder.DropIndex(
                name: "IX_FgsCredentialAudit_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsCredentialAudit");

            migrationBuilder.DropIndex(
                name: "IX_FgsCredentialAudit_TenantId_CompanyId_CredentialSecretId",
                schema: "dbo",
                table: "FgsCredentialAudit");

            migrationBuilder.DropIndex(
                name: "IX_FgsCredentialAudit_TenantId_CompanyId_CredentialSecretId_Ac~",
                schema: "dbo",
                table: "FgsCredentialAudit");

            migrationBuilder.DropColumn(
                name: "GloPaymentMethodTypeId",
                schema: "dbo",
                table: "FgsSetupPaymentMethod");

            migrationBuilder.DropColumn(
                name: "BreakLabel",
                schema: "dbo",
                table: "FgsSetupGLBreak");

            migrationBuilder.DropColumn(
                name: "AddressLine3",
                schema: "dbo",
                table: "FgsLocation");

            migrationBuilder.DropColumn(
                name: "AddressLine4",
                schema: "dbo",
                table: "FgsLocation");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsLocation");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "dbo",
                table: "FgsLocation");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsCredentialSecret");

            migrationBuilder.DropColumn(
                name: "EncryptedDek",
                schema: "dbo",
                table: "FgsCredentialSecret");

            migrationBuilder.DropColumn(
                name: "EncryptedSecretValue",
                schema: "dbo",
                table: "FgsCredentialSecret");

            migrationBuilder.DropColumn(
                name: "EncryptionKeyId",
                schema: "dbo",
                table: "FgsCredentialSecret");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "dbo",
                table: "FgsCredentialSecret");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsCredentialProviderConfiguration");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "dbo",
                table: "FgsCredentialProviderConfiguration");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsCredentialAudit");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "dbo",
                table: "FgsCredentialAudit");

            migrationBuilder.RenameColumn(
                name: "LogoUrl",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "MasterEntityTypeId",
                schema: "dbo",
                table: "FgsLocation",
                newName: "EntityTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_FgsLocation_MasterEntityTypeId",
                schema: "dbo",
                table: "FgsLocation",
                newName: "IX_FgsLocation_EntityTypeId");

            migrationBuilder.RenameColumn(
                name: "LastRotatedOn",
                schema: "dbo",
                table: "FgsCredentialSecret",
                newName: "RotatedOn");

            migrationBuilder.RenameColumn(
                name: "IsRevoked",
                schema: "dbo",
                table: "FgsCredentialSecret",
                newName: "RotationEnabled");

            migrationBuilder.RenameColumn(
                name: "ExpiresOn",
                schema: "dbo",
                table: "FgsCredentialSecret",
                newName: "LastValidatedOn");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupZone",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupTitleOfCourtesy",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupTimeSlot",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupTechTrade",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupTechSkillLevel",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupTaxDetail",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupTaxAuthority",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupTax",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupServiceAssetType",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupServiceAssetModelSerialDescription",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupServiceAssetMedia",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupServiceAssetManufacturer",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupPriceSheetOther",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupPriceSheetMaterialRange",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupPriceSheetMaterial",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupPriceSheetLaborTier",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupPriceSheetLabor",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupPriceSheet",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupPostalCode",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupPaymentTerm",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupPaymentMethod",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethodType",
                schema: "dbo",
                table: "FgsSetupPaymentMethod",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "LogoLocationId",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupDescription",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupCommunicationTemplate",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<int>(
                name: "GloMasterEntityTypeId",
                schema: "dbo",
                table: "FgsSetupCommunicationTemplate",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SecretName",
                schema: "dbo",
                table: "FgsCredentialSecret",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<string>(
                name: "KmsKeyArn",
                schema: "dbo",
                table: "FgsCredentialSecret",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegionName",
                schema: "dbo",
                table: "FgsCredentialSecret",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                schema: "dbo",
                table: "FgsCredentialSecret",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecretArn",
                schema: "dbo",
                table: "FgsCredentialSecret",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VaultProvider",
                schema: "dbo",
                table: "FgsCredentialSecret",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsCredentialProvider",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsCredentialProvider",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateTable(
                name: "FgsSetupCommunicationToken",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    SampleValue = table.Column<string>(type: "text", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    TokenCode = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupCommunicationToken", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsTenantCompanyConfiguration",
                schema: "dbo",
                columns: table => new
                {
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    AccountingIntegrationTypeId = table.Column<int>(type: "integer", nullable: true),
                    BillHoursFromDispatchOrArrive = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    BillToStartNumber = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    DTEndTime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    DTStartTime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    EnableAutoArrive = table.Column<bool>(type: "boolean", nullable: false),
                    EnableCallBookingWidget = table.Column<bool>(type: "boolean", nullable: false),
                    EnableCustomerPortal = table.Column<bool>(type: "boolean", nullable: false),
                    EnablePaymentWidget = table.Column<bool>(type: "boolean", nullable: false),
                    EnableRulesManagement = table.Column<bool>(type: "boolean", nullable: false),
                    InvoiceBatchNumberFormat = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    InvoiceNumberPrefix = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    OTEndTime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    OTStartTime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    PONumberPrefix = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    POStartNumber = table.Column<long>(type: "bigint", nullable: false),
                    QuoteNumberPrefix = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    QuoteStartNumber = table.Column<long>(type: "bigint", nullable: false),
                    SourceCodeRequiredOnServiceLocation = table.Column<bool>(type: "boolean", nullable: false),
                    SourceCodeRequiredOnWorkOrder = table.Column<bool>(type: "boolean", nullable: false),
                    TimeCardOptionId = table.Column<int>(type: "integer", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    WorkLocationRadiusForAutoArrive = table.Column<int>(type: "integer", nullable: true),
                    WorkOrderNumberPrefix = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    WorkOrderStartNumber = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsTenantCompanyConfiguration", x => new { x.TenantId, x.CompanyId });
                });

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredentialProvider_TenantId_Code",
                schema: "dbo",
                table: "FgsCredentialProvider",
                columns: new[] { "TenantId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupCommunicationToken_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupCommunicationToken",
                columns: new[] { "TenantId", "CompanyId" });
        }
    }
}
