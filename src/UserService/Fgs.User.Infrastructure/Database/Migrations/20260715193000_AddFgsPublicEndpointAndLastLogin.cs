using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.User.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddFgsPublicEndpointAndLastLogin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FgsPublicEndpoint",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Unique identifier for the service endpoint.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant that owns the service endpoint."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company that owns the service endpoint."),
                    EndpointType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Type of public endpoint. Supported values are BFF for the application backend and API for third-party integrations."),
                    EnvironmentCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, comment: "Deployment environment of the endpoint. Supported values are PROD, SANDBOX, TRAINING, QA, PREVIEW and DEVELOPMENT."),
                    BaseUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false, comment: "Base URL clients use to access the public endpoint."),
                    DisplayName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User-friendly name displayed when multiple environments are available."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, comment: "Date and time the service endpoint was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User or system that created the service endpoint."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the service endpoint was last modified."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User or system that last modified the service endpoint."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the endpoint is available for use.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsPublicEndpoint", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsPublicEndpoint_FgsTenantCompanyCache",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "identity",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores public endpoints exposed by the platform for each tenant and company. Used during authentication and by client applications to discover the appropriate application or integration endpoint.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsPublicEndpoint_Tenant_Company",
                schema: "identity",
                table: "FgsPublicEndpoint",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsPublicEndpoint_Tenant_Company_Type_Environment",
                schema: "identity",
                table: "FgsPublicEndpoint",
                columns: new[] { "TenantId", "CompanyId", "EndpointType", "EnvironmentCode" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsPublicEndpoint",
                schema: "identity");
        }
    }
}
