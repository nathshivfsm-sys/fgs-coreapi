using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UserService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTenantCompanyAndAuditable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_invite_tenant_company_tenant_id_company_id",
                schema: "fgs",
                table: "invite");

            migrationBuilder.DropForeignKey(
                name: "FK_invite_tenant_tenant_id",
                schema: "fgs",
                table: "invite");

            migrationBuilder.DropForeignKey(
                name: "FK_tenant_company_tenant_tenant_id",
                schema: "fgs",
                table: "tenant_company");

            migrationBuilder.DropForeignKey(
                name: "FK_users_tenant_company_tenant_id_company_id",
                schema: "fgs",
                table: "users");

            migrationBuilder.DropForeignKey(
                name: "FK_users_tenant_tenant_id",
                schema: "fgs",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_tenant_id_company_id",
                schema: "fgs",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tenant",
                schema: "fgs",
                table: "tenant");

            migrationBuilder.DropIndex(
                name: "IX_invite_tenant_id_company_id",
                schema: "fgs",
                table: "invite");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tenant_company",
                schema: "fgs",
                table: "tenant_company");

            migrationBuilder.DropColumn(
                name: "status",
                schema: "fgs",
                table: "tenant");

            migrationBuilder.DropColumn(
                name: "company_id",
                schema: "fgs",
                table: "tenant_company");

            migrationBuilder.RenameTable(
                name: "tenant",
                schema: "fgs",
                newName: "Tenant",
                newSchema: "fgs");

            migrationBuilder.RenameTable(
                name: "tenant_company",
                schema: "fgs",
                newName: "Company",
                newSchema: "fgs");

            migrationBuilder.RenameColumn(
                name: "created_at",
                schema: "fgs",
                table: "Tenant",
                newName: "created_on");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                schema: "fgs",
                table: "Tenant",
                newName: "updated_on");

            migrationBuilder.RenameColumn(
                name: "created_at",
                schema: "fgs",
                table: "Company",
                newName: "created_on");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                schema: "fgs",
                table: "Company",
                newName: "updated_on");

            migrationBuilder.RenameIndex(
                name: "ix_tenant_company_tenant",
                schema: "fgs",
                table: "Company",
                newName: "ix_company_tenant");

            migrationBuilder.AlterColumn<long>(
                name: "company_id",
                schema: "fgs",
                table: "users",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                schema: "fgs",
                table: "Tenant",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<Guid>(
                name: "created_by",
                schema: "fgs",
                table: "Tenant",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "default_currency",
                schema: "fgs",
                table: "Tenant",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "default_language_id",
                schema: "fgs",
                table: "Tenant",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "email",
                schema: "fgs",
                table: "Tenant",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                schema: "fgs",
                table: "Tenant",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "legal_name",
                schema: "fgs",
                table: "Tenant",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "phone_number",
                schema: "fgs",
                table: "Tenant",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "primary_location_id",
                schema: "fgs",
                table: "Tenant",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "subscription_plan_id",
                schema: "fgs",
                table: "Tenant",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "time_zone",
                schema: "fgs",
                table: "Tenant",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "updated_by",
                schema: "fgs",
                table: "Tenant",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "website",
                schema: "fgs",
                table: "Tenant",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "company_id",
                schema: "fgs",
                table: "invite",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                schema: "fgs",
                table: "Company",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<long>(
                name: "id",
                schema: "fgs",
                table: "Company",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "business_type_id",
                schema: "fgs",
                table: "Company",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "code",
                schema: "fgs",
                table: "Company",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "compact_logo_url",
                schema: "fgs",
                table: "Company",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "company_guid",
                schema: "fgs",
                table: "Company",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<long>(
                name: "company_number",
                schema: "fgs",
                table: "Company",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<Guid>(
                name: "created_by",
                schema: "fgs",
                table: "Company",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "email",
                schema: "fgs",
                table: "Company",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "favicon_url",
                schema: "fgs",
                table: "Company",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "full_logo_url",
                schema: "fgs",
                table: "Company",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "icon_logo_url",
                schema: "fgs",
                table: "Company",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                schema: "fgs",
                table: "Company",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "legal_name",
                schema: "fgs",
                table: "Company",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "phone_number",
                schema: "fgs",
                table: "Company",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "primary_location_id",
                schema: "fgs",
                table: "Company",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tax_id",
                schema: "fgs",
                table: "Company",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "updated_by",
                schema: "fgs",
                table: "Company",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "website",
                schema: "fgs",
                table: "Company",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tenant",
                schema: "fgs",
                table: "Tenant",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Company",
                schema: "fgs",
                table: "Company",
                column: "id");

            migrationBuilder.Sql(
                """
                UPDATE fgs."Company" SET company_guid = gen_random_uuid() WHERE company_guid = '00000000-0000-0000-0000-000000000000';
                UPDATE fgs."Company" SET code = LEFT(COALESCE(NULLIF(TRIM(name), ''), 'company'), 100) WHERE code = '';
                UPDATE fgs."Company" SET business_type_id = 1 WHERE business_type_id = 0;
                UPDATE fgs."Company" SET company_number = 1 WHERE company_number = 0;
                UPDATE fgs.users u SET company_id = m.id FROM (SELECT tenant_id, MIN(id) AS id FROM fgs."Company" GROUP BY tenant_id) m WHERE u.tenant_id = m.tenant_id;
                UPDATE fgs.invite i SET company_id = m.id FROM (SELECT tenant_id, MIN(id) AS id FROM fgs."Company" GROUP BY tenant_id) m WHERE i.tenant_id = m.tenant_id;
                """);

            migrationBuilder.CreateIndex(
                name: "IX_users_company_id",
                schema: "fgs",
                table: "users",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_invite_company_id",
                schema: "fgs",
                table: "invite",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_Company_company_guid",
                schema: "fgs",
                table: "Company",
                column: "company_guid",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Company_Tenant_tenant_id",
                schema: "fgs",
                table: "Company",
                column: "tenant_id",
                principalSchema: "fgs",
                principalTable: "Tenant",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_invite_Company_company_id",
                schema: "fgs",
                table: "invite",
                column: "company_id",
                principalSchema: "fgs",
                principalTable: "Company",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_invite_Tenant_tenant_id",
                schema: "fgs",
                table: "invite",
                column: "tenant_id",
                principalSchema: "fgs",
                principalTable: "Tenant",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_users_Company_company_id",
                schema: "fgs",
                table: "users",
                column: "company_id",
                principalSchema: "fgs",
                principalTable: "Company",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_users_Tenant_tenant_id",
                schema: "fgs",
                table: "users",
                column: "tenant_id",
                principalSchema: "fgs",
                principalTable: "Tenant",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Company_Tenant_tenant_id",
                schema: "fgs",
                table: "Company");

            migrationBuilder.DropForeignKey(
                name: "FK_invite_Company_company_id",
                schema: "fgs",
                table: "invite");

            migrationBuilder.DropForeignKey(
                name: "FK_invite_Tenant_tenant_id",
                schema: "fgs",
                table: "invite");

            migrationBuilder.DropForeignKey(
                name: "FK_users_Company_company_id",
                schema: "fgs",
                table: "users");

            migrationBuilder.DropForeignKey(
                name: "FK_users_Tenant_tenant_id",
                schema: "fgs",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_company_id",
                schema: "fgs",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tenant",
                schema: "fgs",
                table: "Tenant");

            migrationBuilder.DropIndex(
                name: "IX_invite_company_id",
                schema: "fgs",
                table: "invite");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Company",
                schema: "fgs",
                table: "Company");

            migrationBuilder.DropIndex(
                name: "IX_Company_company_guid",
                schema: "fgs",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "created_by",
                schema: "fgs",
                table: "Tenant");

            migrationBuilder.DropColumn(
                name: "default_currency",
                schema: "fgs",
                table: "Tenant");

            migrationBuilder.DropColumn(
                name: "default_language_id",
                schema: "fgs",
                table: "Tenant");

            migrationBuilder.DropColumn(
                name: "email",
                schema: "fgs",
                table: "Tenant");

            migrationBuilder.DropColumn(
                name: "is_active",
                schema: "fgs",
                table: "Tenant");

            migrationBuilder.DropColumn(
                name: "legal_name",
                schema: "fgs",
                table: "Tenant");

            migrationBuilder.DropColumn(
                name: "phone_number",
                schema: "fgs",
                table: "Tenant");

            migrationBuilder.DropColumn(
                name: "primary_location_id",
                schema: "fgs",
                table: "Tenant");

            migrationBuilder.DropColumn(
                name: "subscription_plan_id",
                schema: "fgs",
                table: "Tenant");

            migrationBuilder.DropColumn(
                name: "time_zone",
                schema: "fgs",
                table: "Tenant");

            migrationBuilder.DropColumn(
                name: "updated_by",
                schema: "fgs",
                table: "Tenant");

            migrationBuilder.DropColumn(
                name: "website",
                schema: "fgs",
                table: "Tenant");

            migrationBuilder.DropColumn(
                name: "id",
                schema: "fgs",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "business_type_id",
                schema: "fgs",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "code",
                schema: "fgs",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "compact_logo_url",
                schema: "fgs",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "company_guid",
                schema: "fgs",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "company_number",
                schema: "fgs",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "created_by",
                schema: "fgs",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "email",
                schema: "fgs",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "favicon_url",
                schema: "fgs",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "full_logo_url",
                schema: "fgs",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "icon_logo_url",
                schema: "fgs",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "is_active",
                schema: "fgs",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "legal_name",
                schema: "fgs",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "phone_number",
                schema: "fgs",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "primary_location_id",
                schema: "fgs",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "tax_id",
                schema: "fgs",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "updated_by",
                schema: "fgs",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "website",
                schema: "fgs",
                table: "Company");

            migrationBuilder.RenameTable(
                name: "Tenant",
                schema: "fgs",
                newName: "tenant",
                newSchema: "fgs");

            migrationBuilder.RenameTable(
                name: "Company",
                schema: "fgs",
                newName: "tenant_company",
                newSchema: "fgs");

            migrationBuilder.RenameColumn(
                name: "created_on",
                schema: "fgs",
                table: "tenant",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "updated_on",
                schema: "fgs",
                table: "tenant",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "created_on",
                schema: "fgs",
                table: "tenant_company",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "updated_on",
                schema: "fgs",
                table: "tenant_company",
                newName: "updated_at");

            migrationBuilder.RenameIndex(
                name: "ix_company_tenant",
                schema: "fgs",
                table: "tenant_company",
                newName: "ix_tenant_company_tenant");

            migrationBuilder.AlterColumn<short>(
                name: "company_id",
                schema: "fgs",
                table: "users",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                schema: "fgs",
                table: "tenant",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<string>(
                name: "status",
                schema: "fgs",
                table: "tenant",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<short>(
                name: "company_id",
                schema: "fgs",
                table: "invite",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                schema: "fgs",
                table: "tenant_company",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<short>(
                name: "company_id",
                schema: "fgs",
                table: "tenant_company",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tenant",
                schema: "fgs",
                table: "tenant",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tenant_company",
                schema: "fgs",
                table: "tenant_company",
                columns: new[] { "tenant_id", "company_id" });

            migrationBuilder.CreateIndex(
                name: "IX_users_tenant_id_company_id",
                schema: "fgs",
                table: "users",
                columns: new[] { "tenant_id", "company_id" });

            migrationBuilder.CreateIndex(
                name: "IX_invite_tenant_id_company_id",
                schema: "fgs",
                table: "invite",
                columns: new[] { "tenant_id", "company_id" });

            migrationBuilder.AddForeignKey(
                name: "FK_invite_tenant_company_tenant_id_company_id",
                schema: "fgs",
                table: "invite",
                columns: new[] { "tenant_id", "company_id" },
                principalSchema: "fgs",
                principalTable: "tenant_company",
                principalColumns: new[] { "tenant_id", "company_id" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_invite_tenant_tenant_id",
                schema: "fgs",
                table: "invite",
                column: "tenant_id",
                principalSchema: "fgs",
                principalTable: "tenant",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tenant_company_tenant_tenant_id",
                schema: "fgs",
                table: "tenant_company",
                column: "tenant_id",
                principalSchema: "fgs",
                principalTable: "tenant",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_users_tenant_company_tenant_id_company_id",
                schema: "fgs",
                table: "users",
                columns: new[] { "tenant_id", "company_id" },
                principalSchema: "fgs",
                principalTable: "tenant_company",
                principalColumns: new[] { "tenant_id", "company_id" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_users_tenant_tenant_id",
                schema: "fgs",
                table: "users",
                column: "tenant_id",
                principalSchema: "fgs",
                principalTable: "tenant",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
