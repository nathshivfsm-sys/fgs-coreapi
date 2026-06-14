DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'dispatch') THEN
        CREATE SCHEMA dispatch;
    END IF;
END $EF$;
CREATE TABLE IF NOT EXISTS dispatch."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260603213051_InitialSchema') THEN
    INSERT INTO dispatch."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260603213051_InitialSchema', '10.0.8');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260604132313_AddFgsTenantCompanyCache') THEN
        IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'dispatch') THEN
            CREATE SCHEMA dispatch;
        END IF;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260604132313_AddFgsTenantCompanyCache') THEN
    CREATE TABLE dispatch."FgsTenantCompanyCache" (
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "CompanyGuid" uuid NOT NULL,
        "CompanyCode" character varying(100) NOT NULL,
        "CompanyName" character varying(200) NOT NULL,
        "IsActive" boolean NOT NULL,
        "UpdatedOn" timestamptz,
        CONSTRAINT "PK_FgsTenantCompanyCache" PRIMARY KEY ("TenantId", "CompanyId")
    );
    COMMENT ON TABLE dispatch."FgsTenantCompanyCache" IS 'Local cache of tenant company information used by the Dispatch schema to eliminate cross-schema dependencies on tenant.FgsTenantCompany.';
    COMMENT ON COLUMN dispatch."FgsTenantCompanyCache"."TenantId" IS 'Tenant identifier.';
    COMMENT ON COLUMN dispatch."FgsTenantCompanyCache"."CompanyId" IS 'Company identifier mapped from tenant.FgsTenantCompany.CompanyNumber.';
    COMMENT ON COLUMN dispatch."FgsTenantCompanyCache"."CompanyGuid" IS 'Globally unique company identifier used by integrations and external systems.';
    COMMENT ON COLUMN dispatch."FgsTenantCompanyCache"."CompanyCode" IS 'Unique company code within a tenant.';
    COMMENT ON COLUMN dispatch."FgsTenantCompanyCache"."CompanyName" IS 'Display name of the company.';
    COMMENT ON COLUMN dispatch."FgsTenantCompanyCache"."IsActive" IS 'Indicates whether the company is active.';
    COMMENT ON COLUMN dispatch."FgsTenantCompanyCache"."UpdatedOn" IS 'Timestamp of the most recent synchronization from tenant.FgsTenantCompany.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260604132313_AddFgsTenantCompanyCache') THEN
    CREATE INDEX "IX_FgsTenantCompanyCache_CompanyName" ON dispatch."FgsTenantCompanyCache" ("CompanyName");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260604132313_AddFgsTenantCompanyCache') THEN
    CREATE INDEX "IX_FgsTenantCompanyCache_IsActive" ON dispatch."FgsTenantCompanyCache" ("IsActive");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260604132313_AddFgsTenantCompanyCache') THEN
    CREATE UNIQUE INDEX "UQ_FgsTenantCompanyCache_CompanyGuid" ON dispatch."FgsTenantCompanyCache" ("CompanyGuid");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260604132313_AddFgsTenantCompanyCache') THEN
    INSERT INTO dispatch."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260604132313_AddFgsTenantCompanyCache', '10.0.8');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE TABLE dispatch."FgsCrew" (
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "Id" bigint GENERATED ALWAYS AS IDENTITY,
        "CrewCode" character varying(25) NOT NULL,
        "CrewName" character varying(100) NOT NULL,
        "Description" character varying(500),
        "IsActive" boolean NOT NULL DEFAULT TRUE,
        "CreatedOn" timestamptz NOT NULL DEFAULT (now()),
        "CreatedBy" bigint NOT NULL,
        "UpdatedOn" timestamptz,
        "UpdatedBy" bigint,
        CONSTRAINT "PK_FgsCrew" PRIMARY KEY ("Id"),
        CONSTRAINT "UX_FgsCrew_TenantCompany_Id" UNIQUE ("TenantId", "CompanyId", "Id"),
        CONSTRAINT "FK_FgsCrew_FgsTenantCompanyCache" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dispatch."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT
    );
    COMMENT ON TABLE dispatch."FgsCrew" IS 'Represents a technician crew used for scheduling, dispatching and workload management.';
    COMMENT ON COLUMN dispatch."FgsCrew"."TenantId" IS 'Tenant identifier.';
    COMMENT ON COLUMN dispatch."FgsCrew"."CompanyId" IS 'Company identifier.';
    COMMENT ON COLUMN dispatch."FgsCrew"."Id" IS 'Primary key.';
    COMMENT ON COLUMN dispatch."FgsCrew"."CrewCode" IS 'Short unique crew code used on dispatch boards, reports and integrations.';
    COMMENT ON COLUMN dispatch."FgsCrew"."CrewName" IS 'Display name of the crew.';
    COMMENT ON COLUMN dispatch."FgsCrew"."Description" IS 'Optional crew description.';
    COMMENT ON COLUMN dispatch."FgsCrew"."IsActive" IS 'Indicates whether the crew is available for scheduling and dispatching.';
    COMMENT ON COLUMN dispatch."FgsCrew"."CreatedOn" IS 'Date and time the record was created.';
    COMMENT ON COLUMN dispatch."FgsCrew"."CreatedBy" IS 'User who created the record.';
    COMMENT ON COLUMN dispatch."FgsCrew"."UpdatedOn" IS 'Date and time the record was last updated.';
    COMMENT ON COLUMN dispatch."FgsCrew"."UpdatedBy" IS 'User who last updated the record.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE TABLE dispatch."FgsPayrollPayPeriod" (
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "Id" bigint GENERATED ALWAYS AS IDENTITY,
        "PayPeriodCode" character varying(20) NOT NULL,
        "PeriodStartDate" date NOT NULL,
        "PeriodEndDate" date NOT NULL,
        "PayrollStatusId" smallint NOT NULL DEFAULT 1,
        "CalculatedOn" timestamptz,
        "CalculatedBy" bigint,
        "ApprovedOn" timestamptz,
        "ApprovedBy" bigint,
        "ExportedOn" timestamptz,
        "ExportedBy" bigint,
        "ExportReference" character varying(100),
        "CreatedOn" timestamptz NOT NULL DEFAULT (now()),
        "CreatedBy" bigint NOT NULL,
        "UpdatedOn" timestamptz,
        "UpdatedBy" bigint,
        CONSTRAINT "PK_FgsPayrollPayPeriod" PRIMARY KEY ("Id"),
        CONSTRAINT "CK_FgsPayrollPayPeriod_Approved" CHECK (("ApprovedOn" IS NULL AND "ApprovedBy" IS NULL) OR ("ApprovedOn" IS NOT NULL AND "ApprovedBy" IS NOT NULL)),
        CONSTRAINT "CK_FgsPayrollPayPeriod_Calculated" CHECK (("CalculatedOn" IS NULL AND "CalculatedBy" IS NULL) OR ("CalculatedOn" IS NOT NULL AND "CalculatedBy" IS NOT NULL)),
        CONSTRAINT "CK_FgsPayrollPayPeriod_DateRange" CHECK ("PeriodEndDate" >= "PeriodStartDate"),
        CONSTRAINT "CK_FgsPayrollPayPeriod_Exported" CHECK (("ExportedOn" IS NULL AND "ExportedBy" IS NULL) OR ("ExportedOn" IS NOT NULL AND "ExportedBy" IS NOT NULL)),
        CONSTRAINT "CK_FgsPayrollPayPeriod_Status" CHECK ("PayrollStatusId" IN (1, 2, 3, 4)),
        CONSTRAINT "FK_FgsPayrollPayPeriod_FgsTenantCompanyCache" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dispatch."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT
    );
    COMMENT ON TABLE dispatch."FgsPayrollPayPeriod" IS 'Defines payroll processing periods used to calculate, approve and export payroll.';
    COMMENT ON COLUMN dispatch."FgsPayrollPayPeriod"."TenantId" IS 'Tenant identifier.';
    COMMENT ON COLUMN dispatch."FgsPayrollPayPeriod"."CompanyId" IS 'Company identifier.';
    COMMENT ON COLUMN dispatch."FgsPayrollPayPeriod"."Id" IS 'Primary key.';
    COMMENT ON COLUMN dispatch."FgsPayrollPayPeriod"."PayPeriodCode" IS 'Human-readable payroll period code such as 2026-PP12, 2026-06A or 2026-06B.';
    COMMENT ON COLUMN dispatch."FgsPayrollPayPeriod"."PeriodStartDate" IS 'Inclusive payroll period start date.';
    COMMENT ON COLUMN dispatch."FgsPayrollPayPeriod"."PeriodEndDate" IS 'Inclusive payroll period end date.';
    COMMENT ON COLUMN dispatch."FgsPayrollPayPeriod"."PayrollStatusId" IS 'Payroll status. 1=Open, 2=Calculated, 3=Approved, 4=Exported.';
    COMMENT ON COLUMN dispatch."FgsPayrollPayPeriod"."CalculatedOn" IS 'Date and time payroll calculations were generated.';
    COMMENT ON COLUMN dispatch."FgsPayrollPayPeriod"."CalculatedBy" IS 'User who generated payroll calculations.';
    COMMENT ON COLUMN dispatch."FgsPayrollPayPeriod"."ApprovedOn" IS 'Date and time payroll was approved.';
    COMMENT ON COLUMN dispatch."FgsPayrollPayPeriod"."ApprovedBy" IS 'User who approved payroll.';
    COMMENT ON COLUMN dispatch."FgsPayrollPayPeriod"."ExportedOn" IS 'Date and time payroll was exported.';
    COMMENT ON COLUMN dispatch."FgsPayrollPayPeriod"."ExportedBy" IS 'User who exported payroll.';
    COMMENT ON COLUMN dispatch."FgsPayrollPayPeriod"."ExportReference" IS 'Optional external payroll batch number, export file identifier or payroll provider reference.';
    COMMENT ON COLUMN dispatch."FgsPayrollPayPeriod"."CreatedOn" IS 'Date and time the record was created.';
    COMMENT ON COLUMN dispatch."FgsPayrollPayPeriod"."CreatedBy" IS 'User who created the record.';
    COMMENT ON COLUMN dispatch."FgsPayrollPayPeriod"."UpdatedOn" IS 'Date and time the record was last updated.';
    COMMENT ON COLUMN dispatch."FgsPayrollPayPeriod"."UpdatedBy" IS 'User who last updated the record.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE TABLE dispatch."FgsWorkOrder" (
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "WorkOrderNumber" character varying(50) NOT NULL,
        "ProjectId" bigint,
        "CustomerId" bigint NOT NULL,
        "LocationId" bigint NOT NULL,
        "ServiceAgreementId" bigint,
        "ServiceAgreementVisitId" bigint,
        "Break1Id" bigint,
        "Break2Id" bigint,
        "JobTypeId" bigint NOT NULL,
        "PriorityId" bigint NOT NULL,
        "WorkOrderStatusId" bigint NOT NULL,
        "WorkOrderResolutionId" bigint,
        "TimeSlotId" bigint,
        "CustomerPO" character varying(100),
        "PersonCalling" character varying(200),
        "PersonCallingPhoneNumber" character varying(30),
        "ContactPerson" character varying(200),
        "ContactPersonPhoneNumber" character varying(30),
        "ProblemDescription" text,
        "Note" text,
        "MaterialPricingMatrixId" bigint,
        "LaborPricingMatrixId" bigint,
        "OtherPricingMatrixId" bigint,
        "PaymentMethodId" bigint,
        "EstimatedHours" numeric(8,2),
        "RequestedOn" timestamptz NOT NULL,
        "StartDate" timestamptz,
        "EndDate" timestamptz,
        "Source" character varying(50),
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" character varying(100),
        "UpdatedOn" timestamptz,
        "UpdatedBy" character varying(100),
        CONSTRAINT "PK_FgsWorkOrder" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_FgsWorkOrder_TenantCompany" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dispatch."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT
    );
    COMMENT ON TABLE dispatch."FgsWorkOrder" IS 'Master work order record representing a customer service request that can be scheduled through one or more appointments.';
    COMMENT ON COLUMN dispatch."FgsWorkOrder"."TenantId" IS 'Tenant identifier.';
    COMMENT ON COLUMN dispatch."FgsWorkOrder"."CompanyId" IS 'Company identifier.';
    COMMENT ON COLUMN dispatch."FgsWorkOrder"."Id" IS 'Primary key.';
    COMMENT ON COLUMN dispatch."FgsWorkOrder"."WorkOrderNumber" IS 'Unique work order number within tenant and company.';
    COMMENT ON COLUMN dispatch."FgsWorkOrder"."ProjectId" IS 'Optional project identifier. References project service; no FK by design.';
    COMMENT ON COLUMN dispatch."FgsWorkOrder"."CustomerId" IS 'Customer identifier. References CRM service; no FK by design.';
    COMMENT ON COLUMN dispatch."FgsWorkOrder"."LocationId" IS 'Service location identifier. References CRM service; no FK by design.';
    COMMENT ON COLUMN dispatch."FgsWorkOrder"."ServiceAgreementId" IS 'Service agreement identifier. References service agreement service; no FK by design.';
    COMMENT ON COLUMN dispatch."FgsWorkOrder"."ServiceAgreementVisitId" IS 'Service agreement visit identifier. References service agreement service; no FK by design.';
    COMMENT ON COLUMN dispatch."FgsWorkOrder"."Break1Id" IS 'Primary break classification identifier. References setup service; no FK by design.';
    COMMENT ON COLUMN dispatch."FgsWorkOrder"."Break2Id" IS 'Secondary break classification identifier. References setup service; no FK by design.';
    COMMENT ON COLUMN dispatch."FgsWorkOrder"."JobTypeId" IS 'Job type identifier. References setup.FgsJobType through application logic; no FK by design.';
    COMMENT ON COLUMN dispatch."FgsWorkOrder"."PriorityId" IS 'Priority identifier. References setup service; no FK by design.';
    COMMENT ON COLUMN dispatch."FgsWorkOrder"."WorkOrderStatusId" IS 'Work order status. New, Started, Completed, or Cancelled.';
    COMMENT ON COLUMN dispatch."FgsWorkOrder"."WorkOrderResolutionId" IS 'Completion or cancellation reason identifier. References setup service; no FK by design.';
    COMMENT ON COLUMN dispatch."FgsWorkOrder"."TimeSlotId" IS 'Promised time window. References setup.FgsSetupTimeSlot through application logic; no FK by design.';
    COMMENT ON COLUMN dispatch."FgsWorkOrder"."CustomerPO" IS 'Customer purchase order reference.';
    COMMENT ON COLUMN dispatch."FgsWorkOrder"."PersonCalling" IS 'Name of person who called to request service.';
    COMMENT ON COLUMN dispatch."FgsWorkOrder"."PersonCallingPhoneNumber" IS 'Phone number of person who called.';
    COMMENT ON COLUMN dispatch."FgsWorkOrder"."ContactPerson" IS 'Onsite contact person name.';
    COMMENT ON COLUMN dispatch."FgsWorkOrder"."ContactPersonPhoneNumber" IS 'Onsite contact person phone number.';
    COMMENT ON COLUMN dispatch."FgsWorkOrder"."ProblemDescription" IS 'Customer problem description.';
    COMMENT ON COLUMN dispatch."FgsWorkOrder"."Note" IS 'Special instructions for technicians.';
    COMMENT ON COLUMN dispatch."FgsWorkOrder"."MaterialPricingMatrixId" IS 'Material pricing matrix identifier. References setup service; no FK by design.';
    COMMENT ON COLUMN dispatch."FgsWorkOrder"."LaborPricingMatrixId" IS 'Labor pricing matrix identifier. References setup service; no FK by design.';
    COMMENT ON COLUMN dispatch."FgsWorkOrder"."OtherPricingMatrixId" IS 'Other pricing matrix identifier. References setup service; no FK by design.';
    COMMENT ON COLUMN dispatch."FgsWorkOrder"."PaymentMethodId" IS 'Payment method identifier. References setup service; no FK by design.';
    COMMENT ON COLUMN dispatch."FgsWorkOrder"."EstimatedHours" IS 'Estimated hours for the work order.';
    COMMENT ON COLUMN dispatch."FgsWorkOrder"."RequestedOn" IS 'Date and time the work order was requested.';
    COMMENT ON COLUMN dispatch."FgsWorkOrder"."StartDate" IS 'Work order start date and time.';
    COMMENT ON COLUMN dispatch."FgsWorkOrder"."EndDate" IS 'Work order end date and time.';
    COMMENT ON COLUMN dispatch."FgsWorkOrder"."Source" IS 'Source of the work order such as Manual, Portal, API, Corrigo, ServiceChannel, Verizon, AHS, etc.';
    COMMENT ON COLUMN dispatch."FgsWorkOrder"."CreatedOn" IS 'Date and time the record was created.';
    COMMENT ON COLUMN dispatch."FgsWorkOrder"."CreatedBy" IS 'User who created the record.';
    COMMENT ON COLUMN dispatch."FgsWorkOrder"."UpdatedOn" IS 'Date and time the record was last updated.';
    COMMENT ON COLUMN dispatch."FgsWorkOrder"."UpdatedBy" IS 'User who last updated the record.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE TABLE dispatch."FgsAppointment" (
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "SourceTypeId" smallint NOT NULL,
        "SourceId" bigint NOT NULL,
        "CrewId" bigint,
        "CustomerContactName" character varying(200),
        "ServiceDate" date NOT NULL,
        "ScheduledTime" time NOT NULL,
        "EstimatedHours" numeric(8,2) NOT NULL,
        "AppointmentStatusId" smallint NOT NULL,
        "CustomerApprovedOn" timestamptz,
        "CreatedOn" timestamptz NOT NULL DEFAULT (now()),
        "CreatedBy" bigint NOT NULL,
        "UpdatedOn" timestamptz,
        "UpdatedBy" bigint,
        CONSTRAINT "PK_FgsAppointment" PRIMARY KEY ("Id"),
        CONSTRAINT "CK_FgsAppointment_EstimatedHours" CHECK ("EstimatedHours" > 0),
        CONSTRAINT "CK_FgsAppointment_Status" CHECK ("AppointmentStatusId" IN (1, 2, 3)),
        CONSTRAINT "FK_FgsAppointment_Crew" FOREIGN KEY ("TenantId", "CompanyId", "CrewId") REFERENCES dispatch."FgsCrew" ("TenantId", "CompanyId", "Id") ON DELETE RESTRICT,
        CONSTRAINT "FK_FgsAppointment_TenantCompany" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dispatch."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT
    );
    COMMENT ON TABLE dispatch."FgsAppointment" IS 'Represents a scheduled customer visit for a lead, opportunity or work order.';
    COMMENT ON COLUMN dispatch."FgsAppointment"."TenantId" IS 'Tenant identifier.';
    COMMENT ON COLUMN dispatch."FgsAppointment"."CompanyId" IS 'Company identifier.';
    COMMENT ON COLUMN dispatch."FgsAppointment"."Id" IS 'Primary key.';
    COMMENT ON COLUMN dispatch."FgsAppointment"."SourceTypeId" IS 'Source type. Typically Lead, Opportunity or Work Order.';
    COMMENT ON COLUMN dispatch."FgsAppointment"."SourceId" IS 'Identifier of the source record.';
    COMMENT ON COLUMN dispatch."FgsAppointment"."CrewId" IS 'Scheduled crew assigned to the appointment.';
    COMMENT ON COLUMN dispatch."FgsAppointment"."CustomerContactName" IS 'Contact name used for appointment reminders and confirmations.';
    COMMENT ON COLUMN dispatch."FgsAppointment"."ServiceDate" IS 'Customer promised service date.';
    COMMENT ON COLUMN dispatch."FgsAppointment"."ScheduledTime" IS 'Customer promised local appointment time.';
    COMMENT ON COLUMN dispatch."FgsAppointment"."EstimatedHours" IS 'Estimated appointment duration used for scheduling and dispatch planning.';
    COMMENT ON COLUMN dispatch."FgsAppointment"."AppointmentStatusId" IS 'Appointment status. 1=Unassigned, 2=Open, 3=Completed.';
    COMMENT ON COLUMN dispatch."FgsAppointment"."CustomerApprovedOn" IS 'Date and time customer approved the appointment visit.';
    COMMENT ON COLUMN dispatch."FgsAppointment"."CreatedOn" IS 'Date and time the record was created.';
    COMMENT ON COLUMN dispatch."FgsAppointment"."CreatedBy" IS 'User who created the record.';
    COMMENT ON COLUMN dispatch."FgsAppointment"."UpdatedOn" IS 'Date and time the record was last updated.';
    COMMENT ON COLUMN dispatch."FgsAppointment"."UpdatedBy" IS 'User who last updated the record.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE TABLE dispatch."FgsCrewMember" (
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "Id" bigint GENERATED ALWAYS AS IDENTITY,
        "CrewId" bigint NOT NULL,
        "EmployeeId" bigint NOT NULL,
        "IsLead" boolean NOT NULL DEFAULT FALSE,
        "CreatedOn" timestamptz NOT NULL DEFAULT (now()),
        "CreatedBy" bigint NOT NULL,
        "UpdatedOn" timestamptz,
        "UpdatedBy" bigint,
        CONSTRAINT "PK_FgsCrewMember" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_FgsCrewMember_FgsCrew" FOREIGN KEY ("CrewId") REFERENCES dispatch."FgsCrew" ("Id") ON DELETE RESTRICT,
        CONSTRAINT "FK_FgsCrewMember_FgsTenantCompanyCache" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dispatch."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT
    );
    COMMENT ON TABLE dispatch."FgsCrewMember" IS 'Stores technician membership within a crew.';
    COMMENT ON COLUMN dispatch."FgsCrewMember"."TenantId" IS 'Tenant identifier.';
    COMMENT ON COLUMN dispatch."FgsCrewMember"."CompanyId" IS 'Company identifier.';
    COMMENT ON COLUMN dispatch."FgsCrewMember"."Id" IS 'Primary key.';
    COMMENT ON COLUMN dispatch."FgsCrewMember"."CrewId" IS 'Crew associated with the technician.';
    COMMENT ON COLUMN dispatch."FgsCrewMember"."EmployeeId" IS 'Employee assigned to the crew. References user service; no FK by design.';
    COMMENT ON COLUMN dispatch."FgsCrewMember"."IsLead" IS 'Indicates whether the employee is the lead technician or foreman for the crew.';
    COMMENT ON COLUMN dispatch."FgsCrewMember"."CreatedOn" IS 'Date and time the record was created.';
    COMMENT ON COLUMN dispatch."FgsCrewMember"."CreatedBy" IS 'User who created the record.';
    COMMENT ON COLUMN dispatch."FgsCrewMember"."UpdatedOn" IS 'Date and time the record was last updated.';
    COMMENT ON COLUMN dispatch."FgsCrewMember"."UpdatedBy" IS 'User who last updated the record.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE TABLE dispatch."FgsPayroll" (
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "Id" bigint GENERATED ALWAYS AS IDENTITY,
        "PayPeriodId" bigint NOT NULL,
        "EmployeeId" bigint NOT NULL,
        "EmployeeNumber" character varying(50),
        "EmployeeName" character varying(200) NOT NULL,
        "RegularHours" numeric(18,2) NOT NULL DEFAULT 0.0,
        "OvertimeHours" numeric(18,2) NOT NULL DEFAULT 0.0,
        "DoubleTimeHours" numeric(18,2) NOT NULL DEFAULT 0.0,
        "RegularRate" numeric(18,4) NOT NULL DEFAULT 0.0,
        "OvertimeRate" numeric(18,4) NOT NULL DEFAULT 0.0,
        "DoubleTimeRate" numeric(18,4) NOT NULL DEFAULT 0.0,
        "RegularAmount" numeric(18,2) NOT NULL DEFAULT 0.0,
        "OvertimeAmount" numeric(18,2) NOT NULL DEFAULT 0.0,
        "DoubleTimeAmount" numeric(18,2) NOT NULL DEFAULT 0.0,
        "CommissionAmount" numeric(18,2) NOT NULL DEFAULT 0.0,
        "BonusAmount" numeric(18,2) NOT NULL DEFAULT 0.0,
        "AdjustmentAmount" numeric(18,2) NOT NULL DEFAULT 0.0,
        "BurdenTypeId" character varying(1) NOT NULL DEFAULT 'P',
        "BurdenValue" numeric(18,4) NOT NULL DEFAULT 0.0,
        "BurdenAmount" numeric(18,2) NOT NULL DEFAULT 0.0,
        "GrossPayAmount" numeric(18,2) NOT NULL DEFAULT 0.0,
        "SignatureFileId" bigint,
        "SignedOn" timestamptz,
        "SignedBy" character varying(200),
        "Notes" text,
        "CreatedOn" timestamptz NOT NULL DEFAULT (now()),
        "CreatedBy" bigint NOT NULL,
        "UpdatedOn" timestamptz,
        "UpdatedBy" bigint,
        CONSTRAINT "PK_FgsPayroll" PRIMARY KEY ("Id"),
        CONSTRAINT "CK_FgsPayroll_BurdenType" CHECK ("BurdenTypeId" IN ('P', 'F')),
        CONSTRAINT "CK_FgsPayroll_Signature" CHECK (("SignedOn" IS NULL AND "SignatureFileId" IS NULL AND "SignedBy" IS NULL) OR ("SignedOn" IS NOT NULL AND "SignatureFileId" IS NOT NULL AND "SignedBy" IS NOT NULL)),
        CONSTRAINT "FK_FgsPayroll_FgsPayrollPayPeriod" FOREIGN KEY ("PayPeriodId") REFERENCES dispatch."FgsPayrollPayPeriod" ("Id") ON DELETE RESTRICT,
        CONSTRAINT "FK_FgsPayroll_FgsTenantCompanyCache" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dispatch."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT
    );
    COMMENT ON TABLE dispatch."FgsPayroll" IS 'Stores payroll results for a single employee within a payroll pay period.';
    COMMENT ON COLUMN dispatch."FgsPayroll"."TenantId" IS 'Tenant identifier.';
    COMMENT ON COLUMN dispatch."FgsPayroll"."CompanyId" IS 'Company identifier.';
    COMMENT ON COLUMN dispatch."FgsPayroll"."Id" IS 'Primary key.';
    COMMENT ON COLUMN dispatch."FgsPayroll"."PayPeriodId" IS 'Payroll pay period associated with this payroll record.';
    COMMENT ON COLUMN dispatch."FgsPayroll"."EmployeeId" IS 'Employee associated with this payroll record. References user service; no FK by design.';
    COMMENT ON COLUMN dispatch."FgsPayroll"."EmployeeNumber" IS 'Employee number snapshot captured at payroll calculation time.';
    COMMENT ON COLUMN dispatch."FgsPayroll"."EmployeeName" IS 'Employee name snapshot captured at payroll calculation time.';
    COMMENT ON COLUMN dispatch."FgsPayroll"."RegularHours" IS 'Regular hours included in payroll calculation.';
    COMMENT ON COLUMN dispatch."FgsPayroll"."OvertimeHours" IS 'Overtime hours included in payroll calculation.';
    COMMENT ON COLUMN dispatch."FgsPayroll"."DoubleTimeHours" IS 'Double-time hours included in payroll calculation.';
    COMMENT ON COLUMN dispatch."FgsPayroll"."RegularRate" IS 'Regular pay rate snapshot at calculation time.';
    COMMENT ON COLUMN dispatch."FgsPayroll"."OvertimeRate" IS 'Overtime pay rate snapshot at calculation time.';
    COMMENT ON COLUMN dispatch."FgsPayroll"."DoubleTimeRate" IS 'Double-time pay rate snapshot at calculation time.';
    COMMENT ON COLUMN dispatch."FgsPayroll"."RegularAmount" IS 'Regular pay amount.';
    COMMENT ON COLUMN dispatch."FgsPayroll"."OvertimeAmount" IS 'Overtime pay amount.';
    COMMENT ON COLUMN dispatch."FgsPayroll"."DoubleTimeAmount" IS 'Double-time pay amount.';
    COMMENT ON COLUMN dispatch."FgsPayroll"."CommissionAmount" IS 'Commission amount included in payroll.';
    COMMENT ON COLUMN dispatch."FgsPayroll"."BonusAmount" IS 'Bonus amount included in payroll.';
    COMMENT ON COLUMN dispatch."FgsPayroll"."AdjustmentAmount" IS 'Positive or negative payroll adjustment amount.';
    COMMENT ON COLUMN dispatch."FgsPayroll"."BurdenTypeId" IS 'Burden calculation method. P=Percent, F=Fixed Amount.';
    COMMENT ON COLUMN dispatch."FgsPayroll"."BurdenValue" IS 'Burden percentage or fixed amount snapshot used during payroll calculation.';
    COMMENT ON COLUMN dispatch."FgsPayroll"."BurdenAmount" IS 'Calculated burden amount used for costing and profitability reporting.';
    COMMENT ON COLUMN dispatch."FgsPayroll"."GrossPayAmount" IS 'Total gross pay exported to the payroll provider.';
    COMMENT ON COLUMN dispatch."FgsPayroll"."SignatureFileId" IS 'Reference to employee payroll acknowledgement signature document.';
    COMMENT ON COLUMN dispatch."FgsPayroll"."SignedOn" IS 'Date and time payroll acknowledgement was signed.';
    COMMENT ON COLUMN dispatch."FgsPayroll"."SignedBy" IS 'Name of person who signed the payroll acknowledgement.';
    COMMENT ON COLUMN dispatch."FgsPayroll"."Notes" IS 'Optional payroll notes, explanations and adjustment reasons.';
    COMMENT ON COLUMN dispatch."FgsPayroll"."CreatedOn" IS 'Date and time the record was created.';
    COMMENT ON COLUMN dispatch."FgsPayroll"."CreatedBy" IS 'User who created the record.';
    COMMENT ON COLUMN dispatch."FgsPayroll"."UpdatedOn" IS 'Date and time the record was last updated.';
    COMMENT ON COLUMN dispatch."FgsPayroll"."UpdatedBy" IS 'User who last updated the record.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE TABLE dispatch."FgsWorkOrderAsset" (
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "WorkOrderId" bigint NOT NULL,
        "AssetId" bigint NOT NULL,
        "CreatedOn" timestamptz NOT NULL,
        "CreatedBy" character varying(100),
        CONSTRAINT "PK_FgsWorkOrderAsset" PRIMARY KEY ("TenantId", "CompanyId", "WorkOrderId", "AssetId"),
        CONSTRAINT "FK_FgsWorkOrderAsset_TenantCompany" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dispatch."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT,
        CONSTRAINT "FK_FgsWorkOrderAsset_WorkOrder" FOREIGN KEY ("WorkOrderId") REFERENCES dispatch."FgsWorkOrder" ("Id") ON DELETE CASCADE
    );
    COMMENT ON TABLE dispatch."FgsWorkOrderAsset" IS 'Associates assets with a work order.';
    COMMENT ON COLUMN dispatch."FgsWorkOrderAsset"."TenantId" IS 'Tenant identifier.';
    COMMENT ON COLUMN dispatch."FgsWorkOrderAsset"."CompanyId" IS 'Company identifier.';
    COMMENT ON COLUMN dispatch."FgsWorkOrderAsset"."WorkOrderId" IS 'Parent work order identifier.';
    COMMENT ON COLUMN dispatch."FgsWorkOrderAsset"."AssetId" IS 'Asset identifier. References asset service; no FK by design.';
    COMMENT ON COLUMN dispatch."FgsWorkOrderAsset"."CreatedOn" IS 'Date and time the record was created.';
    COMMENT ON COLUMN dispatch."FgsWorkOrderAsset"."CreatedBy" IS 'User who created the record.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE TABLE dispatch."FgsWorkOrderIntegration" (
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "WorkOrderId" bigint,
        "IntegrationName" character varying(100) NOT NULL,
        "ExternalId" character varying(100) NOT NULL,
        "ExternalWorkOrderNumber" character varying(100),
        "ReceivedOn" timestamptz NOT NULL,
        "Status" character varying(50) NOT NULL DEFAULT 'Received',
        "Payload" jsonb NOT NULL,
        "ProcessedOn" timestamptz,
        "ProcessedBy" character varying(100),
        "CreatedOn" timestamptz NOT NULL DEFAULT (now()),
        "CreatedBy" character varying(100),
        "UpdatedOn" timestamptz,
        "UpdatedBy" character varying(100),
        CONSTRAINT "PK_FgsWorkOrderIntegration" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_FgsWorkOrderIntegration_FgsTenantCompanyCache_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dispatch."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT,
        CONSTRAINT "FK_FgsWorkOrderIntegration_WorkOrder" FOREIGN KEY ("WorkOrderId") REFERENCES dispatch."FgsWorkOrder" ("Id") ON DELETE SET NULL
    );
    COMMENT ON TABLE dispatch."FgsWorkOrderIntegration" IS 'Stores externally received work orders and their raw payloads before they are reviewed and booked into dispatch.';
    COMMENT ON COLUMN dispatch."FgsWorkOrderIntegration"."TenantId" IS 'Tenant identifier.';
    COMMENT ON COLUMN dispatch."FgsWorkOrderIntegration"."CompanyId" IS 'Company identifier.';
    COMMENT ON COLUMN dispatch."FgsWorkOrderIntegration"."Id" IS 'Primary key.';
    COMMENT ON COLUMN dispatch."FgsWorkOrderIntegration"."WorkOrderId" IS 'Dispatch work order created from this integration record.';
    COMMENT ON COLUMN dispatch."FgsWorkOrderIntegration"."IntegrationName" IS 'Integration source such as Corrigo, ServiceChannel, Verizon, AHS, etc.';
    COMMENT ON COLUMN dispatch."FgsWorkOrderIntegration"."ExternalId" IS 'Primary identifier from the external system.';
    COMMENT ON COLUMN dispatch."FgsWorkOrderIntegration"."ExternalWorkOrderNumber" IS 'External work order number visible to users in the external system.';
    COMMENT ON COLUMN dispatch."FgsWorkOrderIntegration"."ReceivedOn" IS 'Date and time the payload was received from the external system.';
    COMMENT ON COLUMN dispatch."FgsWorkOrderIntegration"."Status" IS 'Current processing status of the imported work order.';
    COMMENT ON COLUMN dispatch."FgsWorkOrderIntegration"."Payload" IS 'Raw JSON payload received from the external system.';
    COMMENT ON COLUMN dispatch."FgsWorkOrderIntegration"."ProcessedOn" IS 'Date and time the record was processed or booked into dispatch.';
    COMMENT ON COLUMN dispatch."FgsWorkOrderIntegration"."ProcessedBy" IS 'User that processed or booked the work order.';
    COMMENT ON COLUMN dispatch."FgsWorkOrderIntegration"."CreatedOn" IS 'Date and time the record was created.';
    COMMENT ON COLUMN dispatch."FgsWorkOrderIntegration"."CreatedBy" IS 'User who created the record.';
    COMMENT ON COLUMN dispatch."FgsWorkOrderIntegration"."UpdatedOn" IS 'Date and time the record was last updated.';
    COMMENT ON COLUMN dispatch."FgsWorkOrderIntegration"."UpdatedBy" IS 'User who last updated the record.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE TABLE dispatch."FgsWorkOrderItem" (
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "WorkOrderId" bigint NOT NULL,
        "InventoryItemId" bigint,
        "ItemName" character varying(200),
        "Description" text,
        "Quantity" numeric(18,2) NOT NULL DEFAULT 1.0,
        "DisplayOrder" integer NOT NULL DEFAULT 1,
        "CreatedOn" timestamptz NOT NULL DEFAULT (now()),
        "CreatedBy" character varying(100),
        "UpdatedOn" timestamptz,
        "UpdatedBy" character varying(100),
        CONSTRAINT "PK_FgsWorkOrderItem" PRIMARY KEY ("Id"),
        CONSTRAINT "CK_FgsWorkOrderItem_Item" CHECK ("InventoryItemId" IS NOT NULL OR COALESCE(TRIM(BOTH FROM "ItemName"), '') <> ''),
        CONSTRAINT "CK_FgsWorkOrderItem_Quantity" CHECK ("Quantity" > 0),
        CONSTRAINT "FK_FgsWorkOrderItem_FgsTenantCompanyCache_TenantId_CompanyId" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dispatch."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT,
        CONSTRAINT "FK_FgsWorkOrderItem_WorkOrder" FOREIGN KEY ("WorkOrderId") REFERENCES dispatch."FgsWorkOrder" ("Id") ON DELETE CASCADE
    );
    COMMENT ON TABLE dispatch."FgsWorkOrderItem" IS 'Stores materials used on a work order. Items may come from the inventory catalog or be entered manually. Customer billing is stored separately on invoice lines.';
    COMMENT ON COLUMN dispatch."FgsWorkOrderItem"."TenantId" IS 'Tenant identifier.';
    COMMENT ON COLUMN dispatch."FgsWorkOrderItem"."CompanyId" IS 'Company identifier.';
    COMMENT ON COLUMN dispatch."FgsWorkOrderItem"."Id" IS 'Primary key.';
    COMMENT ON COLUMN dispatch."FgsWorkOrderItem"."WorkOrderId" IS 'Parent work order identifier.';
    COMMENT ON COLUMN dispatch."FgsWorkOrderItem"."InventoryItemId" IS 'Inventory item identifier. May be NULL when the item is manually entered.';
    COMMENT ON COLUMN dispatch."FgsWorkOrderItem"."ItemName" IS 'Item name used when the item does not exist in the inventory catalog.';
    COMMENT ON COLUMN dispatch."FgsWorkOrderItem"."Description" IS 'Additional item description or technician notes.';
    COMMENT ON COLUMN dispatch."FgsWorkOrderItem"."Quantity" IS 'Quantity of material used on the work order.';
    COMMENT ON COLUMN dispatch."FgsWorkOrderItem"."DisplayOrder" IS 'Display order within the work order item list.';
    COMMENT ON COLUMN dispatch."FgsWorkOrderItem"."CreatedOn" IS 'Date and time the record was created.';
    COMMENT ON COLUMN dispatch."FgsWorkOrderItem"."CreatedBy" IS 'User who created the record.';
    COMMENT ON COLUMN dispatch."FgsWorkOrderItem"."UpdatedOn" IS 'Date and time the record was last updated.';
    COMMENT ON COLUMN dispatch."FgsWorkOrderItem"."UpdatedBy" IS 'User who last updated the record.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE TABLE dispatch."FgsAppointmentAssignment" (
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "Id" bigint GENERATED BY DEFAULT AS IDENTITY,
        "AppointmentId" bigint NOT NULL,
        "EmployeeId" bigint NOT NULL,
        "CrewId" bigint,
        "ServiceDate" date NOT NULL,
        "ScheduledTime" time NOT NULL,
        "EstimatedHours" numeric(8,2) NOT NULL,
        "ActualStartOn" timestamptz,
        "ActualEndOn" timestamptz,
        "AssignedOn" timestamptz NOT NULL DEFAULT (now()),
        "AssignedBy" bigint NOT NULL,
        "CreatedOn" timestamptz NOT NULL DEFAULT (now()),
        "CreatedBy" bigint NOT NULL,
        "UpdatedOn" timestamptz,
        "UpdatedBy" bigint,
        CONSTRAINT "PK_FgsAppointmentAssignment" PRIMARY KEY ("Id"),
        CONSTRAINT "CK_FgsAppointmentAssignment_EstimatedHours" CHECK ("EstimatedHours" > 0),
        CONSTRAINT "FK_FgsAppointmentAssignment_Appointment" FOREIGN KEY ("AppointmentId") REFERENCES dispatch."FgsAppointment" ("Id") ON DELETE RESTRICT,
        CONSTRAINT "FK_FgsAppointmentAssignment_Crew" FOREIGN KEY ("TenantId", "CompanyId", "CrewId") REFERENCES dispatch."FgsCrew" ("TenantId", "CompanyId", "Id") ON DELETE RESTRICT,
        CONSTRAINT "FK_FgsAppointmentAssignment_TenantCompany" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dispatch."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT
    );
    COMMENT ON TABLE dispatch."FgsAppointmentAssignment" IS 'Represents a technician assigned to a scheduled appointment.';
    COMMENT ON COLUMN dispatch."FgsAppointmentAssignment"."TenantId" IS 'Tenant identifier.';
    COMMENT ON COLUMN dispatch."FgsAppointmentAssignment"."CompanyId" IS 'Company identifier.';
    COMMENT ON COLUMN dispatch."FgsAppointmentAssignment"."Id" IS 'Primary key.';
    COMMENT ON COLUMN dispatch."FgsAppointmentAssignment"."AppointmentId" IS 'Appointment associated with the assignment.';
    COMMENT ON COLUMN dispatch."FgsAppointmentAssignment"."EmployeeId" IS 'Employee assigned to the appointment. References user service; no FK by design.';
    COMMENT ON COLUMN dispatch."FgsAppointmentAssignment"."CrewId" IS 'Crew assignment snapshot at the time of scheduling.';
    COMMENT ON COLUMN dispatch."FgsAppointmentAssignment"."ServiceDate" IS 'Scheduled service date for the technician assignment.';
    COMMENT ON COLUMN dispatch."FgsAppointmentAssignment"."ScheduledTime" IS 'Scheduled local start time for the technician assignment.';
    COMMENT ON COLUMN dispatch."FgsAppointmentAssignment"."EstimatedHours" IS 'Estimated hours assigned to the technician.';
    COMMENT ON COLUMN dispatch."FgsAppointmentAssignment"."ActualStartOn" IS 'System-maintained start timestamp derived from assignment events.';
    COMMENT ON COLUMN dispatch."FgsAppointmentAssignment"."ActualEndOn" IS 'System-maintained end timestamp derived from assignment events.';
    COMMENT ON COLUMN dispatch."FgsAppointmentAssignment"."AssignedOn" IS 'Date and time the technician was assigned.';
    COMMENT ON COLUMN dispatch."FgsAppointmentAssignment"."AssignedBy" IS 'User who assigned the technician.';
    COMMENT ON COLUMN dispatch."FgsAppointmentAssignment"."CreatedOn" IS 'Date and time the record was created.';
    COMMENT ON COLUMN dispatch."FgsAppointmentAssignment"."CreatedBy" IS 'User who created the record.';
    COMMENT ON COLUMN dispatch."FgsAppointmentAssignment"."UpdatedOn" IS 'Date and time the record was last updated.';
    COMMENT ON COLUMN dispatch."FgsAppointmentAssignment"."UpdatedBy" IS 'User who last updated the record.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE TABLE dispatch."FgsPayrollLine" (
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "Id" bigint GENERATED ALWAYS AS IDENTITY,
        "PayrollId" bigint NOT NULL,
        "PayrollLineTypeId" smallint NOT NULL,
        "Description" character varying(250) NOT NULL,
        "Amount" numeric(18,2) NOT NULL DEFAULT 0.0,
        "Notes" text,
        "CreatedOn" timestamptz NOT NULL DEFAULT (now()),
        "CreatedBy" bigint NOT NULL,
        "UpdatedOn" timestamptz,
        "UpdatedBy" bigint,
        CONSTRAINT "PK_FgsPayrollLine" PRIMARY KEY ("Id"),
        CONSTRAINT "CK_FgsPayrollLine_Type" CHECK ("PayrollLineTypeId" IN (1, 2, 3)),
        CONSTRAINT "FK_FgsPayrollLine_FgsPayroll" FOREIGN KEY ("PayrollId") REFERENCES dispatch."FgsPayroll" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_FgsPayrollLine_FgsTenantCompanyCache" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dispatch."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT
    );
    COMMENT ON TABLE dispatch."FgsPayrollLine" IS 'Stores payroll detail lines associated with a payroll record.';
    COMMENT ON COLUMN dispatch."FgsPayrollLine"."TenantId" IS 'Tenant identifier.';
    COMMENT ON COLUMN dispatch."FgsPayrollLine"."CompanyId" IS 'Company identifier.';
    COMMENT ON COLUMN dispatch."FgsPayrollLine"."Id" IS 'Primary key.';
    COMMENT ON COLUMN dispatch."FgsPayrollLine"."PayrollId" IS 'Parent payroll record.';
    COMMENT ON COLUMN dispatch."FgsPayrollLine"."PayrollLineTypeId" IS 'Payroll line type. 1=Commission, 2=Bonus, 3=Adjustment.';
    COMMENT ON COLUMN dispatch."FgsPayrollLine"."Description" IS 'User-facing payroll line description.';
    COMMENT ON COLUMN dispatch."FgsPayrollLine"."Amount" IS 'Positive or negative payroll line amount.';
    COMMENT ON COLUMN dispatch."FgsPayrollLine"."Notes" IS 'Optional notes and explanation for the payroll line.';
    COMMENT ON COLUMN dispatch."FgsPayrollLine"."CreatedOn" IS 'Date and time the record was created.';
    COMMENT ON COLUMN dispatch."FgsPayrollLine"."CreatedBy" IS 'User who created the record.';
    COMMENT ON COLUMN dispatch."FgsPayrollLine"."UpdatedOn" IS 'Date and time the record was last updated.';
    COMMENT ON COLUMN dispatch."FgsPayrollLine"."UpdatedBy" IS 'User who last updated the record.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE TABLE dispatch."FgsAppointmentAssignmentEvent" (
        "TenantId" bigint NOT NULL,
        "CompanyId" bigint NOT NULL,
        "Id" bigint GENERATED ALWAYS AS IDENTITY,
        "AssignmentId" bigint,
        "EmployeeId" bigint NOT NULL,
        "ServiceDate" date NOT NULL,
        "EventTypeId" smallint NOT NULL,
        "EventOccurredOn" timestamptz NOT NULL,
        "EnteredByOffice" boolean NOT NULL DEFAULT FALSE,
        "Notes" character varying(500),
        "CreatedOn" timestamptz NOT NULL DEFAULT (now()),
        "CreatedBy" character varying(100),
        "UpdatedOn" timestamptz,
        "UpdatedBy" character varying(100),
        CONSTRAINT "PK_FgsAppointmentAssignmentEvent" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_FgsAppointmentAssignmentEvent_FgsAppointmentAssignment" FOREIGN KEY ("AssignmentId") REFERENCES dispatch."FgsAppointmentAssignment" ("Id") ON DELETE RESTRICT,
        CONSTRAINT "FK_FgsAppointmentAssignmentEvent_FgsTenantCompanyCache" FOREIGN KEY ("TenantId", "CompanyId") REFERENCES dispatch."FgsTenantCompanyCache" ("TenantId", "CompanyId") ON DELETE RESTRICT
    );
    COMMENT ON TABLE dispatch."FgsAppointmentAssignmentEvent" IS 'Stores technician activity events used for dispatch tracking, payroll calculations, utilization reporting and technician history.';
    COMMENT ON COLUMN dispatch."FgsAppointmentAssignmentEvent"."TenantId" IS 'Tenant identifier.';
    COMMENT ON COLUMN dispatch."FgsAppointmentAssignmentEvent"."CompanyId" IS 'Company identifier.';
    COMMENT ON COLUMN dispatch."FgsAppointmentAssignmentEvent"."Id" IS 'Primary key.';
    COMMENT ON COLUMN dispatch."FgsAppointmentAssignmentEvent"."AssignmentId" IS 'Appointment assignment associated with the event. NULL for technician-only events such as On Duty, Off Duty, Lunch Start and Lunch End.';
    COMMENT ON COLUMN dispatch."FgsAppointmentAssignmentEvent"."EmployeeId" IS 'Employee associated with the event. References user service; no FK by design.';
    COMMENT ON COLUMN dispatch."FgsAppointmentAssignmentEvent"."ServiceDate" IS 'Business service date associated with the event. Used for overnight work and payroll calculations.';
    COMMENT ON COLUMN dispatch."FgsAppointmentAssignmentEvent"."EventTypeId" IS 'References glo.GloAppointmentAssignmentEventType.EventTypeId.';
    COMMENT ON COLUMN dispatch."FgsAppointmentAssignmentEvent"."EventOccurredOn" IS 'Actual timestamp when the event occurred.';
    COMMENT ON COLUMN dispatch."FgsAppointmentAssignmentEvent"."EnteredByOffice" IS 'Indicates the event was entered or reconstructed by office staff rather than captured by the technician.';
    COMMENT ON COLUMN dispatch."FgsAppointmentAssignmentEvent"."Notes" IS 'Optional notes entered by office staff or technician.';
    COMMENT ON COLUMN dispatch."FgsAppointmentAssignmentEvent"."CreatedOn" IS 'Date and time the record was created.';
    COMMENT ON COLUMN dispatch."FgsAppointmentAssignmentEvent"."CreatedBy" IS 'User who created the record.';
    COMMENT ON COLUMN dispatch."FgsAppointmentAssignmentEvent"."UpdatedOn" IS 'Date and time the record was last updated.';
    COMMENT ON COLUMN dispatch."FgsAppointmentAssignmentEvent"."UpdatedBy" IS 'User who last updated the record.';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsAppointment_Crew" ON dispatch."FgsAppointment" ("TenantId", "CompanyId", "CrewId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsAppointment_ServiceDate" ON dispatch."FgsAppointment" ("TenantId", "CompanyId", "ServiceDate");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsAppointment_Source" ON dispatch."FgsAppointment" ("TenantId", "CompanyId", "SourceTypeId", "SourceId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsAppointment_Status" ON dispatch."FgsAppointment" ("TenantId", "CompanyId", "AppointmentStatusId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsAppointmentAssignment_Appointment" ON dispatch."FgsAppointmentAssignment" ("TenantId", "CompanyId", "AppointmentId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsAppointmentAssignment_AppointmentId" ON dispatch."FgsAppointmentAssignment" ("AppointmentId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsAppointmentAssignment_Crew" ON dispatch."FgsAppointmentAssignment" ("TenantId", "CompanyId", "CrewId", "ServiceDate");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsAppointmentAssignment_Employee" ON dispatch."FgsAppointmentAssignment" ("TenantId", "CompanyId", "EmployeeId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsAppointmentAssignment_EmployeeSchedule" ON dispatch."FgsAppointmentAssignment" ("TenantId", "CompanyId", "EmployeeId", "ServiceDate", "ScheduledTime");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsAppointmentAssignment_Overlap" ON dispatch."FgsAppointmentAssignment" ("TenantId", "CompanyId", "EmployeeId", "ActualStartOn", "ActualEndOn");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsAppointmentAssignment_ServiceDate" ON dispatch."FgsAppointmentAssignment" ("TenantId", "CompanyId", "ServiceDate");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE UNIQUE INDEX "UQ_FgsAppointmentAssignment_AppointmentEmployee" ON dispatch."FgsAppointmentAssignment" ("TenantId", "CompanyId", "AppointmentId", "EmployeeId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsAppointmentAssignmentEvent_Assignment" ON dispatch."FgsAppointmentAssignmentEvent" ("TenantId", "CompanyId", "AssignmentId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsAppointmentAssignmentEvent_AssignmentEventOccurredOn" ON dispatch."FgsAppointmentAssignmentEvent" ("TenantId", "CompanyId", "AssignmentId", "EventOccurredOn");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsAppointmentAssignmentEvent_AssignmentId" ON dispatch."FgsAppointmentAssignmentEvent" ("AssignmentId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsAppointmentAssignmentEvent_Employee" ON dispatch."FgsAppointmentAssignmentEvent" ("TenantId", "CompanyId", "EmployeeId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsAppointmentAssignmentEvent_EmployeeEventOccurredOn" ON dispatch."FgsAppointmentAssignmentEvent" ("TenantId", "CompanyId", "EmployeeId", "EventOccurredOn");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsAppointmentAssignmentEvent_EventType" ON dispatch."FgsAppointmentAssignmentEvent" ("TenantId", "CompanyId", "EventTypeId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsAppointmentAssignmentEvent_ServiceDate" ON dispatch."FgsAppointmentAssignmentEvent" ("TenantId", "CompanyId", "ServiceDate");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsAppointmentAssignmentEvent_TenantCompany" ON dispatch."FgsAppointmentAssignmentEvent" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE UNIQUE INDEX "UX_FgsAppointmentAssignmentEvent_NoDuplicates" ON dispatch."FgsAppointmentAssignmentEvent" ("TenantId", "CompanyId", "EmployeeId", "EventTypeId", "EventOccurredOn");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsCrew_IsActive" ON dispatch."FgsCrew" ("TenantId", "CompanyId", "IsActive");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsCrew_TenantCompany" ON dispatch."FgsCrew" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE UNIQUE INDEX "UX_FgsCrew_CrewCode" ON dispatch."FgsCrew" ("TenantId", "CompanyId", "CrewCode");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE UNIQUE INDEX "UX_FgsCrew_CrewName" ON dispatch."FgsCrew" ("TenantId", "CompanyId", "CrewName");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsCrewMember_CrewId" ON dispatch."FgsCrewMember" ("CrewId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsCrewMember_IsLead" ON dispatch."FgsCrewMember" ("TenantId", "CompanyId", "CrewId", "IsLead");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsCrewMember_TenantCompany" ON dispatch."FgsCrewMember" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE UNIQUE INDEX "UX_FgsCrewMember_Employee" ON dispatch."FgsCrewMember" ("TenantId", "CompanyId", "EmployeeId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE UNIQUE INDEX "UX_FgsCrewMember_LeadPerCrew" ON dispatch."FgsCrewMember" ("TenantId", "CompanyId", "CrewId") WHERE "IsLead" = true;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsPayroll_Employee" ON dispatch."FgsPayroll" ("TenantId", "CompanyId", "EmployeeId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsPayroll_PayPeriod" ON dispatch."FgsPayroll" ("TenantId", "CompanyId", "PayPeriodId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsPayroll_PayPeriodId" ON dispatch."FgsPayroll" ("PayPeriodId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsPayroll_SignedOn" ON dispatch."FgsPayroll" ("TenantId", "CompanyId", "SignedOn");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsPayroll_TenantCompany" ON dispatch."FgsPayroll" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE UNIQUE INDEX "UX_FgsPayroll_PayPeriodEmployee" ON dispatch."FgsPayroll" ("TenantId", "CompanyId", "PayPeriodId", "EmployeeId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsPayrollLine_Payroll" ON dispatch."FgsPayrollLine" ("TenantId", "CompanyId", "PayrollId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsPayrollLine_PayrollId" ON dispatch."FgsPayrollLine" ("PayrollId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsPayrollLine_PayrollType" ON dispatch."FgsPayrollLine" ("TenantId", "CompanyId", "PayrollId", "PayrollLineTypeId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsPayrollLine_TenantCompany" ON dispatch."FgsPayrollLine" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsPayrollLine_Type" ON dispatch."FgsPayrollLine" ("TenantId", "CompanyId", "PayrollLineTypeId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsPayrollPayPeriod_EndDate" ON dispatch."FgsPayrollPayPeriod" ("TenantId", "CompanyId", "PeriodEndDate");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsPayrollPayPeriod_StartDate" ON dispatch."FgsPayrollPayPeriod" ("TenantId", "CompanyId", "PeriodStartDate");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsPayrollPayPeriod_Status" ON dispatch."FgsPayrollPayPeriod" ("TenantId", "CompanyId", "PayrollStatusId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsPayrollPayPeriod_TenantCompany" ON dispatch."FgsPayrollPayPeriod" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE UNIQUE INDEX "UX_FgsPayrollPayPeriod_DateRange" ON dispatch."FgsPayrollPayPeriod" ("TenantId", "CompanyId", "PeriodStartDate", "PeriodEndDate");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE UNIQUE INDEX "UX_FgsPayrollPayPeriod_PayPeriodCode" ON dispatch."FgsPayrollPayPeriod" ("TenantId", "CompanyId", "PayPeriodCode");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsWorkOrder_Customer" ON dispatch."FgsWorkOrder" ("TenantId", "CompanyId", "CustomerId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsWorkOrder_JobType" ON dispatch."FgsWorkOrder" ("TenantId", "CompanyId", "JobTypeId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsWorkOrder_Location" ON dispatch."FgsWorkOrder" ("TenantId", "CompanyId", "LocationId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsWorkOrder_Priority" ON dispatch."FgsWorkOrder" ("TenantId", "CompanyId", "PriorityId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsWorkOrder_Project" ON dispatch."FgsWorkOrder" ("TenantId", "CompanyId", "ProjectId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsWorkOrder_RequestedOn" ON dispatch."FgsWorkOrder" ("TenantId", "CompanyId", "RequestedOn");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsWorkOrder_ServiceAgreement" ON dispatch."FgsWorkOrder" ("TenantId", "CompanyId", "ServiceAgreementId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsWorkOrder_Source" ON dispatch."FgsWorkOrder" ("TenantId", "CompanyId", "Source");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsWorkOrder_Status" ON dispatch."FgsWorkOrder" ("TenantId", "CompanyId", "WorkOrderStatusId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsWorkOrder_TenantCompany" ON dispatch."FgsWorkOrder" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsWorkOrder_TimeSlot" ON dispatch."FgsWorkOrder" ("TenantId", "CompanyId", "TimeSlotId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE UNIQUE INDEX "UQ_FgsWorkOrder_WorkOrderNumber" ON dispatch."FgsWorkOrder" ("TenantId", "CompanyId", "WorkOrderNumber");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsWorkOrderAsset_WorkOrderId" ON dispatch."FgsWorkOrderAsset" ("WorkOrderId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsWorkOrderIntegration_ReceivedOn" ON dispatch."FgsWorkOrderIntegration" ("TenantId", "CompanyId", "ReceivedOn");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsWorkOrderIntegration_Status" ON dispatch."FgsWorkOrderIntegration" ("TenantId", "CompanyId", "Status");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsWorkOrderIntegration_TenantId_CompanyId" ON dispatch."FgsWorkOrderIntegration" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsWorkOrderIntegration_WorkOrderId" ON dispatch."FgsWorkOrderIntegration" ("TenantId", "CompanyId", "WorkOrderId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsWorkOrderIntegration_WorkOrderId1" ON dispatch."FgsWorkOrderIntegration" ("WorkOrderId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE UNIQUE INDEX "UQ_FgsWorkOrderIntegration_External" ON dispatch."FgsWorkOrderIntegration" ("TenantId", "CompanyId", "IntegrationName", "ExternalId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsWorkOrderItem_DisplayOrder" ON dispatch."FgsWorkOrderItem" ("TenantId", "CompanyId", "WorkOrderId", "DisplayOrder");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsWorkOrderItem_InventoryItemId" ON dispatch."FgsWorkOrderItem" ("TenantId", "CompanyId", "InventoryItemId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsWorkOrderItem_TenantId_CompanyId" ON dispatch."FgsWorkOrderItem" ("TenantId", "CompanyId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsWorkOrderItem_WorkOrderId" ON dispatch."FgsWorkOrderItem" ("TenantId", "CompanyId", "WorkOrderId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    CREATE INDEX "IX_FgsWorkOrderItem_WorkOrderId1" ON dispatch."FgsWorkOrderItem" ("WorkOrderId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dispatch."__EFMigrationsHistory" WHERE "MigrationId" = '20260614161609_AddDispatchSchedulingSchema') THEN
    INSERT INTO dispatch."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260614161609_AddDispatchSchedulingSchema', '10.0.8');
    END IF;
END $EF$;
COMMIT;

