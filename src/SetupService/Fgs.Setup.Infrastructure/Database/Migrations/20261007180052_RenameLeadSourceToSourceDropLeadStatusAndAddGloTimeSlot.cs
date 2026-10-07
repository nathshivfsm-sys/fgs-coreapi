using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Setup.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class RenameLeadSourceToSourceDropLeadStatusAndAddGloTimeSlot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsLeadDisqualificationReason",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsLeadStatus",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "GloLeadDisqualificationReason",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "GloLeadStatus",
                schema: "glo");

            migrationBuilder.Sql("""
                ALTER TABLE glo."GloLeadSource" RENAME TO "GloSource";
                ALTER TABLE glo."GloSource" RENAME CONSTRAINT "PK_GloLeadSource" TO "PK_GloSource";
                ALTER INDEX glo."UX_GloLeadSource_SourceCode" RENAME TO "UX_GloSource_SourceCode";

                ALTER TABLE setup."FgsLeadSource" RENAME TO "FgsSource";
                ALTER TABLE setup."FgsSource" RENAME CONSTRAINT "PK_FgsLeadSource" TO "PK_FgsSource";
                ALTER TABLE setup."FgsSource" RENAME CONSTRAINT "FK_FgsLeadSource_FgsTenantCompanyCache_TenantId_CompanyId" TO "FK_FgsSource_FgsTenantCompanyCache_TenantId_CompanyId";
                ALTER INDEX setup."UX_FgsLeadSource_TenantId_CompanyId_SourceCode" RENAME TO "UX_FgsSource_TenantId_CompanyId_SourceCode";
                """);

            migrationBuilder.CreateTable(
                name: "GloTimeSlot",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    BeginTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    MarkTechArrivedLateAfter = table.Column<TimeSpan>(type: "interval", nullable: true),
                    MarkWorkOrderDelayedCompletionAfter = table.Column<TimeSpan>(type: "interval", nullable: true),
                    IsMobileVisible = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsCustomerPortalVisible = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IncludeInCapacityPlanning = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    ShowToExternalSystem = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloTimeSlot", x => x.Id);
                    table.CheckConstraint("CK_GloTimeSlot_Code_Upper", "\"Code\" = UPPER(\"Code\")");
                    table.CheckConstraint("CK_GloTimeSlot_TimeRange", "\"EndTime\" > \"BeginTime\"");
                });

            migrationBuilder.CreateIndex(
                name: "UQ_GloTimeSlot_Code",
                schema: "glo",
                table: "GloTimeSlot",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GloTimeSlot",
                schema: "glo");

            migrationBuilder.Sql("""
                ALTER TABLE glo."GloSource" RENAME TO "GloLeadSource";
                ALTER TABLE glo."GloLeadSource" RENAME CONSTRAINT "PK_GloSource" TO "PK_GloLeadSource";
                ALTER INDEX glo."UX_GloSource_SourceCode" RENAME TO "UX_GloLeadSource_SourceCode";

                ALTER TABLE setup."FgsSource" RENAME TO "FgsLeadSource";
                ALTER TABLE setup."FgsLeadSource" RENAME CONSTRAINT "PK_FgsSource" TO "PK_FgsLeadSource";
                ALTER TABLE setup."FgsLeadSource" RENAME CONSTRAINT "FK_FgsSource_FgsTenantCompanyCache_TenantId_CompanyId" TO "FK_FgsLeadSource_FgsTenantCompanyCache_TenantId_CompanyId";
                ALTER INDEX setup."UX_FgsSource_TenantId_CompanyId_SourceCode" RENAME TO "UX_FgsLeadSource_TenantId_CompanyId_SourceCode";
                """);

            migrationBuilder.CreateTable(
                name: "FgsLeadDisqualificationReason",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "Optional description explaining the reason."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Controls the order in which reasons are displayed in dropdowns and lists."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the reason is available for selection."),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether the reason was seeded by the system or created by a user."),
                    ReasonCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Unique business code for the disqualification reason within a company."),
                    ReasonName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "User-friendly name displayed throughout the application."),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsLeadDisqualificationReason", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsLeadDisqualificationReason_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores tenant/company specific lead disqualification reasons used when leads are marked as disqualified.");

            migrationBuilder.CreateTable(
                name: "FgsLeadStatus",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifier of the company that owns the lead status."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who created the record."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time when the record was created."),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "Optional description explaining the purpose of the lead status."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Determines the order in which statuses appear in dropdowns, lists, and reports."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the status is available for selection and use."),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether the record was seeded by the system or created by a user."),
                    StatusCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Unique business code for the lead status within a company. Examples: NEW, CONTACTED, QUALIFIED, CONVERTED."),
                    StatusName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "User-friendly name displayed throughout the application."),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifier of the tenant that owns the lead status."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who last updated the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time when the record was last updated.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsLeadStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsLeadStatus_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores tenant/company specific lead statuses used in the CRM lead lifecycle. Seeded from glo.GloLeadStatus during onboarding and may be customized by users.");

            migrationBuilder.CreateTable(
                name: "GloLeadDisqualificationReason",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "Optional description explaining the reason."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Controls the order in which reasons are displayed."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the reason is available for seeding and use."),
                    ReasonCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Unique business code for the disqualification reason."),
                    ReasonName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "User-friendly name displayed throughout the application."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloLeadDisqualificationReason", x => x.Id);
                },
                comment: "Master list of lead disqualification reasons used to seed tenant-specific records into setup.FgsLeadDisqualificationReason.");

            migrationBuilder.CreateTable(
                name: "GloLeadStatus",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    StatusCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    StatusName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloLeadStatus", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FgsLeadDisqualificationReason_TenantId_CompanyId",
                schema: "setup",
                table: "FgsLeadDisqualificationReason",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsLeadDisqualificationReason_TenantId_CompanyId_DisplayOrder",
                schema: "setup",
                table: "FgsLeadDisqualificationReason",
                columns: new[] { "TenantId", "CompanyId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsLeadDisqualificationReason_TenantId_CompanyId_ReasonCode",
                schema: "setup",
                table: "FgsLeadDisqualificationReason",
                columns: new[] { "TenantId", "CompanyId", "ReasonCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_FgsLeadDisqualificationReason_TenantId_CompanyId_ReasonName",
                schema: "setup",
                table: "FgsLeadDisqualificationReason",
                columns: new[] { "TenantId", "CompanyId", "ReasonName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsLeadStatus_TenantId_CompanyId_DisplayOrder",
                schema: "setup",
                table: "FgsLeadStatus",
                columns: new[] { "TenantId", "CompanyId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsLeadStatus_TenantId_CompanyId_StatusCode",
                schema: "setup",
                table: "FgsLeadStatus",
                columns: new[] { "TenantId", "CompanyId", "StatusCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_FgsLeadStatus_TenantId_CompanyId_StatusName",
                schema: "setup",
                table: "FgsLeadStatus",
                columns: new[] { "TenantId", "CompanyId", "StatusName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloLeadDisqualificationReason_DisplayOrder",
                schema: "glo",
                table: "GloLeadDisqualificationReason",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "UX_GloLeadDisqualificationReason_ReasonCode",
                schema: "glo",
                table: "GloLeadDisqualificationReason",
                column: "ReasonCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_GloLeadDisqualificationReason_ReasonName",
                schema: "glo",
                table: "GloLeadDisqualificationReason",
                column: "ReasonName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloLeadStatus_DisplayOrder",
                schema: "glo",
                table: "GloLeadStatus",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "UX_GloLeadStatus_StatusCode",
                schema: "glo",
                table: "GloLeadStatus",
                column: "StatusCode",
                unique: true);
        }
    }
}
