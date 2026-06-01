using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.User.Infrastructure.Persistence.Database.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceCredentialModelWithKmsEnvelopeEncryption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FgsCredentialAudit_CredentialSecret",
                schema: "audit",
                table: "FgsCredentialAudit");

            migrationBuilder.DropTable(
                name: "FgsCredentialProviderConfiguration",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsCredentialSecret",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "GloCredentialCategory",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "FgsCredentialProvider",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "GloCredentialProviderType",
                schema: "glo");

            migrationBuilder.RenameColumn(
                name: "CredentialSecretId",
                schema: "audit",
                table: "FgsCredentialAudit",
                newName: "CredentialId");

            migrationBuilder.RenameIndex(
                name: "IX_FgsCredentialAudit_CredentialSecretId",
                schema: "audit",
                table: "FgsCredentialAudit",
                newName: "IX_FgsCredentialAudit_CredentialId");

            migrationBuilder.CreateTable(
                name: "GloCredentialProviderType",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    ProviderCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "System unique code used by application logic and integration services."),
                    ProviderName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "User friendly provider name displayed in setup screens."),
                    ConfigurationSchema = table.Column<string>(type: "jsonb", nullable: false, comment: "JSON schema used by the UI to dynamically render provider configuration fields and perform validation."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the provider can be selected for new credential configurations."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloCredentialProviderType", x => x.Id);
                },
                comment: "Master list of supported credential providers and integrations available within the FSM platform.");

            migrationBuilder.CreateTable(
                name: "FgsCredential",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant that owns the credential."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company that owns the credential."),
                    CredentialProviderTypeId = table.Column<int>(type: "integer", nullable: false, comment: "Credential provider associated with this credential."),
                    CredentialName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "User friendly name displayed in tenant administration screens."),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Optional description of the credential usage."),
                    CredentialData = table.Column<byte[]>(type: "bytea", nullable: false, comment: "Provider credential JSON encrypted using a Data Encryption Key (DEK)."),
                    EncryptedDataKey = table.Column<byte[]>(type: "bytea", nullable: false, comment: "Data Encryption Key encrypted using AWS KMS."),
                    KeyIdentifier = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "AWS KMS key ARN or alias used to encrypt the Data Encryption Key."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the credential is active and available for use."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsCredential", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsCredential_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "tenant",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsCredential_GloCredentialProviderType",
                        column: x => x.CredentialProviderTypeId,
                        principalSchema: "glo",
                        principalTable: "GloCredentialProviderType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores tenant-owned credentials encrypted using AWS KMS envelope encryption.");

            migrationBuilder.CreateTable(
                name: "GloCredential",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    CredentialProviderTypeId = table.Column<int>(type: "integer", nullable: false),
                    CredentialName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CredentialData = table.Column<byte[]>(type: "bytea", nullable: false),
                    EncryptedDataKey = table.Column<byte[]>(type: "bytea", nullable: false),
                    KeyIdentifier = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloCredential", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GloCredential_ProviderType",
                        column: x => x.CredentialProviderTypeId,
                        principalSchema: "glo",
                        principalTable: "GloCredentialProviderType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "UQ_GloCredentialProviderType_ProviderCode",
                schema: "glo",
                table: "GloCredentialProviderType",
                column: "ProviderCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredential_CredentialProviderTypeId",
                schema: "setup",
                table: "FgsCredential",
                column: "CredentialProviderTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredential_Tenant_Company",
                schema: "setup",
                table: "FgsCredential",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredential_Tenant_Company_ProviderType",
                schema: "setup",
                table: "FgsCredential",
                columns: new[] { "TenantId", "CompanyId", "CredentialProviderTypeId" });

            migrationBuilder.CreateIndex(
                name: "UQ_FgsCredential_Tenant_Company_ProviderType",
                schema: "setup",
                table: "FgsCredential",
                columns: new[] { "TenantId", "CompanyId", "CredentialProviderTypeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloCredential_CredentialProviderTypeId",
                schema: "glo",
                table: "GloCredential",
                column: "CredentialProviderTypeId");

            migrationBuilder.Sql("DELETE FROM audit.\"FgsCredentialAudit\";");

            migrationBuilder.AddForeignKey(
                name: "FK_FgsCredentialAudit_Credential",
                schema: "audit",
                table: "FgsCredentialAudit",
                column: "CredentialId",
                principalSchema: "setup",
                principalTable: "FgsCredential",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FgsCredentialAudit_Credential",
                schema: "audit",
                table: "FgsCredentialAudit");

            migrationBuilder.DropTable(
                name: "FgsCredential",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "GloCredential",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "GloCredentialProviderType",
                schema: "glo");

            migrationBuilder.RenameColumn(
                name: "CredentialId",
                schema: "audit",
                table: "FgsCredentialAudit",
                newName: "CredentialSecretId");

            migrationBuilder.RenameIndex(
                name: "IX_FgsCredentialAudit_CredentialId",
                schema: "audit",
                table: "FgsCredentialAudit",
                newName: "IX_FgsCredentialAudit_CredentialSecretId");

            migrationBuilder.CreateTable(
                name: "GloCredentialProviderType",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloCredentialProviderType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GloCredentialProviderType_Code",
                schema: "glo",
                table: "GloCredentialProviderType",
                column: "Code",
                unique: true);

            migrationBuilder.CreateTable(
                name: "FgsCredentialProvider",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CredentialProviderTypeId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Environment = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsCredentialProvider", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsCredentialProvider_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "tenant",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GloCredentialCategory",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloCredentialCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsCredentialProviderConfiguration",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    ConfigurationKey = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ConfigurationValue = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CredentialProviderId = table.Column<Guid>(type: "uuid", nullable: false),
                    Environment = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsCredentialProviderConfiguration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsCredProvCfg_Company",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "tenant",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsCredProvCfg_Provider",
                        column: x => x.CredentialProviderId,
                        principalSchema: "setup",
                        principalTable: "FgsCredentialProvider",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsCredentialSecret",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CredentialProviderId = table.Column<Guid>(type: "uuid", nullable: false),
                    EncryptedDek = table.Column<string>(type: "text", nullable: false),
                    EncryptedSecretValue = table.Column<string>(type: "text", nullable: false),
                    EncryptionKeyId = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ExpiresOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsRevoked = table.Column<bool>(type: "boolean", nullable: false),
                    LastRotatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    SecretName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    VersionNo = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsCredentialSecret", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsCredentialSecret_Company",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "tenant",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsCredentialSecret_Provider",
                        column: x => x.CredentialProviderId,
                        principalSchema: "setup",
                        principalTable: "FgsCredentialProvider",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredentialProvider_TenantId_CompanyId_Code",
                schema: "setup",
                table: "FgsCredentialProvider",
                columns: new[] { "TenantId", "CompanyId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredentialProviderConfiguration_CredentialProviderId",
                schema: "setup",
                table: "FgsCredentialProviderConfiguration",
                column: "CredentialProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredProvCfg_Tenant_Company",
                schema: "setup",
                table: "FgsCredentialProviderConfiguration",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredProvCfg_Tenant_Company_Prov",
                schema: "setup",
                table: "FgsCredentialProviderConfiguration",
                columns: new[] { "TenantId", "CompanyId", "CredentialProviderId" });

            migrationBuilder.CreateIndex(
                name: "UQ_FgsCredentialProviderConfiguration",
                schema: "setup",
                table: "FgsCredentialProviderConfiguration",
                columns: new[] { "TenantId", "CompanyId", "CredentialProviderId", "ConfigurationKey", "Environment" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredentialSecret_CredentialProviderId",
                schema: "setup",
                table: "FgsCredentialSecret",
                column: "CredentialProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredentialSecret_IsActive",
                schema: "setup",
                table: "FgsCredentialSecret",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredentialSecret_Tenant_Company",
                schema: "setup",
                table: "FgsCredentialSecret",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredentialSecret_Tenant_Company_Prov",
                schema: "setup",
                table: "FgsCredentialSecret",
                columns: new[] { "TenantId", "CompanyId", "CredentialProviderId" });

            migrationBuilder.CreateIndex(
                name: "UQ_FgsCredentialSecret",
                schema: "setup",
                table: "FgsCredentialSecret",
                columns: new[] { "TenantId", "CompanyId", "CredentialProviderId", "SecretName", "VersionNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloCredentialCategory_Code",
                schema: "glo",
                table: "GloCredentialCategory",
                column: "Code",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsCredentialAudit_CredentialSecret",
                schema: "audit",
                table: "FgsCredentialAudit",
                column: "CredentialSecretId",
                principalSchema: "setup",
                principalTable: "FgsCredentialSecret",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
