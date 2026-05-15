using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.User.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class GloGeoAndBillingCatalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GloStateProvince_GloCountry_GloCountryId",
                schema: "dbo",
                table: "GloStateProvince");

            migrationBuilder.DropIndex(
                name: "IX_GloStateProvince_GloCountryId",
                schema: "dbo",
                table: "GloStateProvince");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GloLanguage",
                schema: "dbo",
                table: "GloLanguage");

            migrationBuilder.DropIndex(
                name: "IX_GloLanguage_LanguageCode",
                schema: "dbo",
                table: "GloLanguage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GloCountry",
                schema: "dbo",
                table: "GloCountry");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "dbo",
                table: "GloStateProvince");

            migrationBuilder.DropColumn(
                name: "GloCountryId",
                schema: "dbo",
                table: "GloStateProvince");

            migrationBuilder.DropColumn(
                name: "RegionCode",
                schema: "dbo",
                table: "GloStateProvince");

            migrationBuilder.DropColumn(
                name: "RegionName",
                schema: "dbo",
                table: "GloStateProvince");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "dbo",
                table: "GloLanguage");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "dbo",
                table: "GloLanguage");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "dbo",
                table: "GloLanguage");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                schema: "dbo",
                table: "GloLanguage");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "dbo",
                table: "GloCountry");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "dbo",
                table: "GloCountry");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "dbo",
                table: "GloStateProvince",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "dbo",
                table: "GloStateProvince",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                schema: "dbo",
                table: "GloStateProvince",
                type: "character varying(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StateProvinceCode",
                schema: "dbo",
                table: "GloStateProvince",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StateProvinceName",
                schema: "dbo",
                table: "GloStateProvince",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "LanguageCode",
                schema: "dbo",
                table: "GloLanguage",
                type: "character varying(5)",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(25)",
                oldMaxLength: 25);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "dbo",
                table: "GloLanguage",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<string>(
                name: "LanguageName",
                schema: "dbo",
                table: "GloLanguage",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "dbo",
                table: "GloCountry",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "CountryName",
                schema: "dbo",
                table: "GloCountry",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "CountryCode",
                schema: "dbo",
                table: "GloCountry",
                type: "character varying(2)",
                maxLength: 2,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AddColumn<string>(
                name: "CurrencyCode",
                schema: "dbo",
                table: "GloCountry",
                type: "character varying(3)",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GloLanguage",
                schema: "dbo",
                table: "GloLanguage",
                column: "LanguageCode");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GloCountry",
                schema: "dbo",
                table: "GloCountry",
                column: "CountryCode");

            migrationBuilder.CreateTable(
                name: "GloBillingCategory",
                schema: "dbo",
                columns: table => new
                {
                    BillingCategoryType = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    BillingCategoryName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloBillingCategory", x => x.BillingCategoryType);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "GloBillingCategory",
                columns: new[] { "BillingCategoryType", "BillingCategoryName" },
                values: new object[,]
                {
                    { "EQ", "Equipment" },
                    { "MT", "Material" },
                    { "LB", "Labor" },
                    { "SB", "Sub Contractor" },
                    { "SF", "Service Fee" },
                    { "SH", "Shipping" },
                    { "TX", "Tax" },
                    { "DS", "Discount" },
                    { "OT", "Other" }
                });

            migrationBuilder.CreateIndex(
                name: "UQ_GloStateProvince",
                schema: "dbo",
                table: "GloStateProvince",
                columns: new[] { "CountryCode", "StateProvinceCode" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GloStateProvince_Country",
                schema: "dbo",
                table: "GloStateProvince",
                column: "CountryCode",
                principalSchema: "dbo",
                principalTable: "GloCountry",
                principalColumn: "CountryCode",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GloStateProvince_Country",
                schema: "dbo",
                table: "GloStateProvince");

            migrationBuilder.DropTable(
                name: "GloBillingCategory",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "UQ_GloStateProvince",
                schema: "dbo",
                table: "GloStateProvince");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GloLanguage",
                schema: "dbo",
                table: "GloLanguage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GloCountry",
                schema: "dbo",
                table: "GloCountry");

            migrationBuilder.DropColumn(
                name: "CountryCode",
                schema: "dbo",
                table: "GloStateProvince");

            migrationBuilder.DropColumn(
                name: "StateProvinceCode",
                schema: "dbo",
                table: "GloStateProvince");

            migrationBuilder.DropColumn(
                name: "StateProvinceName",
                schema: "dbo",
                table: "GloStateProvince");

            migrationBuilder.DropColumn(
                name: "LanguageName",
                schema: "dbo",
                table: "GloLanguage");

            migrationBuilder.DropColumn(
                name: "CurrencyCode",
                schema: "dbo",
                table: "GloCountry");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "dbo",
                table: "GloStateProvince",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "GloStateProvince",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedOn",
                schema: "dbo",
                table: "GloStateProvince",
                type: "timestamptz",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<long>(
                name: "GloCountryId",
                schema: "dbo",
                table: "GloStateProvince",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "RegionCode",
                schema: "dbo",
                table: "GloStateProvince",
                type: "character varying(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RegionName",
                schema: "dbo",
                table: "GloStateProvince",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "dbo",
                table: "GloLanguage",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "LanguageCode",
                schema: "dbo",
                table: "GloLanguage",
                type: "character varying(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(5)",
                oldMaxLength: 5);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "dbo",
                table: "GloLanguage",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedOn",
                schema: "dbo",
                table: "GloLanguage",
                type: "timestamptz",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "dbo",
                table: "GloLanguage",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedOn",
                schema: "dbo",
                table: "GloLanguage",
                type: "timestamptz",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "dbo",
                table: "GloCountry",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "CountryName",
                schema: "dbo",
                table: "GloCountry",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "CountryCode",
                schema: "dbo",
                table: "GloCountry",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2)",
                oldMaxLength: 2);

            migrationBuilder.AddColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "GloCountry",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedOn",
                schema: "dbo",
                table: "GloCountry",
                type: "timestamptz",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddPrimaryKey(
                name: "PK_GloLanguage",
                schema: "dbo",
                table: "GloLanguage",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GloCountry",
                schema: "dbo",
                table: "GloCountry",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_GloStateProvince_GloCountryId",
                schema: "dbo",
                table: "GloStateProvince",
                column: "GloCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_GloLanguage_LanguageCode",
                schema: "dbo",
                table: "GloLanguage",
                column: "LanguageCode",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GloStateProvince_GloCountry_GloCountryId",
                schema: "dbo",
                table: "GloStateProvince",
                column: "GloCountryId",
                principalSchema: "dbo",
                principalTable: "GloCountry",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
