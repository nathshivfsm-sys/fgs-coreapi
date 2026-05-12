using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.User.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class FgsTableColumnOrderTenantCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsTenantServiceSetup",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsTenantServiceSetup",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsTenantCompany",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyGuid",
                schema: "dbo",
                table: "FgsTenantCompany",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsTenantCompany",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .Annotation("Relational:ColumnOrder", 0)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "dbo",
                table: "FgsTenant",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupZone",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupZone",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupZone",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .Annotation("Relational:ColumnOrder", 0)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupTitleOfCourtesy",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupTitleOfCourtesy",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupTitleOfCourtesy",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .Annotation("Relational:ColumnOrder", 0)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupTimeSlot",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupTimeSlot",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupTimeSlot",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .Annotation("Relational:ColumnOrder", 0)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupTechTrade",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupTechTrade",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupTechTrade",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .Annotation("Relational:ColumnOrder", 0)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupTechSkillLevel",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupTechSkillLevel",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupTechSkillLevel",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .Annotation("Relational:ColumnOrder", 0)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupTaxDetail",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupTaxDetail",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupTaxDetail",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .Annotation("Relational:ColumnOrder", 0)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupTaxAuthority",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupTaxAuthority",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupTaxAuthority",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .Annotation("Relational:ColumnOrder", 0)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupTax",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupTax",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupTax",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .Annotation("Relational:ColumnOrder", 0)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupServiceAssetType",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupServiceAssetType",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupServiceAssetType",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .Annotation("Relational:ColumnOrder", 0)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupServiceAssetModelSerialDescription",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupServiceAssetModelSerialDescription",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupServiceAssetModelSerialDescription",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .Annotation("Relational:ColumnOrder", 0)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupServiceAssetMedia",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupServiceAssetMedia",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupServiceAssetMedia",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .Annotation("Relational:ColumnOrder", 0)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupServiceAssetManufacturer",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupServiceAssetManufacturer",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupServiceAssetManufacturer",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .Annotation("Relational:ColumnOrder", 0)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupPriceSheetOther",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupPriceSheetOther",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupPriceSheetOther",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .Annotation("Relational:ColumnOrder", 0)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupPriceSheetMaterialRange",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupPriceSheetMaterialRange",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupPriceSheetMaterialRange",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .Annotation("Relational:ColumnOrder", 0)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupPriceSheetMaterial",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupPriceSheetMaterial",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupPriceSheetMaterial",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .Annotation("Relational:ColumnOrder", 0)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupPriceSheetLaborTier",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupPriceSheetLaborTier",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupPriceSheetLaborTier",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .Annotation("Relational:ColumnOrder", 0)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupPriceSheetLabor",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupPriceSheetLabor",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupPriceSheetLabor",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .Annotation("Relational:ColumnOrder", 0)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupPriceSheet",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupPriceSheet",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupPriceSheet",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .Annotation("Relational:ColumnOrder", 0)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupPostalCode",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupPostalCode",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupPostalCode",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .Annotation("Relational:ColumnOrder", 0)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupPaymentTerm",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupPaymentTerm",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupPaymentTerm",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .Annotation("Relational:ColumnOrder", 0)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupPaymentMethod",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupPaymentMethod",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupPaymentMethod",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .Annotation("Relational:ColumnOrder", 0)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .Annotation("Relational:ColumnOrder", 0)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupDescription",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupDescription",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupDescription",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .Annotation("Relational:ColumnOrder", 0)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupCommunicationTemplate",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupCommunicationTemplate",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupCommunicationTemplate",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .Annotation("Relational:ColumnOrder", 0)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsLocation",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsLocation",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "dbo",
                table: "FgsLocation",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsCredentialSecret",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsCredentialSecret",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "dbo",
                table: "FgsCredentialSecret",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsCredentialProviderConfiguration",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsCredentialProviderConfiguration",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "dbo",
                table: "FgsCredentialProviderConfiguration",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsCredentialProvider",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsCredentialProvider",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "dbo",
                table: "FgsCredentialProvider",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsCredentialAudit",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsCredentialAudit",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "dbo",
                table: "FgsCredentialAudit",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsTenantServiceSetup",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsTenantServiceSetup",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsTenantCompany",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyGuid",
                schema: "dbo",
                table: "FgsTenantCompany",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsTenantCompany",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "dbo",
                table: "FgsTenant",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupZone",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupZone",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupZone",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupTitleOfCourtesy",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupTitleOfCourtesy",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupTitleOfCourtesy",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupTimeSlot",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupTimeSlot",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupTimeSlot",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupTechTrade",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupTechTrade",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupTechTrade",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupTechSkillLevel",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupTechSkillLevel",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupTechSkillLevel",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupTaxDetail",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupTaxDetail",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupTaxDetail",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupTaxAuthority",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupTaxAuthority",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupTaxAuthority",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupTax",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupTax",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupTax",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupServiceAssetType",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupServiceAssetType",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupServiceAssetType",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupServiceAssetModelSerialDescription",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupServiceAssetModelSerialDescription",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupServiceAssetModelSerialDescription",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupServiceAssetMedia",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupServiceAssetMedia",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupServiceAssetMedia",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupServiceAssetManufacturer",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupServiceAssetManufacturer",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupServiceAssetManufacturer",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupPriceSheetOther",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupPriceSheetOther",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupPriceSheetOther",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupPriceSheetMaterialRange",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupPriceSheetMaterialRange",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupPriceSheetMaterialRange",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupPriceSheetMaterial",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupPriceSheetMaterial",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupPriceSheetMaterial",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupPriceSheetLaborTier",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupPriceSheetLaborTier",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupPriceSheetLaborTier",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupPriceSheetLabor",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupPriceSheetLabor",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupPriceSheetLabor",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupPriceSheet",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupPriceSheet",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupPriceSheet",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupPostalCode",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupPostalCode",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupPostalCode",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupPaymentTerm",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupPaymentTerm",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupPaymentTerm",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupPaymentMethod",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupPaymentMethod",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupPaymentMethod",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupDescription",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupDescription",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupDescription",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupCommunicationTemplate",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupCommunicationTemplate",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupCommunicationTemplate",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsLocation",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsLocation",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "dbo",
                table: "FgsLocation",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsCredentialSecret",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsCredentialSecret",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "dbo",
                table: "FgsCredentialSecret",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsCredentialProviderConfiguration",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsCredentialProviderConfiguration",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "dbo",
                table: "FgsCredentialProviderConfiguration",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsCredentialProvider",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsCredentialProvider",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "dbo",
                table: "FgsCredentialProvider",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsCredentialAudit",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsCredentialAudit",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "dbo",
                table: "FgsCredentialAudit",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 0);
        }
    }
}
