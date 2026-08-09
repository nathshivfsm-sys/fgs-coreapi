using System;
using Fgs.Audit.Domain.Enums;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Audit.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddFgsEventTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:audit.event_detail_type", "FIELD_CHANGE,CALCULATION,VALIDATION,WORKFLOW,INTEGRATION,MESSAGE,EXCEPTION,NOTE")
                .Annotation("Npgsql:Enum:audit.event_source", "WEB,MOBILE,PORTAL,API,IMPORT,EXPORT,WORKER,SCHEDULER,RABBITMQ,QBO,EMAIL,SMS,SYSTEM")
                .Annotation("Npgsql:Enum:audit.record_type", "CUSTOMER,CONTACT,LOCATION,CALL,WORK_ORDER,APPOINTMENT,ESTIMATE,INVOICE,PAYMENT,ASSET,CONTRACT,INVENTORY_ITEM,PURCHASE_ORDER,TECHNICIAN,TASK,USER,PRICEBOOK,JOB_TYPE,ATTACHMENT,NOTE,SYSTEM");

            migrationBuilder.CreateTable(
                name: "FgsArchiveCatalog",
                schema: "audit",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Unique identifier of the archive record.")
                        .Annotation("Npgsql:IdentitySequenceOptions", "'1', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ArchiveMonth = table.Column<DateOnly>(type: "date", nullable: false, comment: "First day of the month represented by the archived partition (for example, 2026-07-01)."),
                    StoragePath = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false, comment: "Object key or path where the archive file is stored."),
                    FileSize = table.Column<long>(type: "bigint", nullable: false, comment: "Size of the archive file in bytes."),
                    CreatedOn = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()", comment: "Date and time the archive record was created.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsArchiveCatalog", x => x.Id);
                },
                comment: "Maintains an inventory of archived event partitions.");

            migrationBuilder.CreateTable(
                name: "FgsEvent",
                schema: "audit",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Unique identifier of the event.")
                        .Annotation("Npgsql:IdentitySequenceOptions", "'1', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant that owns the event."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company within the tenant that owns the event."),
                    EventCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Application-defined event code identifying the event."),
                    EventSource = table.Column<AuditEventSource>(type: "audit.event_source", nullable: false, comment: "Application, service, integration, or process that generated the event."),
                    RecordType = table.Column<AuditRecordType>(type: "audit.record_type", nullable: false, comment: "Business entity associated with the event."),
                    EntityId = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key of the associated business entity."),
                    EntityNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, comment: "Business reference number displayed to users, such as WO-100245 or INV-10234."),
                    UserName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true, comment: "Display name of the user responsible for the event at the time it occurred."),
                    Summary = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false, comment: "Short human-readable description displayed in the event history."),
                    OccurredOn = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()", comment: "Date and time the event occurred. Used for partitioning."),
                    CreatedOn = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()", comment: "Date and time the event record was written to the audit database.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsEvent", x => x.Id);
                },
                comment: "Stores business, system, security, and integration events generated by the FSM platform. Provides the primary audit history for all business entities.");

            migrationBuilder.CreateTable(
                name: "FgsEventAttachment",
                schema: "audit",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Unique identifier of the event attachment.")
                        .Annotation("Npgsql:IdentitySequenceOptions", "'1', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EventId = table.Column<long>(type: "bigint", nullable: false, comment: "References the audit event associated with the document."),
                    DocumentId = table.Column<long>(type: "bigint", nullable: false, comment: "References the associated document managed by the Document Service."),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Optional description explaining why the document is associated with the event."),
                    CreatedOn = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()", comment: "Date and time the attachment association was created.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsEventAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsEventAttachment_Event",
                        column: x => x.EventId,
                        principalSchema: "audit",
                        principalTable: "FgsEvent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Associates documents with audit events. Document metadata and storage are managed by the Document Service.");

            migrationBuilder.CreateTable(
                name: "FgsEventDetail",
                schema: "audit",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Unique identifier of the event detail record.")
                        .Annotation("Npgsql:IdentitySequenceOptions", "'1', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EventId = table.Column<long>(type: "bigint", nullable: false, comment: "References the parent event."),
                    EntryType = table.Column<AuditEventDetailType>(type: "audit.event_detail_type", nullable: false, comment: "Classifies the type of detail entry, such as field change, calculation, validation, message, or exception."),
                    Sequence = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Determines the display order of detail entries within an event."),
                    ItemName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Name of the property, calculation item, validation rule, or message attribute."),
                    OldValue = table.Column<string>(type: "text", nullable: true, comment: "Original value before the event occurred. Typically populated for field changes."),
                    NewValue = table.Column<string>(type: "text", nullable: true, comment: "New value after the event occurred, or the resulting value for calculations, messages, and other detail types."),
                    CreatedOn = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()", comment: "Date and time the detail record was created.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsEventDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsEventDetail_Event",
                        column: x => x.EventId,
                        principalSchema: "audit",
                        principalTable: "FgsEvent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Stores detailed information associated with an event, including field changes, calculations, validation results, workflow actions, messages, and exceptions.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsArchiveCatalog_ArchiveMonth",
                schema: "audit",
                table: "FgsArchiveCatalog",
                column: "ArchiveMonth",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsEvent_Entity",
                schema: "audit",
                table: "FgsEvent",
                columns: new[] { "RecordType", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEvent_EventCode",
                schema: "audit",
                table: "FgsEvent",
                column: "EventCode");

            migrationBuilder.CreateIndex(
                name: "IX_FgsEvent_EventSource",
                schema: "audit",
                table: "FgsEvent",
                column: "EventSource");

            migrationBuilder.CreateIndex(
                name: "IX_FgsEvent_OccurredOn",
                schema: "audit",
                table: "FgsEvent",
                column: "OccurredOn",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_FgsEvent_TenantCompanyOccurredOn",
                schema: "audit",
                table: "FgsEvent",
                columns: new[] { "TenantId", "CompanyId", "OccurredOn" },
                descending: new[] { false, false, true });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEventAttachment_DocumentId",
                schema: "audit",
                table: "FgsEventAttachment",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsEventAttachment_EventId",
                schema: "audit",
                table: "FgsEventAttachment",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsEventDetail_EntryType",
                schema: "audit",
                table: "FgsEventDetail",
                column: "EntryType");

            migrationBuilder.CreateIndex(
                name: "IX_FgsEventDetail_EventId",
                schema: "audit",
                table: "FgsEventDetail",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsEventDetail_EventId_Sequence",
                schema: "audit",
                table: "FgsEventDetail",
                columns: new[] { "EventId", "Sequence" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsArchiveCatalog",
                schema: "audit");

            migrationBuilder.DropTable(
                name: "FgsEventAttachment",
                schema: "audit");

            migrationBuilder.DropTable(
                name: "FgsEventDetail",
                schema: "audit");

            migrationBuilder.DropTable(
                name: "FgsEvent",
                schema: "audit");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:Enum:audit.event_detail_type", "FIELD_CHANGE,CALCULATION,VALIDATION,WORKFLOW,INTEGRATION,MESSAGE,EXCEPTION,NOTE")
                .OldAnnotation("Npgsql:Enum:audit.event_source", "WEB,MOBILE,PORTAL,API,IMPORT,EXPORT,WORKER,SCHEDULER,RABBITMQ,QBO,EMAIL,SMS,SYSTEM")
                .OldAnnotation("Npgsql:Enum:audit.record_type", "CUSTOMER,CONTACT,LOCATION,CALL,WORK_ORDER,APPOINTMENT,ESTIMATE,INVOICE,PAYMENT,ASSET,CONTRACT,INVENTORY_ITEM,PURCHASE_ORDER,TECHNICIAN,TASK,USER,PRICEBOOK,JOB_TYPE,ATTACHMENT,NOTE,SYSTEM");
        }
    }
}
