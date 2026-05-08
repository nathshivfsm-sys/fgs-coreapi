using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UserService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFsgSetupAccountingLanguageMasterEntityLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FSGSetupAccountingIntegrationType",
                schema: "fgs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FSGSetupAccountingIntegrationType", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "FSGSetupLanguage",
                schema: "fgs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    culture_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FSGSetupLanguage", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "FSGSetupLocationType",
                schema: "fgs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FSGSetupLocationType", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "FSGSetupMasterEntityType",
                schema: "fgs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FSGSetupMasterEntityType", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FSGSetupAccountingIntegrationType_code",
                schema: "fgs",
                table: "FSGSetupAccountingIntegrationType",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FSGSetupLanguage_code",
                schema: "fgs",
                table: "FSGSetupLanguage",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FSGSetupLocationType_code",
                schema: "fgs",
                table: "FSGSetupLocationType",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FSGSetupMasterEntityType_code",
                schema: "fgs",
                table: "FSGSetupMasterEntityType",
                column: "code",
                unique: true);

            migrationBuilder.Sql(
                """
                INSERT INTO fgs."FSGSetupAccountingIntegrationType" (code, name, is_active, created_on)
                VALUES
                    ('NONE', 'No Accounting', TRUE, now()),
                    ('QUICKBOOKSONLINE', 'QuickBooks Online', TRUE, now()),
                    ('SAGEINTACCT', 'Sage Intacct', TRUE, now())
                ON CONFLICT (code) DO NOTHING;

                INSERT INTO fgs."FSGSetupLanguage" (code, name, culture_code, is_default, sort_order, is_active, created_on)
                VALUES
                    ('EN', 'English', 'en-US', TRUE, 1, TRUE, now()),
                    ('ES', 'Spanish', 'es-US', FALSE, 2, TRUE, now()),
                    ('FR', 'French', 'fr-FR', FALSE, 3, TRUE, now())
                ON CONFLICT (code) DO NOTHING;

                INSERT INTO fgs."FSGSetupMasterEntityType" (code, name, is_active, created_on)
                VALUES
                    ('TENANT', 'TENANT', TRUE, now()),
                    ('COMPANY', 'COMPANY', TRUE, now()),
                    ('SERVICELOCATION', 'SERVICELOCATION', TRUE, now()),
                    ('BILLTO', 'BILLTO', TRUE, now()),
                    ('VENDOR', 'VENDOR', TRUE, now()),
                    ('SUBCONTRACTOR', 'SUBCONTRACTOR', TRUE, now()),
                    ('LEAD', 'LEAD', TRUE, now()),
                    ('PROPOSAL', 'PROPOSAL', TRUE, now()),
                    ('CUSTOMER', 'CUSTOMER', TRUE, now()),
                    ('WORKORDER', 'WORKORDER', TRUE, now()),
                    ('INVOICE', 'INVOICE', TRUE, now())
                ON CONFLICT (code) DO NOTHING;

                INSERT INTO fgs."FSGSetupLocationType" (code, name, is_active, created_on)
                VALUES
                    ('BILLING', 'BILLING', TRUE, now()),
                    ('SHIPPING', 'SHIPPING', TRUE, now()),
                    ('PHYSICAL', 'PHYSICAL', TRUE, now()),
                    ('SERVICE', 'SERVICE', TRUE, now()),
                    ('MAILING', 'MAILING', TRUE, now()),
                    ('HQ', 'HQ', TRUE, now()),
                    ('REMITTO', 'REMITTO', TRUE, now()),
                    ('JOBSITE', 'JOBSITE', TRUE, now())
                ON CONFLICT (code) DO NOTHING;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FSGSetupAccountingIntegrationType",
                schema: "fgs");

            migrationBuilder.DropTable(
                name: "FSGSetupLanguage",
                schema: "fgs");

            migrationBuilder.DropTable(
                name: "FSGSetupLocationType",
                schema: "fgs");

            migrationBuilder.DropTable(
                name: "FSGSetupMasterEntityType",
                schema: "fgs");
        }
    }
}
