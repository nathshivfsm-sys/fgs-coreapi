using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Setup.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class RedesignJobTypeHierarchy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Dependent PriceBook FK blocks drop of FgsJobType.
            migrationBuilder.DropForeignKey(
                name: "FK_FgsPriceBook_JobTypeId",
                schema: "setup",
                table: "FgsPriceBook");

            // Clear dependent price-book rows (JobTypeId targets will be gone).
            migrationBuilder.Sql("""DELETE FROM setup."FgsPriceBookItem";""");
            migrationBuilder.Sql("""DELETE FROM setup."FgsPriceBook";""");

            migrationBuilder.DropTable(
                name: "FgsJobType",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsJobTypeCategory",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsJobTypeSubCategory",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "GloJobTypeSubCategory",
                schema: "glo");

            migrationBuilder.CreateTable(
                name: "FgsJobCategory",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Unique identifier for the Job Category.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifier of the tenant that owns this Job Category."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifier of the company within the tenant that owns this Job Category."),
                    CategoryCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Unique business code used to identify the Job Category within a tenant and company."),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false, comment: "Display name of the Job Category."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Controls the display sequence of Job Categories in lists and selection controls."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time when the Job Category was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who created the Job Category."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time when the Job Category was last modified."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who last modified the Job Category."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the Job Category is active and available for selection.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsJobCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsJobCategory_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores the master list of Job Categories available for configuring Job Types. Categories organize related Job Tasks within a Job Type.");

            migrationBuilder.CreateTable(
                name: "FgsJobType",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Unique identifier for the Job Type.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifier of the tenant that owns this Job Type."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifier of the company within the tenant that owns this Job Type."),
                    JobTypeCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Unique business code used to identify the Job Type within a tenant and company."),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Display name of the Job Type shown throughout the application."),
                    UsedFor = table.Column<short>(type: "smallint", nullable: false, comment: "Specifies the business process for the Job Type. Valid values: 1=Service, 2=Maintenance, 3=Warranty, 4=Installation. Corresponds to the JobTypeUsedFor enum in the application."),
                    BusinessUnit = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Optional business unit or department responsible for this Job Type."),
                    BackgroundColor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, comment: "Optional background color used when displaying the Job Type in the user interface."),
                    TextColor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, comment: "Optional text color used when displaying the Job Type in the user interface."),
                    ShowToFieldTech = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether this Job Type is visible to field technicians in the mobile application."),
                    ShowOnCustomerPortal = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether this Job Type is available for customers through the customer portal."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Controls the display sequence of Job Types in lists and selection controls."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time when the Job Type was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who created the Job Type."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time when the Job Type was last modified."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who last modified the Job Type."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the Job Type is active and available for new work orders.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsJobType", x => x.Id);
                    table.CheckConstraint("CK_FgsJobType_UsedFor", "\"UsedFor\" IN (1, 2, 3, 4)");
                    table.ForeignKey(
                        name: "FK_FgsJobType_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Defines reusable Job Types that represent the type of work performed. A Job Type serves as the header for one or more Job Type Categories and their associated tasks.");

            migrationBuilder.CreateTable(
                name: "FgsJobTypeCategory",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Unique identifier for the Job Type Category mapping.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifier of the tenant that owns this Job Type Category mapping."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifier of the company within the tenant that owns this Job Type Category mapping."),
                    JobTypeId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifier of the Job Type."),
                    JobCategoryId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifier of the Job Category assigned to the Job Type."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Controls the display sequence of Job Categories within the Job Type."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time when the mapping was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who created the mapping."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time when the mapping was last modified."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who last modified the mapping."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the Job Category assignment is active.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsJobTypeCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsJobTypeCategory_FgsJobCategory",
                        column: x => x.JobCategoryId,
                        principalSchema: "setup",
                        principalTable: "FgsJobCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsJobTypeCategory_FgsJobType",
                        column: x => x.JobTypeId,
                        principalSchema: "setup",
                        principalTable: "FgsJobType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FgsJobTypeCategory_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Maps Job Categories to Job Types. A Job Type can contain one or more Job Categories, each with its own display order.");

            migrationBuilder.CreateTable(
                name: "FgsJobTypeTask",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Unique identifier for the Job Type Task.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifier of the tenant that owns this Job Type Task."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifier of the company within the tenant that owns this Job Type Task."),
                    JobTypeCategoryId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifier of the Job Type Category that owns this task."),
                    TradeId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifier of the Trade responsible for performing this task."),
                    TaskName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Name of the task to be performed."),
                    Priority = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)5, comment: "Execution priority for the task. Lower values typically represent higher priority."),
                    EstimatedHours = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false, defaultValue: 1.00m, comment: "Estimated labor hours required to complete the task."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Controls the display sequence of tasks within the Job Type Category."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time when the Job Type Task was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who created the Job Type Task."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time when the Job Type Task was last modified."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who last modified the Job Type Task."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the Job Type Task is active and available for use.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsJobTypeTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsJobTypeTask_FgsJobTypeCategory",
                        column: x => x.JobTypeCategoryId,
                        principalSchema: "setup",
                        principalTable: "FgsJobTypeCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FgsJobTypeTask_FgsSetupTechTrade",
                        column: x => x.TradeId,
                        principalSchema: "setup",
                        principalTable: "FgsSetupTechTrade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsJobTypeTask_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores the tasks that belong to a Job Type Category. Each task defines the work to be performed, along with its associated Trade, Priority, and estimated labor hours.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobCategory_Tenant_Company",
                schema: "setup",
                table: "FgsJobCategory",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsJobCategory_Tenant_Company_CategoryCode",
                schema: "setup",
                table: "FgsJobCategory",
                columns: new[] { "TenantId", "CompanyId", "CategoryCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobType_Tenant_Company",
                schema: "setup",
                table: "FgsJobType",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobType_Tenant_Company_BusinessUnit",
                schema: "setup",
                table: "FgsJobType",
                columns: new[] { "TenantId", "CompanyId", "BusinessUnit" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobType_Tenant_Company_UsedFor",
                schema: "setup",
                table: "FgsJobType",
                columns: new[] { "TenantId", "CompanyId", "UsedFor" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsJobType_Tenant_Company_JobTypeCode",
                schema: "setup",
                table: "FgsJobType",
                columns: new[] { "TenantId", "CompanyId", "JobTypeCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_FgsJobType_Tenant_Company_Name",
                schema: "setup",
                table: "FgsJobType",
                columns: new[] { "TenantId", "CompanyId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobTypeCategory_JobCategoryId",
                schema: "setup",
                table: "FgsJobTypeCategory",
                column: "JobCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobTypeCategory_JobTypeId",
                schema: "setup",
                table: "FgsJobTypeCategory",
                column: "JobTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobTypeCategory_Tenant_Company",
                schema: "setup",
                table: "FgsJobTypeCategory",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobTypeCategory_Tenant_Company_DisplayOrder",
                schema: "setup",
                table: "FgsJobTypeCategory",
                columns: new[] { "TenantId", "CompanyId", "JobTypeId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobTypeCategory_Tenant_Company_JobCategory",
                schema: "setup",
                table: "FgsJobTypeCategory",
                columns: new[] { "TenantId", "CompanyId", "JobCategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobTypeCategory_Tenant_Company_JobType",
                schema: "setup",
                table: "FgsJobTypeCategory",
                columns: new[] { "TenantId", "CompanyId", "JobTypeId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsJobTypeCategory_Tenant_Company_JobType_Category",
                schema: "setup",
                table: "FgsJobTypeCategory",
                columns: new[] { "TenantId", "CompanyId", "JobTypeId", "JobCategoryId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobTypeTask_JobTypeCategoryId",
                schema: "setup",
                table: "FgsJobTypeTask",
                column: "JobTypeCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobTypeTask_Tenant_Company",
                schema: "setup",
                table: "FgsJobTypeTask",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobTypeTask_Tenant_Company_JobTypeCategory",
                schema: "setup",
                table: "FgsJobTypeTask",
                columns: new[] { "TenantId", "CompanyId", "JobTypeCategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobTypeTask_TradeId",
                schema: "setup",
                table: "FgsJobTypeTask",
                column: "TradeId");

            migrationBuilder.AddForeignKey(
                name: "FK_FgsPriceBook_JobTypeId",
                schema: "setup",
                table: "FgsPriceBook",
                column: "JobTypeId",
                principalSchema: "setup",
                principalTable: "FgsJobType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Irreversible data loss: new hierarchy tables are dropped; old catalog shape is restored empty.
            migrationBuilder.DropForeignKey(
                name: "FK_FgsPriceBook_JobTypeId",
                schema: "setup",
                table: "FgsPriceBook");

            migrationBuilder.Sql("""DELETE FROM setup."FgsPriceBookItem";""");
            migrationBuilder.Sql("""DELETE FROM setup."FgsPriceBook";""");

            migrationBuilder.DropTable(
                name: "FgsJobTypeTask",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsJobTypeCategory",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsJobType",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsJobCategory",
                schema: "setup");

            migrationBuilder.CreateTable(
                name: "FgsJobTypeCategory",
                schema: "setup",
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
                        name: "FK_FgsJobTypeCategory_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsJobTypeSubCategory",
                schema: "setup",
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
                        name: "FK_FgsJobTypeSubCategory_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GloJobTypeSubCategory",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    BusinessTypeId = table.Column<int>(type: "integer", nullable: true),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloJobTypeSubCategory", x => x.Id);
                    table.CheckConstraint("CK_GloJobTypeSubCategory_Code_Upper", "\"Code\" = upper(\"Code\")");
                    table.ForeignKey(
                        name: "FK_GloJobTypeSubCategory_GloBusinessType_BusinessTypeId",
                        column: x => x.BusinessTypeId,
                        principalSchema: "glo",
                        principalTable: "GloBusinessType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsJobType",
                schema: "setup",
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
                        principalSchema: "setup",
                        principalTable: "FgsJobTypeCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsJobType_FgsJobTypeSubCategory",
                        column: x => x.JobTypeSubCategoryId,
                        principalSchema: "setup",
                        principalTable: "FgsJobTypeSubCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsJobType_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "UX_FgsJobTypeCategory_Tenant_Company_CategoryCode",
                schema: "setup",
                table: "FgsJobTypeCategory",
                columns: new[] { "TenantId", "CompanyId", "CategoryCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobTypeCategory_Tenant_Company",
                schema: "setup",
                table: "FgsJobTypeCategory",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobTypeSubCategory_Tenant_Company",
                schema: "setup",
                table: "FgsJobTypeSubCategory",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsJobTypeSubCategory_Tenant_Company_SubCategoryCode",
                schema: "setup",
                table: "FgsJobTypeSubCategory",
                columns: new[] { "TenantId", "CompanyId", "SubCategoryCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloJobTypeSubCategory_BusinessTypeId",
                schema: "glo",
                table: "GloJobTypeSubCategory",
                column: "BusinessTypeId");

            migrationBuilder.CreateIndex(
                name: "UQ_GloJobTypeSubCategory_Code",
                schema: "glo",
                table: "GloJobTypeSubCategory",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobType_JobTypeCategoryId",
                schema: "setup",
                table: "FgsJobType",
                column: "JobTypeCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobType_JobTypeSubCategoryId",
                schema: "setup",
                table: "FgsJobType",
                column: "JobTypeSubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobType_Tenant_Company",
                schema: "setup",
                table: "FgsJobType",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobType_Tenant_Company_BusinessUnit",
                schema: "setup",
                table: "FgsJobType",
                columns: new[] { "TenantId", "CompanyId", "BusinessUnit" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobType_Tenant_Company_Trade",
                schema: "setup",
                table: "FgsJobType",
                columns: new[] { "TenantId", "CompanyId", "Trade" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobType_Tenant_Company_UsedFor",
                schema: "setup",
                table: "FgsJobType",
                columns: new[] { "TenantId", "CompanyId", "UsedFor" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsJobType_Tenant_Company_JobTypeCode",
                schema: "setup",
                table: "FgsJobType",
                columns: new[] { "TenantId", "CompanyId", "JobTypeCode" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsPriceBook_JobTypeId",
                schema: "setup",
                table: "FgsPriceBook",
                column: "JobTypeId",
                principalSchema: "setup",
                principalTable: "FgsJobType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
