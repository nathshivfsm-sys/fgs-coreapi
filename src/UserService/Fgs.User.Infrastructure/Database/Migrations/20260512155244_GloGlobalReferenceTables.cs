using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.User.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class GloGlobalReferenceTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GloAccountingIntegrationType",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloAccountingIntegrationType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GloBusinessType",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloBusinessType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GloCountry",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CountryCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    CountryName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloCountry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GloCredentialCategory",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloCredentialCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GloCredentialProviderType",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloCredentialProviderType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GloLanguage",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LanguageCode = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloLanguage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GloLocationType",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloLocationType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GloResolutionType",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ResolutionTypeCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ResolutionTypeName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloResolutionType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GloStateProvince",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GloCountryId = table.Column<long>(type: "bigint", nullable: false),
                    RegionCode = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    RegionName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloStateProvince", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GloStateProvince_GloCountry_GloCountryId",
                        column: x => x.GloCountryId,
                        principalSchema: "dbo",
                        principalTable: "GloCountry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GloAccountingIntegrationType_Code",
                schema: "dbo",
                table: "GloAccountingIntegrationType",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloBusinessType_Code",
                schema: "dbo",
                table: "GloBusinessType",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloCredentialCategory_Code",
                schema: "dbo",
                table: "GloCredentialCategory",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloCredentialProviderType_Code",
                schema: "dbo",
                table: "GloCredentialProviderType",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloLanguage_LanguageCode",
                schema: "dbo",
                table: "GloLanguage",
                column: "LanguageCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloLocationType_Code",
                schema: "dbo",
                table: "GloLocationType",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloResolutionType_ResolutionTypeCode",
                schema: "dbo",
                table: "GloResolutionType",
                column: "ResolutionTypeCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloStateProvince_GloCountryId",
                schema: "dbo",
                table: "GloStateProvince",
                column: "GloCountryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GloAccountingIntegrationType",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloBusinessType",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloCredentialCategory",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloCredentialProviderType",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloLanguage",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloLocationType",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloResolutionType",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloStateProvince",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloCountry",
                schema: "dbo");
        }
    }
}
