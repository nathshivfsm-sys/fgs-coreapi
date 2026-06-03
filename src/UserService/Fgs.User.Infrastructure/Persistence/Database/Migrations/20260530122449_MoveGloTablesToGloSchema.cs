using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.User.Infrastructure.Persistence.Database.Migrations
{
    /// <inheritdoc />
    public partial class MoveGloTablesToGloSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "GloTimeCardOption",
                schema: "dispatch",
                newName: "GloTimeCardOption",
                newSchema: "glo");

            migrationBuilder.RenameTable(
                name: "GloSeedTableMapping",
                schema: "tenant",
                newName: "GloSeedTableMapping",
                newSchema: "glo");

            migrationBuilder.RenameTable(
                name: "GloSeedTableColumnMapping",
                schema: "tenant",
                newName: "GloSeedTableColumnMapping",
                newSchema: "glo");

            migrationBuilder.RenameTable(
                name: "GloRole",
                schema: "identity",
                newName: "GloRole",
                newSchema: "glo");

            migrationBuilder.RenameTable(
                name: "GloOutboxMessage",
                schema: "shared",
                newName: "GloOutboxMessage",
                newSchema: "glo");

            migrationBuilder.RenameTable(
                name: "GloCredentialProviderType",
                schema: "integration",
                newName: "GloCredentialProviderType",
                newSchema: "glo");

            migrationBuilder.RenameTable(
                name: "GloCredentialCategory",
                schema: "integration",
                newName: "GloCredentialCategory",
                newSchema: "glo");

            migrationBuilder.RenameTable(
                name: "GloCommunicationToken",
                schema: "notification",
                newName: "GloCommunicationToken",
                newSchema: "glo");

            migrationBuilder.RenameTable(
                name: "GloAccountingIntegrationType",
                schema: "integration",
                newName: "GloAccountingIntegrationType",
                newSchema: "glo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "GloTimeCardOption",
                schema: "glo",
                newName: "GloTimeCardOption",
                newSchema: "dispatch");

            migrationBuilder.RenameTable(
                name: "GloSeedTableMapping",
                schema: "glo",
                newName: "GloSeedTableMapping",
                newSchema: "tenant");

            migrationBuilder.RenameTable(
                name: "GloSeedTableColumnMapping",
                schema: "glo",
                newName: "GloSeedTableColumnMapping",
                newSchema: "tenant");

            migrationBuilder.RenameTable(
                name: "GloRole",
                schema: "glo",
                newName: "GloRole",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "GloOutboxMessage",
                schema: "glo",
                newName: "GloOutboxMessage",
                newSchema: "shared");

            migrationBuilder.RenameTable(
                name: "GloCredentialProviderType",
                schema: "glo",
                newName: "GloCredentialProviderType",
                newSchema: "integration");

            migrationBuilder.RenameTable(
                name: "GloCredentialCategory",
                schema: "glo",
                newName: "GloCredentialCategory",
                newSchema: "integration");

            migrationBuilder.RenameTable(
                name: "GloCommunicationToken",
                schema: "glo",
                newName: "GloCommunicationToken",
                newSchema: "notification");

            migrationBuilder.RenameTable(
                name: "GloAccountingIntegrationType",
                schema: "glo",
                newName: "GloAccountingIntegrationType",
                newSchema: "integration");
        }
    }
}
