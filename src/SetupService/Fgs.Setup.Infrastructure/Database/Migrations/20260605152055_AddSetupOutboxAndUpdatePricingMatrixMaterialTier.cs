using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Setup.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddSetupOutboxAndUpdatePricingMatrixMaterialTier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FgsSetupPricingMatrixMaterialTier_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupPricingMatrixMaterialTier");

            migrationBuilder.DropCheckConstraint(
                name: "CK_FgsSetupPricingMatrixMaterialTier_DiscountPercent",
                schema: "setup",
                table: "FgsSetupPricingMatrixMaterialTier");

            migrationBuilder.DropCheckConstraint(
                name: "CK_FgsSetupPricingMatrixMaterialTier_MarkupPercent",
                schema: "setup",
                table: "FgsSetupPricingMatrixMaterialTier");

            migrationBuilder.DropColumn(
                name: "DiscountPercent",
                schema: "setup",
                table: "FgsSetupPricingMatrixMaterialTier");

            migrationBuilder.DropColumn(
                name: "MarkupPercent",
                schema: "setup",
                table: "FgsSetupPricingMatrixMaterialTier");

            migrationBuilder.AlterTable(
                name: "FgsSetupPricingMatrixMaterialTier",
                schema: "setup",
                comment: "Defines material cost tiers and pricing adjustments used by a pricing matrix. Each tier applies a single pricing adjustment method to determine the selling price from material cost.");

            migrationBuilder.AlterColumn<decimal>(
                name: "ToCost",
                schema: "setup",
                table: "FgsSetupPricingMatrixMaterialTier",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                comment: "Inclusive maximum material cost for this pricing tier. NULL indicates no upper limit.",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "FromCost",
                schema: "setup",
                table: "FgsSetupPricingMatrixMaterialTier",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                comment: "Inclusive minimum material cost for this pricing tier.",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<Guid>(
                name: "FgsSetupPricingMatrixId",
                schema: "setup",
                table: "FgsSetupPricingMatrixMaterialTier",
                type: "uuid",
                nullable: false,
                comment: "Reference to the pricing matrix that contains this tier.",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<decimal>(
                name: "AdjustmentValue",
                schema: "setup",
                table: "FgsSetupPricingMatrixMaterialTier",
                type: "numeric(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                defaultValue: 0m,
                comment: "Value used by the selected pricing adjustment type. Examples: 25 = 25% markup, 150 = $150 markup, 1.75 = multiplier.");

            migrationBuilder.AddColumn<short>(
                name: "PriceAdjustmentTypeId",
                schema: "setup",
                table: "FgsSetupPricingMatrixMaterialTier",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0,
                comment: "Pricing adjustment method. Valid values: 1=Markup Percent, 2=Markup Amount, 3=Multiplier.");

            migrationBuilder.CreateTable(
                name: "SetupOutboxMessage",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: true),
                    CompanyId = table.Column<long>(type: "bigint", nullable: true),
                    EventType = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    AggregateType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AggregateId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CorrelationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CausationId = table.Column<Guid>(type: "uuid", nullable: true),
                    ExchangeName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    RoutingKey = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Payload = table.Column<string>(type: "jsonb", nullable: false),
                    Headers = table.Column<string>(type: "jsonb", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "Pending"),
                    RetryCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    MaxRetryCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 10),
                    NextRetryOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    ProcessedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    LastError = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetupOutboxMessage", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPricingMatrixMaterialTier_PriceAdjustmentTypeId",
                schema: "setup",
                table: "FgsSetupPricingMatrixMaterialTier",
                column: "PriceAdjustmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPricingMatrixMaterialTier_TenantId_CompanyId_FgsSetupPricingMatrixId",
                schema: "setup",
                table: "FgsSetupPricingMatrixMaterialTier",
                columns: new[] { "TenantId", "CompanyId", "FgsSetupPricingMatrixId" });

            migrationBuilder.CreateIndex(
                name: "UQ_FgsSetupPricingMatrixMaterialTier_Matrix_FromCost",
                schema: "setup",
                table: "FgsSetupPricingMatrixMaterialTier",
                columns: new[] { "TenantId", "CompanyId", "FgsSetupPricingMatrixId", "FromCost" },
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_FgsSetupPricingMatrixMaterialTier_AdjustmentValue",
                schema: "setup",
                table: "FgsSetupPricingMatrixMaterialTier",
                sql: "\"AdjustmentValue\" >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_FgsSetupPricingMatrixMaterialTier_PriceAdjustmentTypeId",
                schema: "setup",
                table: "FgsSetupPricingMatrixMaterialTier",
                sql: "\"PriceAdjustmentTypeId\" BETWEEN 1 AND 3");

            migrationBuilder.CreateIndex(
                name: "IX_SetupOutboxMessage_CorrelationId",
                schema: "setup",
                table: "SetupOutboxMessage",
                column: "CorrelationId");

            migrationBuilder.CreateIndex(
                name: "IX_SetupOutboxMessage_EventType",
                schema: "setup",
                table: "SetupOutboxMessage",
                column: "EventType");

            migrationBuilder.CreateIndex(
                name: "IX_SetupOutboxMessage_Status_NextRetryOn",
                schema: "setup",
                table: "SetupOutboxMessage",
                columns: new[] { "Status", "NextRetryOn" });

            migrationBuilder.CreateIndex(
                name: "IX_SetupOutboxMessage_TenantId",
                schema: "setup",
                table: "SetupOutboxMessage",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SetupOutboxMessage",
                schema: "setup");

            migrationBuilder.DropIndex(
                name: "IX_FgsSetupPricingMatrixMaterialTier_PriceAdjustmentTypeId",
                schema: "setup",
                table: "FgsSetupPricingMatrixMaterialTier");

            migrationBuilder.DropIndex(
                name: "IX_FgsSetupPricingMatrixMaterialTier_TenantId_CompanyId_FgsSetupPricingMatrixId",
                schema: "setup",
                table: "FgsSetupPricingMatrixMaterialTier");

            migrationBuilder.DropIndex(
                name: "UQ_FgsSetupPricingMatrixMaterialTier_Matrix_FromCost",
                schema: "setup",
                table: "FgsSetupPricingMatrixMaterialTier");

            migrationBuilder.DropCheckConstraint(
                name: "CK_FgsSetupPricingMatrixMaterialTier_AdjustmentValue",
                schema: "setup",
                table: "FgsSetupPricingMatrixMaterialTier");

            migrationBuilder.DropCheckConstraint(
                name: "CK_FgsSetupPricingMatrixMaterialTier_PriceAdjustmentTypeId",
                schema: "setup",
                table: "FgsSetupPricingMatrixMaterialTier");

            migrationBuilder.DropColumn(
                name: "AdjustmentValue",
                schema: "setup",
                table: "FgsSetupPricingMatrixMaterialTier");

            migrationBuilder.DropColumn(
                name: "PriceAdjustmentTypeId",
                schema: "setup",
                table: "FgsSetupPricingMatrixMaterialTier");

            migrationBuilder.AlterTable(
                name: "FgsSetupPricingMatrixMaterialTier",
                schema: "setup",
                oldComment: "Defines material cost tiers and pricing adjustments used by a pricing matrix. Each tier applies a single pricing adjustment method to determine the selling price from material cost.");

            migrationBuilder.AlterColumn<decimal>(
                name: "ToCost",
                schema: "setup",
                table: "FgsSetupPricingMatrixMaterialTier",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true,
                oldComment: "Inclusive maximum material cost for this pricing tier. NULL indicates no upper limit.");

            migrationBuilder.AlterColumn<decimal>(
                name: "FromCost",
                schema: "setup",
                table: "FgsSetupPricingMatrixMaterialTier",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldComment: "Inclusive minimum material cost for this pricing tier.");

            migrationBuilder.AlterColumn<Guid>(
                name: "FgsSetupPricingMatrixId",
                schema: "setup",
                table: "FgsSetupPricingMatrixMaterialTier",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "Reference to the pricing matrix that contains this tier.");

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountPercent",
                schema: "setup",
                table: "FgsSetupPricingMatrixMaterialTier",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MarkupPercent",
                schema: "setup",
                table: "FgsSetupPricingMatrixMaterialTier",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPricingMatrixMaterialTier_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupPricingMatrixMaterialTier",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.AddCheckConstraint(
                name: "CK_FgsSetupPricingMatrixMaterialTier_DiscountPercent",
                schema: "setup",
                table: "FgsSetupPricingMatrixMaterialTier",
                sql: "\"DiscountPercent\" IS NULL OR (\"DiscountPercent\" >= 0 AND \"DiscountPercent\" <= 100)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_FgsSetupPricingMatrixMaterialTier_MarkupPercent",
                schema: "setup",
                table: "FgsSetupPricingMatrixMaterialTier",
                sql: "\"MarkupPercent\" >= 0");
        }
    }
}
