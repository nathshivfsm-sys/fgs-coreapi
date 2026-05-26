using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.User.Infrastructure.Persistence.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddFgsJobTypeTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FgsJobTypeCategory",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CategoryCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsJobTypeCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsJobTypeCategory_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsJobTypeSubCategory",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    SubCategoryCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsJobTypeSubCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsJobTypeSubCategory_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsJobType",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    JobTypeCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    JobTypeSubCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    JobTypeCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TaskName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    UsedFor = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Trade = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    EstimatedDurationMinutes = table.Column<int>(type: "integer", nullable: true),
                    BusinessUnit = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Priority = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)5),
                    BackgroundColor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    TextColor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    ShowToFieldTech = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    ShowOnCustomerPortal = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsJobType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsJobType_FgsJobTypeCategory",
                        column: x => x.JobTypeCategoryId,
                        principalSchema: "dbo",
                        principalTable: "FgsJobTypeCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsJobType_FgsJobTypeSubCategory",
                        column: x => x.JobTypeSubCategoryId,
                        principalSchema: "dbo",
                        principalTable: "FgsJobTypeSubCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsJobType_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobType_JobTypeCategoryId",
                schema: "dbo",
                table: "FgsJobType",
                column: "JobTypeCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobType_JobTypeSubCategoryId",
                schema: "dbo",
                table: "FgsJobType",
                column: "JobTypeSubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobType_Tenant_Company",
                schema: "dbo",
                table: "FgsJobType",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobType_Tenant_Company_BusinessUnit",
                schema: "dbo",
                table: "FgsJobType",
                columns: new[] { "TenantId", "CompanyId", "BusinessUnit" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobType_Tenant_Company_Trade",
                schema: "dbo",
                table: "FgsJobType",
                columns: new[] { "TenantId", "CompanyId", "Trade" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobType_Tenant_Company_UsedFor",
                schema: "dbo",
                table: "FgsJobType",
                columns: new[] { "TenantId", "CompanyId", "UsedFor" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsJobType_Tenant_Company_JobTypeCode",
                schema: "dbo",
                table: "FgsJobType",
                columns: new[] { "TenantId", "CompanyId", "JobTypeCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobTypeCategory_Tenant_Company",
                schema: "dbo",
                table: "FgsJobTypeCategory",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsJobTypeCategory_Tenant_Company_CategoryCode",
                schema: "dbo",
                table: "FgsJobTypeCategory",
                columns: new[] { "TenantId", "CompanyId", "CategoryCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobTypeSubCategory_Tenant_Company",
                schema: "dbo",
                table: "FgsJobTypeSubCategory",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsJobTypeSubCategory_Tenant_Company_SubCategoryCode",
                schema: "dbo",
                table: "FgsJobTypeSubCategory",
                columns: new[] { "TenantId", "CompanyId", "SubCategoryCode" },
                unique: true);

            migrationBuilder.Sql(
                """
                COMMENT ON TABLE dbo."FgsJobTypeCategory" IS 'Stores tenant/company specific job type categories used to classify service and work order types. Seeded from GloJobTypeCategory.';
                COMMENT ON COLUMN dbo."FgsJobTypeCategory"."Id" IS 'Surrogate primary key.';
                COMMENT ON COLUMN dbo."FgsJobTypeCategory"."TenantId" IS 'Owning tenant identifier.';
                COMMENT ON COLUMN dbo."FgsJobTypeCategory"."CompanyId" IS 'Tenant-scoped company number (maps to FgsTenantCompany.CompanyNumber).';
                COMMENT ON COLUMN dbo."FgsJobTypeCategory"."CategoryCode" IS 'Unique category code within tenant and company scope.';
                COMMENT ON COLUMN dbo."FgsJobTypeCategory"."Name" IS 'Display name of the job type category.';
                COMMENT ON COLUMN dbo."FgsJobTypeCategory"."Description" IS 'Optional description of the category.';
                COMMENT ON COLUMN dbo."FgsJobTypeCategory"."DisplayOrder" IS 'Controls sorting/display order in dropdowns and setup screens.';
                COMMENT ON COLUMN dbo."FgsJobTypeCategory"."IsActive" IS 'Indicates whether the category is active and available for use.';
                COMMENT ON COLUMN dbo."FgsJobTypeCategory"."CreatedOn" IS 'UTC timestamp when the record was created.';
                COMMENT ON COLUMN dbo."FgsJobTypeCategory"."CreatedBy" IS 'User or process that created the record.';
                COMMENT ON COLUMN dbo."FgsJobTypeCategory"."UpdatedOn" IS 'UTC timestamp of the last update.';
                COMMENT ON COLUMN dbo."FgsJobTypeCategory"."UpdatedBy" IS 'User or process that last updated the record.';

                COMMENT ON TABLE dbo."FgsJobTypeSubCategory" IS 'Stores tenant/company specific job type subcategories (install, repair, service, etc.). Seeded from GloJobTypeSubCategory.';
                COMMENT ON COLUMN dbo."FgsJobTypeSubCategory"."Id" IS 'Surrogate primary key.';
                COMMENT ON COLUMN dbo."FgsJobTypeSubCategory"."TenantId" IS 'Owning tenant identifier.';
                COMMENT ON COLUMN dbo."FgsJobTypeSubCategory"."CompanyId" IS 'Tenant-scoped company number (maps to FgsTenantCompany.CompanyNumber).';
                COMMENT ON COLUMN dbo."FgsJobTypeSubCategory"."SubCategoryCode" IS 'Unique subcategory code within tenant and company scope.';
                COMMENT ON COLUMN dbo."FgsJobTypeSubCategory"."Name" IS 'Display name of the job type subcategory.';
                COMMENT ON COLUMN dbo."FgsJobTypeSubCategory"."Description" IS 'Optional description of the subcategory.';
                COMMENT ON COLUMN dbo."FgsJobTypeSubCategory"."DisplayOrder" IS 'Controls sorting/display order in dropdowns and setup screens.';
                COMMENT ON COLUMN dbo."FgsJobTypeSubCategory"."IsActive" IS 'Indicates whether the subcategory is active and available for use.';
                COMMENT ON COLUMN dbo."FgsJobTypeSubCategory"."CreatedOn" IS 'UTC timestamp when the record was created.';
                COMMENT ON COLUMN dbo."FgsJobTypeSubCategory"."CreatedBy" IS 'User or process that created the record.';
                COMMENT ON COLUMN dbo."FgsJobTypeSubCategory"."UpdatedOn" IS 'UTC timestamp of the last update.';
                COMMENT ON COLUMN dbo."FgsJobTypeSubCategory"."UpdatedBy" IS 'User or process that last updated the record.';

                COMMENT ON TABLE dbo."FgsJobType" IS 'Stores tenant and company specific job/service types used for dispatching, scheduling, maintenance, installation, warranty, and service operations. Job types are linked to category and subcategory classifications and define operational settings such as trade, estimated duration, business unit, priority, UI display settings, and customer/field visibility.';
                COMMENT ON COLUMN dbo."FgsJobType"."Id" IS 'Surrogate primary key.';
                COMMENT ON COLUMN dbo."FgsJobType"."TenantId" IS 'Owning tenant identifier.';
                COMMENT ON COLUMN dbo."FgsJobType"."CompanyId" IS 'Tenant-scoped company number (maps to FgsTenantCompany.CompanyNumber).';
                COMMENT ON COLUMN dbo."FgsJobType"."JobTypeCategoryId" IS 'Reference to the parent job type category.';
                COMMENT ON COLUMN dbo."FgsJobType"."JobTypeSubCategoryId" IS 'Optional reference to the job type subcategory.';
                COMMENT ON COLUMN dbo."FgsJobType"."JobTypeCode" IS 'Unique job type code within tenant and company scope.';
                COMMENT ON COLUMN dbo."FgsJobType"."TaskName" IS 'Display name of the job or service task (e.g. Emergency AC Repair).';
                COMMENT ON COLUMN dbo."FgsJobType"."Description" IS 'Optional detailed description of the job type.';
                COMMENT ON COLUMN dbo."FgsJobType"."UsedFor" IS 'Operational classification such as Service, Maintenance, Install, or Warranty.';
                COMMENT ON COLUMN dbo."FgsJobType"."Trade" IS 'Trade associated with the job type such as HVAC, Plumbing, or Electrical.';
                COMMENT ON COLUMN dbo."FgsJobType"."EstimatedDurationMinutes" IS 'Estimated duration of the job in minutes.';
                COMMENT ON COLUMN dbo."FgsJobType"."BusinessUnit" IS 'Business unit classification such as Residential, Commercial, or Retrofit.';
                COMMENT ON COLUMN dbo."FgsJobType"."Priority" IS 'Scheduling priority where lower numbers indicate higher priority.';
                COMMENT ON COLUMN dbo."FgsJobType"."BackgroundColor" IS 'UI background color for calendar and dispatch displays.';
                COMMENT ON COLUMN dbo."FgsJobType"."TextColor" IS 'UI text color for calendar and dispatch displays.';
                COMMENT ON COLUMN dbo."FgsJobType"."ShowToFieldTech" IS 'Indicates whether the job type is visible to field technicians.';
                COMMENT ON COLUMN dbo."FgsJobType"."ShowOnCustomerPortal" IS 'Indicates whether the job type is visible on the customer portal.';
                COMMENT ON COLUMN dbo."FgsJobType"."DisplayOrder" IS 'Controls sorting/display order in dropdowns and setup screens.';
                COMMENT ON COLUMN dbo."FgsJobType"."IsActive" IS 'Indicates whether the job type is active and available for use.';
                COMMENT ON COLUMN dbo."FgsJobType"."CreatedOn" IS 'UTC timestamp when the record was created.';
                COMMENT ON COLUMN dbo."FgsJobType"."CreatedBy" IS 'User or process that created the record.';
                COMMENT ON COLUMN dbo."FgsJobType"."UpdatedOn" IS 'UTC timestamp of the last update.';
                COMMENT ON COLUMN dbo."FgsJobType"."UpdatedBy" IS 'User or process that last updated the record.';
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsJobType",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsJobTypeCategory",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsJobTypeSubCategory",
                schema: "dbo");
        }
    }
}
