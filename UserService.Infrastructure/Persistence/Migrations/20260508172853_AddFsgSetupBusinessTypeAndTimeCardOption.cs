using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UserService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFsgSetupBusinessTypeAndTimeCardOption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FSGSetupBusinessType",
                schema: "fgs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FSGSetupBusinessType", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "FSGSetupTimeCardOption",
                schema: "fgs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FSGSetupTimeCardOption", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Company_business_type_id",
                schema: "fgs",
                table: "Company",
                column: "business_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_FSGSetupBusinessType_code",
                schema: "fgs",
                table: "FSGSetupBusinessType",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FSGSetupTimeCardOption_code",
                schema: "fgs",
                table: "FSGSetupTimeCardOption",
                column: "code",
                unique: true);

            migrationBuilder.Sql(
                """
                INSERT INTO fgs."FSGSetupBusinessType" (code, name, sort_order, is_active, created_on)
                VALUES
                    ('HVAC', 'HVAC', 1, TRUE, now()),
                    ('PLUMBING', 'Plumbing', 2, TRUE, now()),
                    ('ELECTRICAL', 'Electrical', 3, TRUE, now()),
                    ('PESTCONTROL', 'Pest Control', 4, TRUE, now()),
                    ('LAWNCARE', 'Lawn Care', 5, TRUE, now()),
                    ('TRASHPICKUP', 'Trash Pickup', 6, TRUE, now()),
                    ('GARAGEDOOR', 'Garage Door', 7, TRUE, now()),
                    ('HOUSECLEANING', 'House Cleaning', 8, TRUE, now()),
                    ('PAINTING', 'Painting', 9, TRUE, now())
                ON CONFLICT (code) DO NOTHING;

                INSERT INTO fgs."FSGSetupTimeCardOption" (code, description, is_active, created_on)
                VALUES
                    ('NONE', 'No formal technician time tracking workflow', TRUE, now()),
                    ('DISPATCHARRIVECOMPLETE', 'Tracks dispatch, arrival, and completion timestamps', TRUE, now()),
                    ('CHECKINCHECKOUT', 'Technician manually checks in and checks out', TRUE, now())
                ON CONFLICT (code) DO NOTHING;
                """);

            migrationBuilder.AddForeignKey(
                name: "FK_Company_FSGSetupBusinessType_business_type_id",
                schema: "fgs",
                table: "Company",
                column: "business_type_id",
                principalSchema: "fgs",
                principalTable: "FSGSetupBusinessType",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Company_FSGSetupBusinessType_business_type_id",
                schema: "fgs",
                table: "Company");

            migrationBuilder.DropTable(
                name: "FSGSetupBusinessType",
                schema: "fgs");

            migrationBuilder.DropTable(
                name: "FSGSetupTimeCardOption",
                schema: "fgs");

            migrationBuilder.DropIndex(
                name: "IX_Company_business_type_id",
                schema: "fgs",
                table: "Company");
        }
    }
}
