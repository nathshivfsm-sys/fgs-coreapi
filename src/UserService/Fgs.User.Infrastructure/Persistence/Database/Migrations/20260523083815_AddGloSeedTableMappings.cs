using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.User.Infrastructure.Persistence.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddGloSeedTableMappings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GloSeedTableMapping",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SeedCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SourceDatabaseName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    SourceSchemaName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValue: "public"),
                    SourceTableName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    TargetDatabaseName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    TargetSchemaName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValue: "public"),
                    TargetTableName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    SeedOrder = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloSeedTableMapping", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GloSeedTableColumnMapping",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SeedTableMappingId = table.Column<long>(type: "bigint", nullable: false),
                    SourceColumnName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    TargetColumnName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    TransformationType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    StaticValue = table.Column<string>(type: "text", nullable: true),
                    ColumnOrder = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloSeedTableColumnMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GloSeedTableColumnMapping_GloSeedTableMapping",
                        column: x => x.SeedTableMappingId,
                        principalSchema: "dbo",
                        principalTable: "GloSeedTableMapping",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GloSeedTableColumnMapping_SeedTableMappingId",
                schema: "dbo",
                table: "GloSeedTableColumnMapping",
                column: "SeedTableMappingId");

            migrationBuilder.CreateIndex(
                name: "IX_GloSeedTableMapping_SeedOrder",
                schema: "dbo",
                table: "GloSeedTableMapping",
                column: "SeedOrder");

            migrationBuilder.CreateIndex(
                name: "UX_GloSeedTableMapping_SeedCode",
                schema: "dbo",
                table: "GloSeedTableMapping",
                column: "SeedCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GloSeedTableColumnMapping",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloSeedTableMapping",
                schema: "dbo");
        }
    }
}
