#!/usr/bin/env python3
"""Generate Setup Service CRUD layers mirroring TitlesOfCourtesy pattern."""
from __future__ import annotations

import argparse
import os
import textwrap
from dataclasses import dataclass, field
from pathlib import Path
from typing import Literal

ROOT = Path(__file__).resolve().parents[1]
APP = ROOT / "Fgs.Setup.Application"
INFRA = ROOT / "Fgs.Setup.Infrastructure"
API = ROOT / "Fgs.Setup.API"
TESTS = ROOT / "Fgs.Setup.Tests"


@dataclass
class Field:
    name: str
    cs_type: str
    max_length: int | None = None
    uppercase: bool = False
    required: bool = True
    in_create: bool = True
    in_update: bool = True
    in_patch: bool = True
    in_summary: bool = True
    in_lookup: bool = False
    in_list_filter: bool = False
    default: str | None = None
    validator_min: int | None = None


@dataclass
class EntityConfig:
    type_prefix: str
    plural_folder: str
    route: str
    controller: str
    domain_entity: str
    table: str
    dbset: str
    display_name: str
    base: Literal["setup_tenant", "lead_entity", "nullable_tenant_entity", "tag_entity"]
    code_field: str
    name_field: str
    has_display_order: bool = False
    sort_field: Literal["DisplayOrder", "SortOrder"] | None = None
    fields: list[Field] = field(default_factory=list)
    unique_code: bool = True
    unique_name: bool = False
    unique_composite: tuple[str, ...] | None = None
    sales_applies_to_check: bool = False
    skip_controller: bool = False
    extra_list_filters: list[tuple[str, str]] = field(default_factory=list)
    fk_checks: list[tuple[str, str, str]] = field(default_factory=list)
    search_columns: list[str] = field(default_factory=list)
    abstractions_folder: str | None = None
    infra_folder: str | None = None


FK_EXISTS_TABLES: dict[str, tuple[str, str]] = {
    "JobTypeCategoryId": ('setup."FgsJobTypeCategory"', '"TenantId" = @TenantId AND "CompanyId" = @CompanyId AND "Id" = @Id AND "IsActive" = TRUE'),
    "JobTypeSubCategoryId": ('setup."FgsJobTypeSubCategory"', '"TenantId" = @TenantId AND "CompanyId" = @CompanyId AND "Id" = @Id AND "IsActive" = TRUE'),
    "GloResolutionTypeId": ('setup."GloResolutionTypeCache"', '"ResolutionTypeId" = @Id AND "IsActive" = TRUE'),
    "FgsSetupTechTradeId": ('setup."FgsSetupTechTrade"', '"TenantId" = @TenantId AND "CompanyId" = @CompanyId AND "Id" = @Id AND "IsActive" = TRUE'),
    "FgsSetupZoneId": ('setup."FgsSetupZone"', '"TenantId" = @TenantId AND "CompanyId" = @CompanyId AND "Id" = @Id AND "IsActive" = TRUE'),
    "FgsSetupTaxId": ('setup."FgsSetupTax"', '"TenantId" = @TenantId AND "CompanyId" = @CompanyId AND "Id" = @Id AND "IsActive" = TRUE'),
    "FgsSetupTaxAuthorityId": ('setup."FgsSetupTaxAuthority"', '"TenantId" = @TenantId AND "CompanyId" = @CompanyId AND "Id" = @Id AND "IsActive" = TRUE'),
    "PaymentTermId": ('setup."FgsSetupPaymentTerm"', '"TenantId" = @TenantId AND "CompanyId" = @CompanyId AND "Id" = @Id AND "IsActive" = TRUE'),
    "WarehouseId": ('setup."FgsWarehouse"', '"TenantId" = @TenantId AND "CompanyId" = @CompanyId AND "Id" = @Id AND "IsActive" = TRUE'),
    "VehicleId": ('setup."FgsVehicle"', '"TenantId" = @TenantId AND "CompanyId" = @CompanyId AND "Id" = @Id AND "IsActive" = TRUE'),
    "VehicleMaintenanceTypeId": ('glo."GloVehicleMaintenanceType"', '"Id" = @Id AND "IsActive" = TRUE'),
    "NextSalesPipelineStatusId": ('setup."FgsSalesPipelineStatus"', '"TenantId" = @TenantId AND "CompanyId" = @CompanyId AND "Id" = @Id AND "IsActive" = TRUE'),
}


def resolve_sort_field(cfg: EntityConfig) -> str | None:
    if cfg.sort_field:
        return cfg.sort_field
    if cfg.has_display_order:
        return "DisplayOrder"
    return None


def composite_exists_method(cfg: EntityConfig) -> str:
    if not cfg.unique_composite:
        return ""
    return "ExistsBy" + "And".join(cfg.unique_composite) + "Async"


def composite_exists_params(cfg: EntityConfig) -> str:
    if not cfg.unique_composite:
        return ""
    return ", ".join(f"string {lc(name)}" for name in cfg.unique_composite)


def composite_exists_args(cfg: EntityConfig) -> str:
    if not cfg.unique_composite:
        return ""
    return ", ".join(f"dto.{name}" for name in cfg.unique_composite)


def f(fields: list[Field]) -> list[Field]:
    return fields


ENTITIES: list[EntityConfig] = [
    EntityConfig(
        type_prefix="FgsBusinessType",
        plural_folder="FgsBusinessTypes",
        route="businesstypes",
        controller="BusinessTypesController",
        domain_entity="FgsBusinessType",
        table='setup."FgsBusinessType"',
        dbset="FgsBusinessTypes",
        display_name="business type",
        base="setup_tenant",
        code_field="Code",
        name_field="Name",
        has_display_order=True,
        fields=f([
            Field("Code", "string", 100, uppercase=True, in_lookup=True, in_list_filter=True),
            Field("Name", "string", 200, in_lookup=True, in_list_filter=True),
            Field("Description", "string?", required=False),
            Field("DisplayOrder", "short?", required=False, in_lookup=True),
        ]),
        search_columns=["Code", "Name", "Description"],
    ),
    EntityConfig(
        type_prefix="BillingCategory",
        plural_folder="BillingCategories",
        route="billingcategories",
        controller="BillingCategoriesController",
        domain_entity="FgsBillingCategory",
        table='setup."FgsBillingCategory"',
        dbset="FgsBillingCategories",
        display_name="billing category",
        base="setup_tenant",
        code_field="BillingCategoryType",
        name_field="BillingCategoryName",
        has_display_order=True,
        unique_code=False,
        unique_composite=("BillingCategoryType", "BillingCategoryName"),
        fields=f([
            Field("BillingCategoryType", "string", 2, uppercase=True, in_lookup=True, in_list_filter=True),
            Field("BillingCategoryName", "string", 100, in_lookup=True, in_list_filter=True),
            Field("Description", "string?", required=False),
            Field("DisplayOrder", "short?", required=False, in_lookup=True),
            Field("IsSystemDefined", "bool", default="false"),
            Field("ShowToFieldTech", "bool", default="false"),
            Field("AllowToPick", "bool", default="true"),
        ]),
        search_columns=["BillingCategoryType", "BillingCategoryName", "Description"],
    ),
    EntityConfig(
        type_prefix="JobTypeCategory",
        plural_folder="JobTypeCategories",
        route="jobtypecategories",
        controller="JobTypeCategoriesController",
        domain_entity="FgsJobTypeCategory",
        table='setup."FgsJobTypeCategory"',
        dbset="FgsJobTypeCategories",
        display_name="job type category",
        base="setup_tenant",
        code_field="CategoryCode",
        name_field="Name",
        has_display_order=True,
        fields=f([
            Field("CategoryCode", "string", 50, uppercase=True, in_lookup=True, in_list_filter=True),
            Field("Name", "string", 150, in_lookup=True, in_list_filter=True),
            Field("Description", "string?", required=False),
            Field("DisplayOrder", "short?", required=False, in_lookup=True),
        ]),
        search_columns=["CategoryCode", "Name", "Description"],
    ),
    EntityConfig(
        type_prefix="JobTypeSubCategory",
        plural_folder="JobTypeSubCategories",
        route="jobtypesubcategories",
        controller="JobTypeSubCategoriesController",
        domain_entity="FgsJobTypeSubCategory",
        table='setup."FgsJobTypeSubCategory"',
        dbset="FgsJobTypeSubCategories",
        display_name="job type subcategory",
        base="setup_tenant",
        code_field="SubCategoryCode",
        name_field="Name",
        has_display_order=True,
        fields=f([
            Field("SubCategoryCode", "string", 50, uppercase=True, in_lookup=True, in_list_filter=True),
            Field("Name", "string", 150, in_lookup=True, in_list_filter=True),
            Field("Description", "string?", required=False),
            Field("DisplayOrder", "short?", required=False, in_lookup=True),
        ]),
        search_columns=["SubCategoryCode", "Name", "Description"],
    ),
    EntityConfig(
        type_prefix="JobType",
        plural_folder="JobTypes",
        route="jobtypes",
        controller="JobTypesController",
        domain_entity="FgsJobType",
        table='setup."FgsJobType"',
        dbset="FgsJobTypes",
        display_name="job type",
        base="setup_tenant",
        code_field="JobTypeCode",
        name_field="TaskName",
        has_display_order=True,
        fields=f([
            Field("JobTypeCategoryId", "long", in_lookup=False, in_summary=True),
            Field("JobTypeSubCategoryId", "long?", required=False),
            Field("JobTypeCode", "string", 50, uppercase=True, in_lookup=True, in_list_filter=True),
            Field("TaskName", "string", 200, in_lookup=True, in_list_filter=True),
            Field("Description", "string?", required=False),
            Field("UsedFor", "string", 50),
            Field("Trade", "string?", 100, required=False),
            Field("EstimatedDurationMinutes", "int?", required=False),
            Field("BusinessUnit", "string?", 100, required=False),
            Field("Priority", "short", default="5", validator_min=1),
            Field("BackgroundColor", "string?", 20, required=False),
            Field("TextColor", "string?", 20, required=False),
            Field("ShowToFieldTech", "bool", default="true"),
            Field("ShowOnCustomerPortal", "bool", default="true"),
            Field("DisplayOrder", "short?", required=False, in_lookup=True),
        ]),
        fk_checks=[
            ("JobTypeCategoryId", "ExistsJobTypeCategoryIdAsync", "job type category"),
            ("JobTypeSubCategoryId", "ExistsJobTypeSubCategoryIdAsync", "job type subcategory"),
        ],
        search_columns=["JobTypeCode", "TaskName", "Description"],
    ),
    EntityConfig(
        type_prefix="LeadDisqualificationReason",
        plural_folder="LeadDisqualificationReasons",
        route="leaddisqualificationreasons",
        controller="LeadDisqualificationReasonsController",
        domain_entity="FgsLeadDisqualificationReason",
        table='setup."FgsLeadDisqualificationReason"',
        dbset="FgsLeadDisqualificationReasons",
        display_name="lead disqualification reason",
        base="lead_entity",
        code_field="ReasonCode",
        name_field="ReasonName",
        has_display_order=True,
        unique_name=True,
        fields=f([
            Field("ReasonCode", "string", 50, uppercase=True, in_lookup=True, in_list_filter=True),
            Field("ReasonName", "string", 100, in_lookup=True, in_list_filter=True),
            Field("Description", "string?", 255, required=False),
            Field("DisplayOrder", "short?", required=False, in_lookup=True),
            Field("IsSystem", "bool", default="false"),
        ]),
        search_columns=["ReasonCode", "ReasonName", "Description"],
    ),
    EntityConfig(
        type_prefix="LeadSource",
        plural_folder="LeadSources",
        route="leadsources",
        controller="LeadSourcesController",
        domain_entity="FgsLeadSource",
        table='setup."FgsLeadSource"',
        dbset="FgsLeadSources",
        display_name="lead source",
        base="lead_entity",
        code_field="SourceCode",
        name_field="SourceName",
        has_display_order=False,
        fields=f([
            Field("SourceCode", "string", 50, uppercase=True, in_lookup=True, in_list_filter=True),
            Field("SourceName", "string", 100, in_lookup=True, in_list_filter=True),
            Field("Description", "string?", 255, required=False),
        ]),
        search_columns=["SourceCode", "SourceName", "Description"],
    ),
    EntityConfig(
        type_prefix="LeadStatus",
        plural_folder="LeadStatuses",
        route="leadstatuses",
        controller="LeadStatusesController",
        domain_entity="FgsLeadStatus",
        table='setup."FgsLeadStatus"',
        dbset="FgsLeadStatuses",
        display_name="lead status",
        base="lead_entity",
        code_field="StatusCode",
        name_field="StatusName",
        has_display_order=True,
        unique_name=True,
        fields=f([
            Field("StatusCode", "string", 50, uppercase=True, in_lookup=True, in_list_filter=True),
            Field("StatusName", "string", 100, in_lookup=True, in_list_filter=True),
            Field("Description", "string?", 255, required=False),
            Field("DisplayOrder", "short?", required=False, in_lookup=True),
            Field("IsSystem", "bool", default="false"),
        ]),
        search_columns=["StatusCode", "StatusName", "Description"],
    ),
    EntityConfig(
        type_prefix="ResolutionCode",
        plural_folder="ResolutionCodes",
        route="resolutioncodes",
        controller="ResolutionCodesController",
        domain_entity="FgsResolutionCode",
        table='setup."FgsResolutionCode"',
        dbset="FgsResolutionCodes",
        display_name="resolution code",
        base="setup_tenant",
        code_field="ResolutionCode",
        name_field="ResolutionName",
        has_display_order=False,
        fields=f([
            Field("GloResolutionTypeId", "int", in_lookup=False),
            Field("ResolutionCode", "string", 50, uppercase=True, in_lookup=True, in_list_filter=True),
            Field("ResolutionName", "string", 200, in_lookup=True, in_list_filter=True),
            Field("IsMobileVisible", "bool", default="true"),
        ]),
        fk_checks=[
            ("GloResolutionTypeId", "ExistsGloResolutionTypeIdAsync", "resolution type"),
        ],
        search_columns=["ResolutionCode", "ResolutionName"],
    ),
]

NEW_ENTITIES: list[EntityConfig] = [
    EntityConfig(
        type_prefix="FgsSalesPipelineStatus",
        plural_folder="SalesPipelineStatuses",
        route="salespipelinestatuses",
        controller="SalesPipelineStatusesController",
        domain_entity="FgsSalesPipelineStatus",
        table='setup."FgsSalesPipelineStatus"',
        dbset="FgsSalesPipelineStatuses",
        display_name="sales pipeline status",
        base="lead_entity",
        code_field="StatusCode",
        name_field="StatusName",
        sort_field="DisplayOrder",
        unique_name=True,
        sales_applies_to_check=True,
        fields=f([
            Field("StatusCode", "string", 50, uppercase=True, in_lookup=True, in_list_filter=True),
            Field("StatusName", "string", 100, in_lookup=True, in_list_filter=True),
            Field("Description", "string?", 255, required=False),
            Field("DisplayOrder", "short", default="1", in_lookup=True),
            Field("IsSystem", "bool", default="false"),
            Field("AppliesToLead", "bool", default="false"),
            Field("AppliesToOpportunity", "bool", default="false"),
            Field("IsTerminal", "bool", default="false"),
            Field("AllowManualSelection", "bool", default="true"),
        ]),
        search_columns=["StatusCode", "StatusName", "Description"],
    ),
    EntityConfig(
        type_prefix="FgsSalesActivityType",
        plural_folder="SalesActivityTypes",
        route="salesactivitytypes",
        controller="SalesActivityTypesController",
        domain_entity="FgsSalesActivityType",
        table='setup."FgsSalesActivityType"',
        dbset="FgsSalesActivityTypes",
        display_name="sales activity type",
        base="lead_entity",
        code_field="ActivityTypeCode",
        name_field="ActivityTypeName",
        sort_field="DisplayOrder",
        unique_name=True,
        sales_applies_to_check=True,
        fields=f([
            Field("ActivityTypeCode", "string", 50, uppercase=True, in_lookup=True, in_list_filter=True),
            Field("ActivityTypeName", "string", 100, in_lookup=True, in_list_filter=True),
            Field("Description", "string?", 255, required=False),
            Field("DisplayOrder", "short", default="1", in_lookup=True),
            Field("IsSystem", "bool", default="false"),
            Field("AppliesToLead", "bool", default="true"),
            Field("AppliesToOpportunity", "bool", default="true"),
            Field("AllowManualSelection", "bool", default="true"),
        ]),
        search_columns=["ActivityTypeCode", "ActivityTypeName", "Description"],
    ),
    EntityConfig(
        type_prefix="FgsSalesDispositionReason",
        plural_folder="SalesDispositionReasons",
        route="salesdispositionreasons",
        controller="SalesDispositionReasonsController",
        domain_entity="FgsSalesDispositionReason",
        table='setup."FgsSalesDispositionReason"',
        dbset="FgsSalesDispositionReasons",
        display_name="sales disposition reason",
        base="lead_entity",
        code_field="DispositionReasonCode",
        name_field="DispositionReasonName",
        sort_field="DisplayOrder",
        unique_name=True,
        sales_applies_to_check=True,
        fields=f([
            Field("DispositionReasonCode", "string", 50, uppercase=True, in_lookup=True, in_list_filter=True),
            Field("DispositionReasonName", "string", 100, in_lookup=True, in_list_filter=True),
            Field("Description", "string?", 255, required=False),
            Field("DisplayOrder", "short", default="1", in_lookup=True),
            Field("IsSystem", "bool", default="false"),
            Field("AppliesToLead", "bool", default="false"),
            Field("AppliesToOpportunity", "bool", default="false"),
            Field("RequireComment", "bool", default="false"),
            Field("IsTerminal", "bool", default="true"),
            Field("AllowManualSelection", "bool", default="true"),
        ]),
        search_columns=["DispositionReasonCode", "DispositionReasonName", "Description"],
    ),
    EntityConfig(
        type_prefix="FgsSalesActivityOutcome",
        plural_folder="SalesActivityOutcomes",
        route="salesactivityoutcomes",
        controller="SalesActivityOutcomesController",
        domain_entity="FgsSalesActivityOutcome",
        table='setup."FgsSalesActivityOutcome"',
        dbset="FgsSalesActivityOutcomes",
        display_name="sales activity outcome",
        base="lead_entity",
        code_field="OutcomeCode",
        name_field="OutcomeName",
        sort_field="DisplayOrder",
        unique_name=True,
        sales_applies_to_check=True,
        fields=f([
            Field("OutcomeCode", "string", 50, uppercase=True, in_lookup=True, in_list_filter=True),
            Field("OutcomeName", "string", 100, in_lookup=True, in_list_filter=True),
            Field("Description", "string?", 255, required=False),
            Field("DisplayOrder", "short", default="1", in_lookup=True),
            Field("IsSystem", "bool", default="false"),
            Field("AppliesToLead", "bool", default="true"),
            Field("AppliesToOpportunity", "bool", default="true"),
            Field("NextSalesPipelineStatusId", "long?", required=False),
            Field("IsTerminal", "bool", default="false"),
            Field("RequireComment", "bool", default="false"),
            Field("AllowManualSelection", "bool", default="true"),
        ]),
        fk_checks=[
            ("NextSalesPipelineStatusId", "ExistsSalesPipelineStatusIdAsync", "sales pipeline status"),
        ],
        search_columns=["OutcomeCode", "OutcomeName", "Description"],
    ),
    EntityConfig(
        type_prefix="FgsSetupZone",
        plural_folder="SetupZones",
        route="zones",
        controller="ZonesController",
        domain_entity="FgsSetupZone",
        table='setup."FgsSetupZone"',
        dbset="FgsSetupZones",
        display_name="zone",
        base="setup_tenant",
        code_field="Code",
        name_field="Name",
        fields=f([
            Field("Code", "string", 100, uppercase=True, in_lookup=True, in_list_filter=True),
            Field("Name", "string", in_lookup=True, in_list_filter=True),
            Field("Description", "string?", required=False),
        ]),
        search_columns=["Code", "Name", "Description"],
    ),
    EntityConfig(
        type_prefix="FgsSetupTechSkillLevel",
        plural_folder="SetupTechSkillLevels",
        route="techskilllevels",
        controller="TechSkillLevelsController",
        domain_entity="FgsSetupTechSkillLevel",
        table='setup."FgsSetupTechSkillLevel"',
        dbset="FgsSetupTechSkillLevels",
        display_name="tech skill level",
        base="setup_tenant",
        code_field="Code",
        name_field="Name",
        sort_field="SortOrder",
        fields=f([
            Field("Code", "string", 100, uppercase=True, in_lookup=True, in_list_filter=True),
            Field("Name", "string", in_lookup=True, in_list_filter=True),
            Field("Description", "string?", required=False),
            Field("SortOrder", "int?", required=False, in_lookup=True),
        ]),
        search_columns=["Code", "Name", "Description"],
    ),
    EntityConfig(
        type_prefix="FgsSetupLaborRateType",
        plural_folder="SetupLaborRateTypes",
        route="laborratetypes",
        controller="LaborRateTypesController",
        domain_entity="FgsSetupLaborRateType",
        table='setup."FgsSetupLaborRateType"',
        dbset="FgsSetupLaborRateTypes",
        display_name="labor rate type",
        base="setup_tenant",
        code_field="Name",
        name_field="Name",
        sort_field="SortOrder",
        unique_code=False,
        unique_name=True,
        fields=f([
            Field("Name", "string", in_lookup=True, in_list_filter=True),
            Field("Description", "string?", required=False),
            Field("SortOrder", "int", default="0", in_lookup=True),
            Field("IsSystem", "bool", default="false"),
        ]),
        search_columns=["Name", "Description"],
    ),
    EntityConfig(
        type_prefix="FgsSetupTax",
        plural_folder="SetupTaxes",
        route="taxes",
        controller="TaxesController",
        domain_entity="FgsSetupTax",
        table='setup."FgsSetupTax"',
        dbset="FgsSetupTaxes",
        display_name="tax",
        base="setup_tenant",
        code_field="TaxCode",
        name_field="Name",
        fields=f([
            Field("TaxCode", "string", uppercase=True, in_lookup=True, in_list_filter=True),
            Field("Name", "string", in_lookup=True, in_list_filter=True),
            Field("IsExternalSystemRecord", "bool", default="false"),
            Field("ExternalSystemId", "string?", 200, required=False),
            Field("SyncToken", "string?", 100, required=False),
            Field("ShowTaxDetail", "bool", default="false"),
            Field("Description", "string?", required=False),
        ]),
        search_columns=["TaxCode", "Name", "Description"],
    ),
    EntityConfig(
        type_prefix="FgsSetupTaxAuthority",
        plural_folder="SetupTaxAuthorities",
        route="taxauthorities",
        controller="TaxAuthoritiesController",
        domain_entity="FgsSetupTaxAuthority",
        table='setup."FgsSetupTaxAuthority"',
        dbset="FgsSetupTaxAuthorities",
        display_name="tax authority",
        base="setup_tenant",
        code_field="Code",
        name_field="Name",
        fields=f([
            Field("Code", "string", uppercase=True, in_lookup=True, in_list_filter=True),
            Field("Name", "string", in_lookup=True, in_list_filter=True),
            Field("RegionCode", "string?", uppercase=True, required=False),
            Field("IsExternalSystemRecord", "bool", default="false"),
            Field("TaxPercent", "decimal", in_lookup=True, in_summary=True),
            Field("Description", "string?", required=False),
        ]),
        search_columns=["Code", "Name", "RegionCode", "Description"],
    ),
    EntityConfig(
        type_prefix="FgsSetupPostalCode",
        plural_folder="SetupPostalCodes",
        route="postalcodes",
        controller="PostalCodesController",
        domain_entity="FgsSetupPostalCode",
        table='setup."FgsSetupPostalCode"',
        dbset="FgsSetupPostalCodes",
        display_name="postal code",
        base="setup_tenant",
        code_field="PostalCode",
        name_field="PostalCode",
        fields=f([
            Field("PostalCode", "string", in_lookup=True, in_list_filter=True),
            Field("FgsSetupZoneId", "long?", required=False),
            Field("FgsSetupTaxId", "long?", required=False),
        ]),
        fk_checks=[
            ("FgsSetupZoneId", "ExistsZoneIdAsync", "zone"),
            ("FgsSetupTaxId", "ExistsTaxIdAsync", "tax"),
        ],
        search_columns=["PostalCode"],
    ),
    EntityConfig(
        type_prefix="FgsSetupPaymentMethod",
        plural_folder="SetupPaymentMethods",
        route="paymentmethods",
        controller="PaymentMethodsController",
        domain_entity="FgsSetupPaymentMethod",
        table='setup."FgsSetupPaymentMethod"',
        dbset="FgsSetupPaymentMethods",
        display_name="payment method",
        base="setup_tenant",
        code_field="DisplayName",
        name_field="DisplayName",
        sort_field="SortOrder",
        unique_code=False,
        unique_name=True,
        fields=f([
            Field("DisplayName", "string", in_lookup=True, in_list_filter=True),
            Field("SortOrder", "int", default="0", in_lookup=True),
            Field("IsMobileVisible", "bool", default="true"),
            Field("IsCustomerPortalVisible", "bool", default="true"),
        ]),
        search_columns=["DisplayName"],
    ),
    EntityConfig(
        type_prefix="FgsSetupPaymentTerm",
        plural_folder="SetupPaymentTerms",
        route="paymentterms",
        controller="PaymentTermsController",
        domain_entity="FgsSetupPaymentTerm",
        table='setup."FgsSetupPaymentTerm"',
        dbset="FgsSetupPaymentTerms",
        display_name="payment term",
        base="setup_tenant",
        code_field="Name",
        name_field="Name",
        unique_code=False,
        unique_name=True,
        fields=f([
            Field("Name", "string", in_lookup=True, in_list_filter=True),
            Field("DueDateMethod", "string"),
            Field("NumberOfDays", "int?", required=False),
            Field("IsAccountsReceivable", "bool", default="true"),
            Field("IsAccountsPayable", "bool", default="true"),
            Field("IsMobileVisible", "bool", default="true"),
        ]),
        search_columns=["Name", "DueDateMethod"],
    ),
    EntityConfig(
        type_prefix="FgsSetupDescription",
        plural_folder="SetupDescriptions",
        route="setupdescriptions",
        controller="SetupDescriptionsController",
        domain_entity="FgsSetupDescription",
        table='setup."FgsSetupDescription"',
        dbset="FgsSetupDescriptions",
        display_name="setup description",
        base="setup_tenant",
        code_field="DescriptionTypeCode",
        name_field="Body",
        sort_field="SortOrder",
        fields=f([
            Field("DescriptionTypeCode", "string", in_lookup=True, in_list_filter=True),
            Field("ShortNote", "string?", 30, required=False),
            Field("Body", "string", in_lookup=True),
            Field("FgsSetupTechTradeId", "long?", required=False),
            Field("SortOrder", "int", default="0", in_lookup=True),
        ]),
        fk_checks=[
            ("FgsSetupTechTradeId", "ExistsTechTradeIdAsync", "tech trade"),
        ],
        search_columns=["DescriptionTypeCode", "ShortNote", "Body"],
    ),
    EntityConfig(
        type_prefix="FgsSetupTimeSlot",
        plural_folder="SetupTimeSlots",
        route="timeslots",
        controller="TimeSlotsController",
        domain_entity="FgsSetupTimeSlot",
        table='setup."FgsSetupTimeSlot"',
        dbset="FgsSetupTimeSlots",
        display_name="time slot",
        base="setup_tenant",
        code_field="Code",
        name_field="Name",
        fields=f([
            Field("FgsSetupZoneId", "long?", required=False),
            Field("Code", "string", uppercase=True, in_lookup=True, in_list_filter=True),
            Field("Name", "string", in_lookup=True, in_list_filter=True),
            Field("BeginTime", "TimeSpan"),
            Field("EndTime", "TimeSpan"),
            Field("MarkTechArrivedLateAfter", "TimeSpan?", required=False),
            Field("MarkWorkOrderDelayedCompletionAfter", "TimeSpan?", required=False),
            Field("IsMobileVisible", "bool", default="true"),
            Field("IsCustomerPortalVisible", "bool", default="true"),
        ]),
        fk_checks=[
            ("FgsSetupZoneId", "ExistsZoneIdAsync", "zone"),
        ],
        search_columns=["Code", "Name"],
    ),
    EntityConfig(
        type_prefix="FgsSetupCommunicationTemplate",
        plural_folder="CommunicationTemplates",
        route="communication-templates",
        controller="CommunicationTemplatesController",
        domain_entity="FgsSetupCommunicationTemplate",
        table='setup."FgsSetupCommunicationTemplate"',
        dbset="FgsSetupCommunicationTemplates",
        display_name="communication template",
        base="nullable_tenant_entity",
        code_field="Code",
        name_field="Name",
        skip_controller=True,
        unique_code=False,
        unique_composite=("CommunicationChannel", "TemplateType", "Code"),
        fields=f([
            Field("TenantId", "long?", required=False, in_summary=False, in_lookup=False),
            Field("CompanyId", "long?", required=False, in_summary=False, in_lookup=False),
            Field("CommunicationChannel", "string", 25, in_lookup=True, in_list_filter=True),
            Field("TemplateType", "string", in_lookup=True, in_list_filter=True),
            Field("Code", "string", in_lookup=True, in_list_filter=True),
            Field("Name", "string", in_lookup=True, in_list_filter=True),
            Field("Subject", "string?", required=False),
            Field("Body", "string"),
            Field("IsMobileVisible", "bool", default="true"),
        ]),
        search_columns=["CommunicationChannel", "TemplateType", "Code", "Name", "Subject"],
    ),
    EntityConfig(
        type_prefix="FgsTag",
        plural_folder="Tags",
        route="tags",
        controller="TagsController",
        domain_entity="FgsTag",
        table='setup."FgsTag"',
        dbset="FgsTags",
        display_name="tag",
        base="tag_entity",
        code_field="TagCode",
        name_field="Name",
        unique_code=False,
        fields=f([
            Field("TagCode", "string?", 50, uppercase=True, required=False, in_lookup=True, in_list_filter=True),
            Field("Name", "string", 100, in_lookup=True, in_list_filter=True),
            Field("Description", "string?", required=False),
            Field("BackgroundColor", "string?", 20, required=False),
            Field("TextColor", "string?", 20, required=False),
            Field("IconFileId", "long?", required=False),
            Field("UsageCount", "int", in_create=False, in_update=False, in_patch=False, in_summary=True, default="0"),
        ]),
        search_columns=["TagCode", "Name", "Description"],
    ),
    EntityConfig(
        type_prefix="FgsWarehouse",
        plural_folder="Warehouses",
        route="warehouses",
        controller="WarehousesController",
        domain_entity="FgsWarehouse",
        table='setup."FgsWarehouse"',
        dbset="FgsWarehouses",
        display_name="warehouse",
        base="setup_tenant",
        code_field="WarehouseCode",
        name_field="Name",
        fields=f([
            Field("WarehouseCode", "string", 50, uppercase=True, in_lookup=True, in_list_filter=True),
            Field("Name", "string", 200, in_lookup=True, in_list_filter=True),
            Field("WarehouseType", "string", 30),
            Field("AddressId", "Guid?", required=False),
            Field("Description", "string?", required=False),
            Field("IsDefault", "bool", default="false"),
        ]),
        search_columns=["WarehouseCode", "Name", "Description"],
    ),
    EntityConfig(
        type_prefix="FgsVendor",
        plural_folder="Vendors",
        route="vendors",
        controller="VendorsController",
        domain_entity="FgsVendor",
        table='setup."FgsVendor"',
        dbset="FgsVendors",
        display_name="vendor",
        base="setup_tenant",
        code_field="VendorCode",
        name_field="Name",
        fields=f([
            Field("VendorCode", "string", 50, uppercase=True, in_lookup=True, in_list_filter=True),
            Field("Name", "string", 200, in_lookup=True, in_list_filter=True),
            Field("LegalName", "string?", 200, required=False),
            Field("VendorType", "string", 50),
            Field("PaymentTermId", "long?", required=False),
            Field("Email", "string?", 255, required=False),
            Field("PhoneNumber", "string?", 50, required=False),
            Field("MobileNumber", "string?", 50, required=False),
            Field("Website", "string?", 255, required=False),
            Field("TaxIdentificationNumber", "string?", 100, required=False),
            Field("LicenseNumber", "string?", 100, required=False),
            Field("InsurancePolicyNumber", "string?", 100, required=False),
            Field("Notes", "string?", required=False),
            Field("Is1099Eligible", "bool", default="false"),
        ]),
        fk_checks=[
            ("PaymentTermId", "ExistsPaymentTermIdAsync", "payment term"),
        ],
        search_columns=["VendorCode", "Name", "LegalName", "Email"],
    ),
    EntityConfig(
        type_prefix="FgsVehicle",
        plural_folder="Vehicles",
        route="vehicles",
        controller="VehiclesController",
        domain_entity="FgsVehicle",
        table='setup."FgsVehicle"',
        dbset="FgsVehicles",
        display_name="vehicle",
        base="setup_tenant",
        code_field="VIN",
        name_field="VIN",
        unique_code=False,
        fields=f([
            Field("WarehouseId", "long"),
            Field("OwnershipType", "string", 20, default="Owned"),
            Field("OwnershipCompany", "string?", 200, required=False),
            Field("Year", "short?", required=False),
            Field("Make", "string?", 100, required=False, in_lookup=True),
            Field("Model", "string?", 100, required=False, in_lookup=True),
            Field("Color", "string?", 50, required=False),
            Field("VIN", "string", 50, in_lookup=True, in_list_filter=True),
            Field("LicensePlate", "string?", 50, required=False),
            Field("LicensePlateState", "string?", 50, required=False),
            Field("PurchaseDate", "DateOnly?", required=False),
            Field("PurchasePrice", "decimal?", required=False),
            Field("PurchasedFrom", "string?", 200, required=False),
            Field("IsPurchasedNew", "bool?", required=False),
            Field("Notes", "string?", required=False),
        ]),
        fk_checks=[
            ("WarehouseId", "ExistsWarehouseIdAsync", "warehouse"),
        ],
        search_columns=["VIN", "Make", "Model", "LicensePlate"],
    ),
    EntityConfig(
        type_prefix="FgsVehicleMaintenance",
        plural_folder="VehicleMaintenances",
        route="vehiclemaintenances",
        controller="VehicleMaintenancesController",
        domain_entity="FgsVehicleMaintenance",
        table='setup."FgsVehicleMaintenance"',
        dbset="FgsVehicleMaintenances",
        display_name="vehicle maintenance",
        base="lead_entity",
        code_field="VehicleId",
        name_field="ServiceDate",
        unique_code=False,
        extra_list_filters=[("IsCompleted", "bool?"), ("VehicleId", "long?")],
        fields=f([
            Field("VehicleId", "long", in_lookup=True, in_summary=True),
            Field("VehicleMaintenanceTypeId", "int"),
            Field("ServiceDate", "DateOnly", in_lookup=True, in_summary=True),
            Field("MileageAtService", "int?", required=False),
            Field("ServiceProvider", "string?", 200, required=False),
            Field("InvoiceNumber", "string?", 100, required=False),
            Field("Cost", "decimal?", required=False),
            Field("NextServiceDate", "DateOnly?", required=False),
            Field("NextServiceMileage", "int?", required=False),
            Field("IsCompleted", "bool", default="true", in_summary=True),
            Field("Description", "string?", 500, required=False),
            Field("Notes", "string?", required=False),
        ]),
        fk_checks=[
            ("VehicleId", "ExistsVehicleIdAsync", "vehicle"),
            ("VehicleMaintenanceTypeId", "ExistsGloVehicleMaintenanceTypeIdAsync", "vehicle maintenance type"),
        ],
        search_columns=["ServiceProvider", "InvoiceNumber", "Description", "Notes"],
    ),
]

for e in ENTITIES + NEW_ENTITIES:
    e.abstractions_folder = e.abstractions_folder or e.plural_folder
    e.infra_folder = e.infra_folder or e.plural_folder


def write(path: Path, content: str) -> None:
    path.parent.mkdir(parents=True, exist_ok=True)
    path.write_text(content.replace("\r\n", "\n"), encoding="utf-8", newline="\n")
    print(f"  {path.relative_to(ROOT)}")


def base_fields(cfg: EntityConfig) -> str:
    return "long Id,\n    long TenantId,\n    long CompanyId,"


def dto_field_lines(cfg: EntityConfig, kind: str) -> list[str]:
    lines: list[str] = []
    if kind in ("summary", "detail", "lookup"):
        if kind != "lookup":
            if cfg.base == "nullable_tenant_entity":
                lines.extend(["long Id", "long? TenantId", "long? CompanyId"])
            else:
                lines.extend(["long Id", "long TenantId", "long CompanyId"])
        else:
            lines.append("long Id")
    skip_fields = {"TenantId", "CompanyId"} if cfg.base == "nullable_tenant_entity" else set()
    for fld in cfg.fields:
        if fld.name in skip_fields and kind in ("summary", "detail"):
            continue
        if kind == "summary" and not fld.in_summary:
            continue
        if kind == "lookup" and not fld.in_lookup:
            continue
        if kind == "create" and not fld.in_create:
            continue
        if kind == "update" and not fld.in_update:
            continue
        if kind == "patch":
            if not fld.in_patch:
                continue
            t = fld.cs_type if fld.cs_type.endswith("?") else fld.cs_type + "?"
        else:
            t = fld.cs_type
        lines.append(f"{t} {fld.name}")
    if kind in ("summary", "detail", "lookup"):
        pass
    elif kind == "patch":
        lines.append("bool? IsActive")
    if kind in ("summary", "detail"):
        lines.extend(["bool IsActive", "DateTimeOffset CreatedOn"])
        if kind == "detail":
            lines.extend(["string? CreatedBy", "DateTimeOffset? UpdatedOn", "string? UpdatedBy"])
        else:
            lines.append("DateTimeOffset? UpdatedOn")
    return lines


def record(name: str, fields: list[str]) -> str:
    body = ",\n    ".join(fields)
    return f"public sealed record {name}(\n    {body});"


def pluralize_display(cfg: EntityConfig) -> str:
    return cfg.plural_folder.replace("Fgs", "").replace("Setup", "")


def iface_name(cfg: EntityConfig) -> str:
    return f"I{cfg.type_prefix}ReadRepository"


def write_iface_name(cfg: EntityConfig) -> str:
    return f"I{cfg.type_prefix}WriteService"


def repo_class(cfg: EntityConfig) -> str:
    return f"{cfg.type_prefix}ReadRepository"


def write_class(cfg: EntityConfig) -> str:
    return f"{cfg.type_prefix}WriteService"


def sql_class(cfg: EntityConfig) -> str:
    return f"{cfg.type_prefix}Sql"


def dapper_prefix(cfg: EntityConfig) -> str:
    return cfg.type_prefix


def exists_methods(cfg: EntityConfig) -> str:
    lines = []
    p = cfg.type_prefix
    if cfg.unique_code:
        lines.append(f"""
    Task<bool> ExistsBy{cfg.code_field}Async(
        string {lc(cfg.code_field)},
        long? excludeId = null,
        CancellationToken cancellationToken = default);""")
    if cfg.unique_name:
        lines.append(f"""
    Task<bool> ExistsBy{cfg.name_field}Async(
        string {lc(cfg.name_field)},
        long? excludeId = null,
        CancellationToken cancellationToken = default);""")
    if cfg.unique_composite:
        method = composite_exists_method(cfg)
        params = composite_exists_params(cfg)
        lines.append(f"""
    Task<bool> {method}(
        {params},
        long? excludeId = null,
        CancellationToken cancellationToken = default);""")
    for fk_field, method, _ in cfg.fk_checks:
        cs = next(f.cs_type for f in cfg.fields if f.name == fk_field)
        lines.append(f"""
    Task<bool> {method}(
        {cs} id,
        CancellationToken cancellationToken = default);""")
    return "".join(lines)


def lc(name: str) -> str:
    return name[0].lower() + name[1:]


def generate_dtos(cfg: EntityConfig) -> None:
    pf = cfg.type_prefix
    ns = f"Fgs.Setup.Application.Features.{cfg.plural_folder}.Dtos"
    filter_names = [f.name for f in cfg.fields if f.in_list_filter]
    extra_names = {name for name, _ in cfg.extra_list_filters}
    filter_fields = ",\n    ".join([f"string? {name} = null" for name in filter_names if name not in extra_names])
    extra_bool_long = ",\n    ".join(
        f"{cs} {name} = null" if cs.endswith("?") else f"{cs}? {name} = null"
        for name, cs in cfg.extra_list_filters
    )
    all_filter_parts = [p for p in [filter_fields, extra_bool_long] if p]
    list_filter_body = ",\n    ".join(all_filter_parts)
    list_filters = (
        f"public sealed record {pf}ListFilters();"
        if not list_filter_body
        else f"public sealed record {pf}ListFilters(\n    {list_filter_body});"
    )
    content = f"""namespace {ns};

{record(f"{pf}SummaryDto", dto_field_lines(cfg, "summary"))}

{record(f"{pf}DetailDto", dto_field_lines(cfg, "detail"))}

{record(f"{pf}LookupDto", dto_field_lines(cfg, "lookup"))}

{record(f"{pf}CreateDto", dto_field_lines(cfg, "create"))}

{record(f"{pf}UpdateDto", dto_field_lines(cfg, "update"))}

{record(f"{pf}PatchDto", dto_field_lines(cfg, "patch"))}

{list_filters}
"""
    write(APP / "Features" / cfg.plural_folder / "Dtos" / f"{pf}Dtos.cs", content)


def generate_abstractions(cfg: EntityConfig) -> None:
    pf = cfg.type_prefix
    folder = cfg.abstractions_folder
    read = f"""using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.{cfg.plural_folder}.Dtos;

namespace Fgs.Setup.Application.Abstractions.{folder};

public interface {iface_name(cfg)}
{{
    Task<{pf}DetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<{pf}SummaryDto>> ListAsync(
        SetupListQuery query,
        {pf}ListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<{pf}LookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);
{exists_methods(cfg)}
}}
"""
    write_svc = f"""using Fgs.Setup.Application.Features.{cfg.plural_folder}.Dtos;

namespace Fgs.Setup.Application.Abstractions.{folder};

public interface {write_iface_name(cfg)}
{{
    Task<{pf}DetailDto> CreateAsync({pf}CreateDto dto, CancellationToken cancellationToken = default);

    Task<{pf}DetailDto> UpdateAsync(long id, {pf}UpdateDto dto, CancellationToken cancellationToken = default);

    Task<{pf}DetailDto> PatchAsync(long id, {pf}PatchDto dto, CancellationToken cancellationToken = default);

    Task<{pf}DetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}}
"""
    write(APP / "Abstractions" / folder / f"{iface_name(cfg)}.cs", read)
    write(APP / "Abstractions" / folder / f"{write_iface_name(cfg)}.cs", write_svc)


def detail_columns(cfg: EntityConfig) -> str:
    cols = ['"Id"', '"TenantId"', '"CompanyId"']
    cols += [f'"{f.name}"' for f in cfg.fields if f.in_summary or f.name not in [c.strip('"') for c in cols]]
    cols += ['"IsActive"', '"CreatedOn"', '"CreatedBy"', '"UpdatedOn"', '"UpdatedBy"']
    seen = set()
    out = []
    for c in cols:
        if c not in seen:
            seen.add(c)
            out.append(c)
    return ", ".join(out)


def summary_columns(cfg: EntityConfig) -> str:
    cols = ['"Id"', '"TenantId"', '"CompanyId"']
    for f in cfg.fields:
        if f.in_summary:
            cols.append(f'"{f.name}"')
    cols += ['"IsActive"', '"CreatedOn"', '"UpdatedOn"']
    return ", ".join(cols)


def lookup_columns(cfg: EntityConfig) -> str:
    cols = ['"Id"']
    for f in cfg.fields:
        if f.in_lookup:
            cols.append(f'"{f.name}"')
    return ", ".join(cols)


def allowed_sort(cfg: EntityConfig) -> list[str]:
    cols = ["Id", "CreatedOn", "IsActive"]
    sort_col = resolve_sort_field(cfg)
    if sort_col:
        cols.append(sort_col)
    cols += [f.name for f in cfg.fields if f.in_summary]
    return cols


def default_order_sql(cfg: EntityConfig) -> str:
    sort_col = resolve_sort_field(cfg)
    if sort_col:
        return f'ORDER BY \\"{sort_col}\\" {{dir}} NULLS LAST, \\"{cfg.name_field}\\" {{dir}}'
    return f'ORDER BY \\"{cfg.name_field}\\" {{dir}}'


def generate_sql(cfg: EntityConfig) -> None:
    sort_cols = list(dict.fromkeys(allowed_sort(cfg)))
    table_cs = cfg.table.replace('"', '\\"')
    default_order = default_order_sql(cfg)
    sort_col = resolve_sort_field(cfg)
    sort_col_check = sort_col or "DisplayOrder"
    content = f"""using Fgs.Foundation.CatalogCrud;

namespace Fgs.Setup.Infrastructure.{cfg.infra_folder};

internal static class {sql_class(cfg)}
{{
    public const string Table = "{table_cs}";

    public const string SelectDetailColumns = \"\"\"
        {detail_columns(cfg)}
        \"\"\";

    public const string SelectSummaryColumns = \"\"\"
        {summary_columns(cfg)}
        \"\"\";

    public const string SelectLookupColumns = \"\"\"
        {lookup_columns(cfg)}
        \"\"\";

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {{
        {", ".join(f'"{c}"' for c in sort_cols)}
    }};

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {{
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {{
            return $"{default_order}";
        }}

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return column.Equals("{sort_col_check}", StringComparison.OrdinalIgnoreCase)
            ? $"{default_order}"
            : $"ORDER BY \\"{{column}}\\" {{dir}}";
    }}
}}
"""
    write(INFRA / cfg.infra_folder / f"{sql_class(cfg)}.cs", content)


def normalize_code_expr(field: Field) -> str:
    if field.uppercase:
        return f"NormalizeCode(dto.{field.name})"
    return f"dto.{field.name}.Trim()"


def assign_field(field: Field, src: str = "dto") -> str:
    if field.uppercase and field.cs_type == "string?":
        return f"entity.{field.name} = string.IsNullOrWhiteSpace({src}.{field.name}) ? null : NormalizeCode({src}.{field.name});"
    if field.uppercase:
        return f'entity.{field.name} = NormalizeCode({src}.{field.name});'
    if field.cs_type == "string?":
        return f"entity.{field.name} = string.IsNullOrWhiteSpace({src}.{field.name}) ? null : {src}.{field.name}.Trim();"
    if field.cs_type in ("short?", "int?"):
        return f"entity.{field.name} = {src}.{field.name} ?? entity.{field.name};"
    if field.cs_type.startswith("string"):
        return f"entity.{field.name} = {src}.{field.name}.Trim();"
    return f"entity.{field.name} = {src}.{field.name};"


def patch_assign(field: Field) -> str:
    if field.cs_type.endswith("?"):
        if field.cs_type in ("bool?", "short?", "int?", "long?", "decimal?", "Guid?"):
            return f"""        if (dto.{field.name}.HasValue)
        {{
            entity.{field.name} = dto.{field.name}.Value;
        }}
"""
        if field.cs_type in ("DateOnly?", "TimeSpan?"):
            return f"""        if (dto.{field.name}.HasValue)
        {{
            entity.{field.name} = dto.{field.name}.Value;
        }}
"""
        if field.cs_type == "string?":
            patch_body = assign_field(field, "dto").replace(f"entity.{field.name} = ", "").strip()
            return f"""        if (dto.{field.name} is not null)
        {{
            entity.{field.name} = {patch_body};
        }}
"""
        body = assign_field(field, "dto").replace(f"entity.{field.name} = ", "").strip()
        return f"""        if (dto.{field.name} is not null)
        {{
            entity.{field.name} = {body};
        }}
"""
    if field.cs_type in ("short", "bool", "int", "long", "decimal", "DateOnly", "TimeSpan"):
        return f"""        if (dto.{field.name}.HasValue)
        {{
            entity.{field.name} = dto.{field.name}.Value;
        }}
"""
    body = assign_field(field, "dto").replace(f"entity.{field.name} = ", "").strip()
    return f"""        if (dto.{field.name} is not null)
        {{
            entity.{field.name} = {body};
        }}
"""


def unique_violation_msg(cfg: EntityConfig) -> str:
    if cfg.unique_composite:
        return f"A {cfg.display_name} with the same type and name already exists."
    if cfg.unique_name:
        return f"A {cfg.display_name} with the same code already exists."
    return f"A {cfg.display_name} with the same code already exists."


def entity_init_field(f: Field) -> str:
    if f.uppercase and f.cs_type == "string?":
        return f"{f.name} = string.IsNullOrWhiteSpace(dto.{f.name}) ? null : NormalizeCode(dto.{f.name})"
    if f.uppercase:
        return f"{f.name} = NormalizeCode(dto.{f.name})"
    if f.cs_type == "string?":
        return f"{f.name} = string.IsNullOrWhiteSpace(dto.{f.name}) ? null : dto.{f.name}.Trim()"
    if f.cs_type in ("short?", "int?"):
        return f"{f.name} = dto.{f.name} ?? 1"
    if f.cs_type.startswith("string"):
        return f"{f.name} = dto.{f.name}.Trim()"
    return f"{f.name} = dto.{f.name}"


def write_service_create_body(cfg: EntityConfig) -> str:
    init_fields = ", ".join(entity_init_field(f) for f in cfg.fields if f.in_create)
    if cfg.base == "tag_entity":
        return f"""        var entity = new {cfg.domain_entity}
        {{
            {init_fields}
        }};

        entity.NormalizedName = dto.Name.Trim().ToUpperInvariant();
        _auditHelper.StampForCreate(entity);"""
    if cfg.base == "nullable_tenant_entity":
        return f"""        var entity = new {cfg.domain_entity}
        {{
            {init_fields}
        }};

        _auditHelper.StampForCreate(entity, dto.TenantId, dto.CompanyId);"""
    return f"""        var entity = new {cfg.domain_entity}
        {{
            {init_fields}
        }};

        _auditHelper.StampForCreate(entity);"""


def generate_write_service(cfg: EntityConfig) -> None:
    pf = cfg.type_prefix
    update_assigns = "\n        ".join(assign_field(f) for f in cfg.fields if f.in_update)
    if cfg.base == "tag_entity":
        update_block = f"""        {update_assigns}
        entity.NormalizedName = dto.Name.Trim().ToUpperInvariant();

        _auditHelper.StampForUpdate(entity);"""
    else:
        update_block = f"""        {update_assigns}

        _auditHelper.StampForUpdate(entity);"""
    patch_assigns = "".join(patch_assign(field) for field in cfg.fields if field.in_patch)
    entity_field_names = [
        f.name for f in cfg.fields
        if not (cfg.base == "nullable_tenant_entity" and f.name in ("TenantId", "CompanyId"))
    ]
    map_fields = ",\n            ".join(["entity.Id", "entity.TenantId", "entity.CompanyId"] + [f"entity.{name}" for name in entity_field_names] + [
        "entity.IsActive", "entity.CreatedOn", "entity.CreatedBy", "entity.UpdatedOn", "entity.UpdatedBy"
    ])
    not_found = f"{cfg.display_name.title()} '{{id}}' was not found."
    create_body = write_service_create_body(cfg)
    content = f"""using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.{cfg.abstractions_folder};
using Fgs.Setup.Application.Features.{cfg.plural_folder}.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.{cfg.infra_folder};

public sealed class {write_class(cfg)} : {write_iface_name(cfg)}
{{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public {write_class(cfg)}(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {{
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }}

    public async Task<{pf}DetailDto> CreateAsync(
        {pf}CreateDto dto,
        CancellationToken cancellationToken = default)
    {{
{create_body}
        await _context.{cfg.dbset}.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }}

    public async Task<{pf}DetailDto> UpdateAsync(
        long id,
        {pf}UpdateDto dto,
        CancellationToken cancellationToken = default)
    {{
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"{not_found}");

{update_block}
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }}

    public async Task<{pf}DetailDto> PatchAsync(
        long id,
        {pf}PatchDto dto,
        CancellationToken cancellationToken = default)
    {{
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"{not_found}");

{patch_assigns}
        if (dto.IsActive.HasValue)
        {{
            entity.IsActive = dto.IsActive.Value;
        }}

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }}

    public async Task<{pf}DetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {{
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"{not_found}");

        if (entity.IsActive)
        {{
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }}

        return MapToDetail(entity);
    }}

    private async Task<{cfg.domain_entity}?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.{cfg.dbset}.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {{
        try
        {{
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }}
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {{
            throw new InvalidOperationException("{unique_violation_msg(cfg)}", ex);
        }}
    }}

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();

    private static {pf}DetailDto MapToDetail({cfg.domain_entity} entity) =>
        new(
            {map_fields});
}}
"""
    write(INFRA / cfg.infra_folder / f"{write_class(cfg)}.cs", content)


def filter_where(cfg: EntityConfig) -> str:
    parts = []
    for f in cfg.fields:
        if not f.in_list_filter:
            continue
        if f.uppercase:
            parts.append(f"""        if (!string.IsNullOrWhiteSpace(filters.{f.name}))
        {{
            where.Add("\\"{f.name}\\" = @{f.name}");
        }}
""")
        else:
            parts.append(f"""        if (!string.IsNullOrWhiteSpace(filters.{f.name}))
        {{
            where.Add("\\"{f.name}\\" ILIKE @{f.name}");
        }}
""")
    for name, cs in cfg.extra_list_filters:
        if cs == "bool?":
            parts.append(f"""        if (filters.{name}.HasValue)
        {{
            where.Add("\\"{name}\\" = @{name}");
        }}
""")
        elif cs == "long?":
            parts.append(f"""        if (filters.{name}.HasValue)
        {{
            where.Add("\\"{name}\\" = @{name}");
        }}
""")
    return "".join(parts)


def filter_params(cfg: EntityConfig) -> str:
    lines = []
    for f in cfg.fields:
        if not f.in_list_filter:
            continue
        if f.uppercase:
            lines.append(f"            {f.name} = filters.{f.name}?.Trim().ToUpperInvariant(),")
        else:
            lines.append(
                f"            {f.name} = string.IsNullOrWhiteSpace(filters.{f.name}) ? null : $\"%{{filters.{f.name}.Trim()}}%\","
            )
    for name, cs in cfg.extra_list_filters:
        lines.append(f"            {name} = filters.{name},")
    return "\n".join(lines)


def search_clause(cfg: EntityConfig) -> str:
    cols = cfg.search_columns or [cfg.code_field, cfg.name_field]
    parts = " OR ".join(f'\\"{c}\\" ILIKE @Search' for c in cols)
    return f"({parts})"


def exists_impl(cfg: EntityConfig) -> str:
    chunks = []
    if cfg.unique_code:
        cf = cfg.code_field
        chunks.append(f"""
    public async Task<bool> ExistsBy{cf}Async(
        string {lc(cf)},
        long? excludeId = null,
        CancellationToken cancellationToken = default)
    {{
        var (tenantId, companyId) = ResolveTenantScope();
        var sql = $\"\"\"
            SELECT EXISTS(
                SELECT 1
                FROM {{{sql_class(cfg)}.Table}}
                WHERE "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND "{cf}" = @{cf}
                  {{(excludeId.HasValue ? "AND \\"Id\\" <> @ExcludeId" : string.Empty)}}
            )
            \"\"\";

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                sql,
                new
                {{
                    TenantId = tenantId,
                    CompanyId = companyId,
                    {cf} = {lc(cf)}.Trim().ToUpperInvariant(),
                    ExcludeId = excludeId
                }},
                cancellationToken: cancellationToken));
    }}""")
    if cfg.unique_name:
        nf = cfg.name_field
        chunks.append(f"""
    public async Task<bool> ExistsBy{nf}Async(
        string {lc(nf)},
        long? excludeId = null,
        CancellationToken cancellationToken = default)
    {{
        var (tenantId, companyId) = ResolveTenantScope();
        var sql = $\"\"\"
            SELECT EXISTS(
                SELECT 1
                FROM {{{sql_class(cfg)}.Table}}
                WHERE "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND "IsActive" = TRUE
                  AND LOWER("{nf}") = LOWER(@{nf})
                  {{(excludeId.HasValue ? "AND \\"Id\\" <> @ExcludeId" : string.Empty)}}
            )
            \"\"\";

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                sql,
                new
                {{
                    TenantId = tenantId,
                    CompanyId = companyId,
                    {nf} = {lc(nf)}.Trim(),
                    ExcludeId = excludeId
                }},
                cancellationToken: cancellationToken));
    }}""")
    if cfg.unique_composite:
        method = composite_exists_method(cfg)
        param_names = ", ".join(f"string {lc(name)}" for name in cfg.unique_composite)
        param_assigns_parts = []
        for name in cfg.unique_composite:
            field_obj = next((f for f in cfg.fields if f.name == name), None)
            if field_obj and field_obj.uppercase:
                param_assigns_parts.append(f"{name} = {lc(name)}.Trim().ToUpperInvariant()")
            else:
                param_assigns_parts.append(f"{name} = {lc(name)}.Trim()")
        param_assigns = ",\n                    ".join(param_assigns_parts)
        where_parts = " AND ".join(f'"{name}" = @{name}' for name in cfg.unique_composite)
        chunks.append(f"""
    public async Task<bool> {method}(
        {param_names},
        long? excludeId = null,
        CancellationToken cancellationToken = default)
    {{
        var (tenantId, companyId) = ResolveTenantScope();
        var sql = $\"\"\"
            SELECT EXISTS(
                SELECT 1
                FROM {{{sql_class(cfg)}.Table}}
                WHERE "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND {where_parts}
                  {{(excludeId.HasValue ? "AND \\"Id\\" <> @ExcludeId" : string.Empty)}}
            )
            \"\"\";

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                sql,
                new
                {{
                    TenantId = tenantId,
                    CompanyId = companyId,
                    {param_assigns},
                    ExcludeId = excludeId
                }},
                cancellationToken: cancellationToken));
    }}""")
    for fk_field, method, _ in cfg.fk_checks:
        cs = next(f.cs_type for f in cfg.fields if f.name == fk_field)
        if fk_field in FK_EXISTS_TABLES:
            table, where = FK_EXISTS_TABLES[fk_field]
        else:
            where = '"TenantId" = @TenantId AND "CompanyId" = @CompanyId AND "Id" = @Id AND "IsActive" = TRUE'
            table = cfg.table
        id_param = "Id = id" if fk_field != "GloResolutionTypeId" else "Id = id"
        chunks.append(f"""
    public async Task<bool> {method}(
        {cs} id,
        CancellationToken cancellationToken = default)
    {{
        var (tenantId, companyId) = ResolveTenantScope();
        var sql = $\"\"\"
            SELECT EXISTS(
                SELECT 1
                FROM {table}
                WHERE {where}
            )
            \"\"\";

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                sql,
                new {{ TenantId = tenantId, CompanyId = companyId, Id = id }},
                cancellationToken: cancellationToken));
    }}""")
    return "".join(chunks)


def generate_read_repository(cfg: EntityConfig) -> None:
    pf = cfg.type_prefix
    sort_col = resolve_sort_field(cfg)
    if sort_col:
        lookup_order = f'ORDER BY "{sort_col}" ASC NULLS LAST, "{cfg.name_field}" ASC'
    else:
        lookup_order = f'ORDER BY "{cfg.name_field}" ASC'
    content = f"""using Dapper;
using Fgs.Foundation.CatalogCrud;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Persistence;
using Fgs.Setup.Application.Abstractions.{cfg.abstractions_folder};
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.{cfg.plural_folder}.Dtos;

namespace Fgs.Setup.Infrastructure.{cfg.infra_folder};

internal sealed class {repo_class(cfg)} : {iface_name(cfg)}
{{
    private readonly ISetupReadConnectionFactory _connectionFactory;
    private readonly ITenantContextAccessor _tenantContextAccessor;

    public {repo_class(cfg)}(
        ISetupReadConnectionFactory connectionFactory,
        ITenantContextAccessor tenantContextAccessor)
    {{
        _connectionFactory = connectionFactory;
        _tenantContextAccessor = tenantContextAccessor;
    }}

    public async Task<{pf}DetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {{
        var (tenantId, companyId) = ResolveTenantScope();
        var sql = $\"\"\"
            SELECT {{{sql_class(cfg)}.SelectDetailColumns}}
            FROM {{{sql_class(cfg)}.Table}}
            WHERE "Id" = @Id
              AND "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
            \"\"\";

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var row = await connection.QueryFirstOrDefaultAsync<{pf}DetailRow>(
            new CommandDefinition(sql, new {{ Id = id, TenantId = tenantId, CompanyId = companyId }}, cancellationToken: cancellationToken));

        return row?.ToDto();
    }}

    public async Task<PagedResult<{pf}SummaryDto>> ListAsync(
        SetupListQuery query,
        {pf}ListFilters filters,
        CancellationToken cancellationToken = default)
    {{
        var (tenantId, companyId) = ResolveTenantScope();
        var paging = query.ToPagedQuery();
        var page = Math.Max(1, paging.Page);
        var pageSize = Math.Clamp(paging.PageSize, 1, 200);
        var offset = (page - 1) * pageSize;

        var where = new List<string>
        {{
            "\\"TenantId\\" = @TenantId",
            "\\"CompanyId\\" = @CompanyId"
        }};

        if (paging.IsActive.HasValue)
        {{
            where.Add("\\"IsActive\\" = @IsActive");
        }}

{filter_where(cfg)}
        if (!string.IsNullOrWhiteSpace(paging.Search))
        {{
            where.Add(
                "{search_clause(cfg)}");
        }}

        var whereClause = string.Join(" AND ", where);
        var orderBy = {sql_class(cfg)}.ResolveOrderBy(paging.SortBy, paging.SortDirection);

        var sql = $\"\"\"
            SELECT {{{sql_class(cfg)}.SelectSummaryColumns}}
            FROM {{{sql_class(cfg)}.Table}}
            WHERE {{whereClause}}
            {{orderBy}}
            LIMIT @PageSize OFFSET @Offset;

            SELECT COUNT(*)
            FROM {{{sql_class(cfg)}.Table}}
            WHERE {{whereClause}};
            \"\"\";

        var parameters = new
        {{
            TenantId = tenantId,
            CompanyId = companyId,
            IsActive = paging.IsActive,
{filter_params(cfg)}
            Search = string.IsNullOrWhiteSpace(paging.Search) ? null : $"%{{paging.Search.Trim()}}%",
            PageSize = pageSize,
            Offset = offset
        }};

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

        var rows = (await multi.ReadAsync<{pf}SummaryRow>()).ToList();
        var totalCount = await multi.ReadSingleAsync<int>();

        return new PagedResult<{pf}SummaryDto>(
            rows.Select(r => r.ToDto()).ToList(),
            page,
            pageSize,
            totalCount);
    }}

    public async Task<IReadOnlyList<{pf}LookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {{
        var (tenantId, companyId) = ResolveTenantScope();
        var activeFilter = activeOnly ? "AND \\"IsActive\\" = TRUE" : string.Empty;
        var sql = $\"\"\"
            SELECT {{{sql_class(cfg)}.SelectLookupColumns}}
            FROM {{{sql_class(cfg)}.Table}}
            WHERE "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
              {{activeFilter}}
            {lookup_order}
            \"\"\";

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<{pf}LookupRow>(
            new CommandDefinition(sql, new {{ TenantId = tenantId, CompanyId = companyId }}, cancellationToken: cancellationToken));

        return rows.Select(r => r.ToDto()).ToList();
    }}
{exists_impl(cfg)}

    private (long TenantId, long CompanyId) ResolveTenantScope()
    {{
        if (_tenantContextAccessor.Current is {{ IsResolved: true }} context)
        {{
            return (context.TenantId, context.CompanyId);
        }}

        throw new InvalidOperationException("Tenant context is not resolved.");
    }}
}}
"""
    write(INFRA / cfg.infra_folder / f"{repo_class(cfg)}.cs", content)


def row_props(cfg: EntityConfig, kind: str) -> list[str]:
    props = []
    if kind != "lookup":
        if cfg.base == "nullable_tenant_entity":
            props.extend(["public long Id { get; set; }", "public long? TenantId { get; set; }", "public long? CompanyId { get; set; }"])
        else:
            props.extend(["public long Id { get; set; }", "public long TenantId { get; set; }", "public long CompanyId { get; set; }"])
    else:
        props.append("public long Id { get; set; }")
    skip_fields = {"TenantId", "CompanyId"} if cfg.base == "nullable_tenant_entity" else set()
    for f in cfg.fields:
        if f.name in skip_fields and kind in ("summary", "detail"):
            continue
        if kind == "summary" and not f.in_summary:
            continue
        if kind == "lookup" and not f.in_lookup:
            continue
        cs = f.cs_type.replace("?", "")
        if "?" in f.cs_type:
            props.append(f"public {cs}? {f.name} {{ get; set; }}")
        else:
            props.append(f"public {cs} {f.name} {{ get; set; }}" + ("" if f.required else " = null!;"))
    if kind != "lookup":
        props.extend([
            "public bool IsActive { get; set; }",
            "public DateTimeOffset CreatedOn { get; set; }",
        ])
        if kind == "detail":
            props.extend([
                "public string? CreatedBy { get; set; }",
                "public DateTimeOffset? UpdatedOn { get; set; }",
                "public string? UpdatedBy { get; set; }",
            ])
        else:
            props.append("public DateTimeOffset? UpdatedOn { get; set; }")
    return props


def dto_ctor_args(cfg: EntityConfig, prefix: str, kind: str) -> str:
    def ref(name: str) -> str:
        return f"{prefix}.{name}" if prefix else name

    args = []
    if kind != "lookup":
        if cfg.base == "nullable_tenant_entity":
            args.extend([ref("Id"), ref("TenantId"), ref("CompanyId")])
        else:
            args.extend([ref("Id"), ref("TenantId"), ref("CompanyId")])
    else:
        args.append(ref("Id"))
    skip_fields = {"TenantId", "CompanyId"} if cfg.base == "nullable_tenant_entity" else set()
    for f in cfg.fields:
        if f.name in skip_fields and kind in ("summary", "detail"):
            continue
        if kind == "summary" and not f.in_summary:
            continue
        if kind == "lookup" and not f.in_lookup:
            continue
        args.append(ref(f.name))
    if kind != "lookup":
        args.extend([ref("IsActive"), ref("CreatedOn")])
        if kind == "detail":
            args.extend([ref("CreatedBy"), ref("UpdatedOn"), ref("UpdatedBy")])
        else:
            args.append(ref("UpdatedOn"))
    return ",\n            ".join(args)


def generate_dapper_rows(cfg: EntityConfig) -> None:
    pf = cfg.type_prefix
    summary_args = dto_ctor_args(cfg, "", "summary")
    detail_args = dto_ctor_args(cfg, "", "detail")
    lookup_args = dto_ctor_args(cfg, "", "lookup")
    content = f"""using Fgs.Setup.Application.Features.{cfg.plural_folder}.Dtos;

namespace Fgs.Setup.Infrastructure.{cfg.infra_folder};

internal sealed class {pf}SummaryRow
{{
{chr(10).join('    ' + p for p in row_props(cfg, 'summary'))}

    public {pf}SummaryDto ToDto() =>
        new(
            {summary_args});
}}

internal sealed class {pf}DetailRow
{{
{chr(10).join('    ' + p for p in row_props(cfg, 'detail'))}

    public {pf}DetailDto ToDto() =>
        new(
            {detail_args});
}}

internal sealed class {pf}LookupRow
{{
{chr(10).join('    ' + p for p in row_props(cfg, 'lookup'))}

    public {pf}LookupDto ToDto() => new({lookup_args});
}}
"""
    write(INFRA / cfg.infra_folder / f"{pf}DapperRows.cs", content)


def field_validation(field: Field, cfg: EntityConfig, mode: str) -> str:
    prop = f"x => x.Dto.{field.name}"
    rules = []
    patch_when = f".When(x => x.Dto.{field.name} is not null)" if mode == "patch" and field.cs_type == "string?" else ""
    patch_when_val = f".When(x => x.Dto.{field.name}.HasValue)" if mode == "patch" and field.cs_type in ("short?", "int?", "long?", "bool?", "decimal?", "Guid?", "DateOnly?", "TimeSpan?") else patch_when

    if mode != "patch" and field.required:
        if field.cs_type.startswith("string"):
            rules.append(f"        RuleFor({prop}).NotEmpty();")
    elif mode == "patch" and field.required and field.cs_type.startswith("string") and not field.cs_type.endswith("?"):
        rules.append(f"        RuleFor({prop}).NotEmpty(){patch_when};")

    if field.max_length:
        rules.append(f"        RuleFor({prop}).MaximumLength({field.max_length}){patch_when_val or patch_when};")

    if field.uppercase:
        if mode == "patch":
            rules.append(
                f'        RuleFor({prop}).Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("{field.name} must be uppercase."){patch_when};'
            )
        else:
            rules.append(
                f'        RuleFor({prop}).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("{field.name} must be uppercase.");'
            )

    if field.validator_min is not None and field.cs_type in ("short", "short?"):
        rules.append(
            f"        RuleFor({prop}).GreaterThanOrEqualTo((short){field.validator_min}){patch_when_val};"
            if mode == "patch"
            else f"        RuleFor({prop}).GreaterThanOrEqualTo((short){field.validator_min});"
        )

    if field.name == cfg.code_field and cfg.unique_code:
        ex = "command.Id" if mode != "create" else "null"
        code_arg = f"code!," if mode == "patch" else "code,"
        patch_unique_when = patch_when if mode == "patch" else ""
        rules.append(f"""        RuleFor({prop}).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsBy{field.name}Async({code_arg} {ex}, cancellationToken))
            .WithMessage("A {cfg.display_name} with this code already exists."){patch_unique_when};""")

    if field.name == cfg.name_field and cfg.unique_name:
        ex = "command.Id" if mode != "create" else "null"
        name_arg = f"name!," if mode == "patch" else "name,"
        patch_unique_when = patch_when if mode == "patch" else ""
        rules.append(f"""        RuleFor({prop}).MustAsync(async (command, name, cancellationToken) =>
                !await readRepository.ExistsBy{field.name}Async({name_arg} {ex}, cancellationToken))
            .WithMessage("An active {cfg.display_name} with this name already exists."){patch_unique_when};""")

    if cfg.unique_composite and field.name == cfg.unique_composite[0]:
        method = composite_exists_method(cfg)
        args = composite_exists_args(cfg)
        ex = "command.Id" if mode != "create" else "null"
        rules.append(f"""        RuleFor(x => x.Dto).MustAsync(async (command, dto, cancellationToken) =>
                !await readRepository.{method}({args}, {ex}, cancellationToken))
            .WithMessage("A {cfg.display_name} with this combination already exists.");""")

    for fk_field, method, label in cfg.fk_checks:
        if field.name == fk_field:
            if mode == "patch":
                rules.append(f"""        RuleFor({prop}).MustAsync(async (command, value, cancellationToken) =>
                !value.HasValue || await readRepository.{method}(value.Value, cancellationToken))
            .WithMessage("The specified {label} was not found.").When(x => x.Dto.{fk_field}.HasValue);""")
            elif "?" in field.cs_type:
                rules.append(f"""        RuleFor({prop}).MustAsync(async (command, value, cancellationToken) =>
                !value.HasValue || await readRepository.{method}(value.Value, cancellationToken))
            .WithMessage("The specified {label} was not found.");""")
            else:
                rules.append(f"""        RuleFor({prop}).MustAsync(async (command, value, cancellationToken) =>
                await readRepository.{method}(value, cancellationToken))
            .WithMessage("The specified {label} was not found.");""")

    sort_col = resolve_sort_field(cfg)
    if sort_col and field.name == sort_col:
        if field.cs_type in ("short?", "int?"):
            rules.append(
                f"        RuleFor({prop}).GreaterThanOrEqualTo((short)0).When(x => x.Dto.{sort_col}.HasValue);"
                if field.cs_type == "short?"
                else f"        RuleFor({prop}).GreaterThanOrEqualTo(0).When(x => x.Dto.{sort_col}.HasValue);"
            )
        elif field.cs_type in ("short", "int") and mode == "patch":
            rules.append(
                f"        RuleFor({prop}).GreaterThanOrEqualTo((short)0).When(x => x.Dto.{sort_col}.HasValue);"
                if field.cs_type == "short"
                else f"        RuleFor({prop}).GreaterThanOrEqualTo(0).When(x => x.Dto.{sort_col}.HasValue);"
            )
        elif field.cs_type == "short":
            rules.append(f"        RuleFor({prop}).GreaterThanOrEqualTo((short)0);")
        elif field.cs_type == "int":
            rules.append(f"        RuleFor({prop}).GreaterThanOrEqualTo(0);")

    return "\n".join(rules)


def extra_validator_rules(cfg: EntityConfig, mode: str) -> str:
    rules = []
    if cfg.sales_applies_to_check:
        if mode == "patch":
            rules.append("""        RuleFor(x => x.Dto).Must(dto =>
                (!dto.AppliesToLead.HasValue && !dto.AppliesToOpportunity.HasValue)
                || dto.AppliesToLead == true
                || dto.AppliesToOpportunity == true)
            .WithMessage("At least one of AppliesToLead or AppliesToOpportunity must be true.");""")
        else:
            rules.append("""        RuleFor(x => x.Dto).Must(dto => dto.AppliesToLead || dto.AppliesToOpportunity)
            .WithMessage("At least one of AppliesToLead or AppliesToOpportunity must be true.");""")
    if cfg.type_prefix == "FgsSetupTaxAuthority":
        rules.append("""        RuleFor(x => x.Dto.TaxPercent).InclusiveBetween(0m, 100m);""")
    if cfg.type_prefix == "FgsSetupTimeSlot":
        rules.append("""        RuleFor(x => x.Dto).Must(dto => dto.EndTime > dto.BeginTime)
            .WithMessage("EndTime must be greater than BeginTime.");""")
    return "\n".join(rules)


def generate_validators(cfg: EntityConfig) -> None:
    pf = cfg.type_prefix
    repo = iface_name(cfg)
    create_rules = "\n".join(field_validation(f, cfg, "create") for f in cfg.fields if f.in_create)
    create_rules += "\n" + extra_validator_rules(cfg, "create") if extra_validator_rules(cfg, "create") else create_rules
    update_rules = "\n".join(
        ["        RuleFor(x => x.Id).GreaterThan(0);"]
        + [field_validation(f, cfg, "update") for f in cfg.fields if f.in_update]
    )
    update_extra = extra_validator_rules(cfg, "update")
    if update_extra:
        update_rules += "\n" + update_extra
    patch_rules = "\n".join(
        ["        RuleFor(x => x.Id).GreaterThan(0);"]
        + [field_validation(f, cfg, "patch") for f in cfg.fields if f.in_patch]
    )
    patch_extra = extra_validator_rules(cfg, "patch")
    if patch_extra:
        patch_rules += "\n" + patch_extra
    content = f"""using Fgs.Setup.Application.Abstractions.{cfg.abstractions_folder};
using Fgs.Setup.Application.Features.{cfg.plural_folder}.Commands.Create{pf};
using Fgs.Setup.Application.Features.{cfg.plural_folder}.Commands.Patch{pf};
using Fgs.Setup.Application.Features.{cfg.plural_folder}.Commands.Update{pf};
using FluentValidation;

namespace Fgs.Setup.Application.Features.{cfg.plural_folder}.Validators;

public sealed class Create{pf}CommandValidator : AbstractValidator<Create{pf}Command>
{{
    public Create{pf}CommandValidator({repo} readRepository)
    {{
{create_rules}
    }}
}}

public sealed class Update{pf}CommandValidator : AbstractValidator<Update{pf}Command>
{{
    public Update{pf}CommandValidator({repo} readRepository)
    {{
{update_rules}
    }}
}}

public sealed class Patch{pf}CommandValidator : AbstractValidator<Patch{pf}Command>
{{
    public Patch{pf}CommandValidator({repo} readRepository)
    {{
{patch_rules}
    }}
}}
"""
    write(APP / "Features" / cfg.plural_folder / "Validators" / f"{pf}Validators.cs", content)


def cache_invalidation_block(cfg: EntityConfig, indent: str = "            ") -> str:
    route = cfg.route
    return f"""{indent}var tenantScope = tenantContextAccessor.Current;
{indent}if (tenantScope?.IsResolved == true)
{indent}{{
{indent}    await cache.RemoveByPrefixAsync(
{indent}        CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "{route}"),
{indent}        cancellationToken);
{indent}}}"""


def cmd_handler(action: str, cfg: EntityConfig) -> str:
    pf = cfg.type_prefix
    svc = write_iface_name(cfg)
    folder = f"{action}{pf}"
    if action == "Create":
        ok_log = f'logger.LogInformation("Created {cfg.display_name} {{Id}} with code {{{cfg.code_field}}}", result.Id, result.{cfg.code_field});'
        err_log = f'logger.LogError(ex, "Failed to create {cfg.display_name}");'
    else:
        ok_log = f'logger.LogInformation("{action}d {cfg.display_name} {{Id}}", result.Id);'
        err_log = f'logger.LogError(ex, "Failed to {action.lower()} {cfg.display_name} {{Id}}", request.Id);'
    invalidation = cache_invalidation_block(cfg)
    return f"""using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.CatalogCrud;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.{cfg.abstractions_folder};
using Fgs.Setup.Application.Features.{cfg.plural_folder}.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.{cfg.plural_folder}.Commands.{folder};

public sealed class {action}{pf}CommandHandler(
    {svc} writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<{action}{pf}CommandHandler> logger)
    : IRequestHandler<{action}{pf}Command, ApiResponse<{pf}DetailDto>>
{{
    public async Task<ApiResponse<{pf}DetailDto>> Handle(
        {action}{pf}Command request,
        CancellationToken cancellationToken)
    {{
        try
        {{
            var result = await writeService.{action}Async({", ".join(["request.Dto"] if action == "Create" else ["request.Id", "request.Dto"] if action in ("Update", "Patch") else ["request.Id"])}, cancellationToken);
            {ok_log}
{invalidation}
            return ApiResponse<{pf}DetailDto>.Ok(result{", ApiStatusCodes.Created" if action == "Create" else ""});
        }}
        catch (Exception ex)
        {{
            {err_log}
            return CatalogCrudExceptionMapper.MapException<{pf}DetailDto>(ex);
        }}
    }}
}}
"""


def generate_commands(cfg: EntityConfig) -> None:
    pf = cfg.type_prefix
    for action in ["Create", "Update", "Patch", "Delete"]:
        folder = f"{action}{pf}"
        if action == "Delete":
            cmd = f"""using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.{cfg.plural_folder}.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.{cfg.plural_folder}.Commands.{folder};

public sealed record Delete{pf}Command(long Id)
    : IRequest<ApiResponse<{pf}DetailDto>>;
"""
            handler = f"""using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.CatalogCrud;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.{cfg.abstractions_folder};
using Fgs.Setup.Application.Features.{cfg.plural_folder}.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.{cfg.plural_folder}.Commands.{folder};

public sealed class Delete{pf}CommandHandler(
    {write_iface_name(cfg)} writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<Delete{pf}CommandHandler> logger)
    : IRequestHandler<Delete{pf}Command, ApiResponse<{pf}DetailDto>>
{{
    public async Task<ApiResponse<{pf}DetailDto>> Handle(
        Delete{pf}Command request,
        CancellationToken cancellationToken)
    {{
        try
        {{
            var result = await writeService.DeleteAsync(request.Id, cancellationToken);
            logger.LogInformation("Soft-deleted {cfg.display_name} {{Id}}", result.Id);
{cache_invalidation_block(cfg)}
            return ApiResponse<{pf}DetailDto>.Ok(result);
        }}
        catch (Exception ex)
        {{
            logger.LogError(ex, "Failed to delete {cfg.display_name} {{Id}}", request.Id);
            return CatalogCrudExceptionMapper.MapException<{pf}DetailDto>(ex);
        }}
    }}
}}
"""
        else:
            dto = f"{pf}CreateDto"
            if action == "Update":
                dto = f"{pf}UpdateDto"
            elif action == "Patch":
                dto = f"{pf}PatchDto"
            params = "(long Id, " + dto + " Dto)" if action in ("Update", "Patch") else f"({dto} Dto)"
            cmd = f"""using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.{cfg.plural_folder}.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.{cfg.plural_folder}.Commands.{folder};

public sealed record {action}{pf}Command{params}
    : IRequest<ApiResponse<{pf}DetailDto>>;
"""
            if action == "Create":
                call = "request.Dto, cancellationToken"
            else:
                call = "request.Id, request.Dto, cancellationToken"
            handler = cmd_handler(action, cfg)
        write(APP / "Features" / cfg.plural_folder / "Commands" / folder / f"{action}{pf}Command.cs", cmd)
        write(APP / "Features" / cfg.plural_folder / "Commands" / folder / f"{action}{pf}CommandHandler.cs", handler)


def generate_queries(cfg: EntityConfig) -> None:
    pf = cfg.type_prefix
    queries = [
        ("GetById", f"Get{pf}ById", f"long Id", f"{pf}DetailDto?", "GetByIdAsync(request.Id", f"{cfg.display_name.title()} '{{request.Id}}' was not found."),
        ("List", f"List{cfg.plural_folder}", f"SetupListQuery Query, {pf}ListFilters Filters", f"PagedResult<{pf}SummaryDto>", "ListAsync(request.Query, request.Filters", None),
        ("ListActive", f"ListActive{cfg.plural_folder}", f"int Page = 1, int PageSize = 25, string? SortBy = null, SortDirection SortDirection = SortDirection.Asc, string? Search = null, {pf}ListFilters? Filters = null", f"PagedResult<{pf}SummaryDto>", "special", None),
        ("Lookup", f"Lookup{cfg.plural_folder}", "bool ActiveOnly = true", f"IReadOnlyList<{pf}LookupDto>", "LookupAsync(request.ActiveOnly", None),
    ]
    for kind, name, params, ret_inner, call, nf in queries:
        qfile = APP / "Features" / cfg.plural_folder / "Queries" / name / f"{name}Query.cs"
        hfile = APP / "Features" / cfg.plural_folder / "Queries" / name / f"{name}QueryHandler.cs"
        if kind == "ListActive":
            qcontent = f"""using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.{cfg.plural_folder}.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.{cfg.plural_folder}.Queries.{name};

public sealed record {name}Query(
    {params})
    : IRequest<ApiResponse<{ret_inner}>>;
"""
            hcontent = f"""using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.CatalogCrud;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.{cfg.abstractions_folder};
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.{cfg.plural_folder}.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.{cfg.plural_folder}.Queries.{name};

public sealed class {name}QueryHandler(
    {iface_name(cfg)} readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<{name}Query, ApiResponse<{ret_inner}>>
{{
    public async Task<ApiResponse<{ret_inner}>> Handle(
        {name}Query request,
        CancellationToken cancellationToken)
    {{
        try
        {{
            var tenantScope = tenantContextAccessor.Current;
            if (tenantScope?.IsResolved == true)
            {{
                var segment = CacheKeys.ListActiveSegment(
                    request.Page,
                    request.PageSize,
                    request.SortBy,
                    request.SortDirection.ToString(),
                    request.Search,
                    CacheKeys.Fingerprint(request.Filters));

                var cacheKey = CacheKeys.Build(
                    tenantScope.TenantId,
                    tenantScope.CompanyId,
                    "{cfg.route}",
                    segment);

                var cached = await cache.GetOrSetAsync(
                    cacheKey,
                    async () =>
                    {{
                        var query = new SetupListQuery(
                            request.Page,
                            request.PageSize,
                            request.SortBy,
                            request.SortDirection,
                            request.Search,
                            IsActive: true);

                        return await readRepository.ListAsync(
                            query,
                            request.Filters ?? new {pf}ListFilters(),
                            cancellationToken);
                    }},
                    cancellationToken: cancellationToken);

                return ApiResponse<{ret_inner}>.Ok(cached!);
            }}

            var listQuery = new SetupListQuery(
                request.Page,
                request.PageSize,
                request.SortBy,
                request.SortDirection,
                request.Search,
                IsActive: true);

            var result = await readRepository.ListAsync(
                listQuery,
                request.Filters ?? new {pf}ListFilters(),
                cancellationToken);

            return ApiResponse<{ret_inner}>.Ok(result);
        }}
        catch (Exception ex)
        {{
            return CatalogCrudExceptionMapper.MapException<{ret_inner}>(ex);
        }}
    }}
}}
"""
        elif kind == "GetById":
            qcontent = f"""using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.{cfg.plural_folder}.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.{cfg.plural_folder}.Queries.{name};

public sealed record {name}Query({params})
    : IRequest<ApiResponse<{pf}DetailDto>>;
"""
            hcontent = f"""using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.CatalogCrud;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.{cfg.abstractions_folder};
using Fgs.Setup.Application.Features.{cfg.plural_folder}.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.{cfg.plural_folder}.Queries.{name};

public sealed class {name}QueryHandler(
    {iface_name(cfg)} readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<{name}Query, ApiResponse<{pf}DetailDto>>
{{
    public async Task<ApiResponse<{pf}DetailDto>> Handle(
        {name}Query request,
        CancellationToken cancellationToken)
    {{
        try
        {{
            var tenantScope = tenantContextAccessor.Current;
            if (tenantScope?.IsResolved == true)
            {{
                var cacheKey = CacheKeys.Build(
                    tenantScope.TenantId,
                    tenantScope.CompanyId,
                    "{cfg.route}",
                    request.Id.ToString());

                var cached = await cache.GetAsync<{pf}DetailDto>(cacheKey, cancellationToken);
                if (cached is not null)
                {{
                    return ApiResponse<{pf}DetailDto>.Ok(cached);
                }}

                var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
                if (result is null)
                {{
                    return ApiResponse<{pf}DetailDto>.Fail(
                        [$"{nf}"],
                        ApiStatusCodes.NotFound);
                }}

                await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
                return ApiResponse<{pf}DetailDto>.Ok(result);
            }}

            var uncached = await readRepository.GetByIdAsync(request.Id, cancellationToken);
            if (uncached is null)
            {{
                return ApiResponse<{pf}DetailDto>.Fail(
                    [$"{nf}"],
                    ApiStatusCodes.NotFound);
            }}

            return ApiResponse<{pf}DetailDto>.Ok(uncached);
        }}
        catch (Exception ex)
        {{
            return CatalogCrudExceptionMapper.MapException<{pf}DetailDto>(ex);
        }}
    }}
}}
"""
        elif kind == "Lookup":
            qcontent = f"""using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.{cfg.plural_folder}.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.{cfg.plural_folder}.Queries.{name};

public sealed record {name}Query({params})
    : IRequest<ApiResponse<{ret_inner}>>;
"""
            hcontent = f"""using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.CatalogCrud;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.{cfg.abstractions_folder};
using Fgs.Setup.Application.Features.{cfg.plural_folder}.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.{cfg.plural_folder}.Queries.{name};

public sealed class {name}QueryHandler(
    {iface_name(cfg)} readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<{name}Query, ApiResponse<{ret_inner}>>
{{
    public async Task<ApiResponse<{ret_inner}>> Handle(
        {name}Query request,
        CancellationToken cancellationToken)
    {{
        try
        {{
            var tenantScope = tenantContextAccessor.Current;
            if (tenantScope?.IsResolved == true)
            {{
                var cacheKey = CacheKeys.Build(
                    tenantScope.TenantId,
                    tenantScope.CompanyId,
                    "{cfg.route}",
                    CacheKeys.LookupSegment(request.ActiveOnly));

                var result = await cache.GetOrSetAsync(
                    cacheKey,
                    () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
                    cancellationToken: cancellationToken);

                return ApiResponse<{ret_inner}>.Ok(result ?? Array.Empty<{pf}LookupDto>());
            }}

            var uncached = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
            return ApiResponse<{ret_inner}>.Ok(uncached);
        }}
        catch (Exception ex)
        {{
            return CatalogCrudExceptionMapper.MapException<{ret_inner}>(ex);
        }}
    }}
}}
"""
        else:
            qcontent = f"""using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.{cfg.plural_folder}.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.{cfg.plural_folder}.Queries.{name};

public sealed record {name}Query(
    {params})
    : IRequest<ApiResponse<{ret_inner}>>;
"""
            hcontent = f"""using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.{cfg.abstractions_folder};
using Fgs.Setup.Application.Features.{cfg.plural_folder}.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.{cfg.plural_folder}.Queries.{name};

public sealed class {name}QueryHandler({iface_name(cfg)} readRepository)
    : IRequestHandler<{name}Query, ApiResponse<{ret_inner}>>
{{
    public async Task<ApiResponse<{ret_inner}>> Handle(
        {name}Query request,
        CancellationToken cancellationToken)
    {{
        try
        {{
            var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
            return ApiResponse<{ret_inner}>.Ok(result);
        }}
        catch (Exception ex)
        {{
            return CatalogCrudExceptionMapper.MapException<{ret_inner}>(ex);
        }}
    }}
}}
"""
        write(qfile, qcontent)
        write(hfile, hcontent)


def controller_filter_params(cfg: EntityConfig) -> str:
    lines = []
    for f in cfg.fields:
        if f.in_list_filter:
            camel = lc(f.name)
            lines.append(f'        [FromQuery] string? {camel} = null,')
    for name, cs in cfg.extra_list_filters:
        camel = lc(name)
        if cs == "bool?":
            lines.append(f'        [FromQuery] bool? {camel} = null,')
        elif cs == "long?":
            lines.append(f'        [FromQuery] long? {camel} = null,')
    return "\n".join(lines)


def controller_filter_args(cfg: EntityConfig) -> str:
    args = [lc(f.name) for f in cfg.fields if f.in_list_filter]
    args += [lc(name) for name, _ in cfg.extra_list_filters]
    if not args:
        return f"new {cfg.type_prefix}ListFilters()"
    return f"new {cfg.type_prefix}ListFilters({', '.join(args)})"


def generate_controller(cfg: EntityConfig) -> None:
    if cfg.skip_controller:
        print(f"  Skipping controller (merge manually): {cfg.controller}")
        return
    pf = cfg.type_prefix
    imports = "\n".join(
        f"using Fgs.Setup.Application.Features.{cfg.plural_folder}.Commands.{a}{pf};"
        for a in ["Create", "Delete", "Patch", "Update"]
    ) + "\n" + "\n".join(
        f"using Fgs.Setup.Application.Features.{cfg.plural_folder}.Queries.{q};"
        for q in [f"Get{pf}ById", f"List{cfg.plural_folder}", f"ListActive{cfg.plural_folder}", f"Lookup{cfg.plural_folder}"]
    )
    content = f"""using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
{imports}
using Fgs.Setup.Application.Features.{cfg.plural_folder}.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Setup.API.Controllers;

/// <summary>
/// Tenant-scoped {cfg.display_name} catalog management.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("{cfg.route}")]
[Produces("application/json")]
public sealed class {cfg.controller}(IMediator mediator) : ControllerBase
{{
    [HttpGet("{{id:long}}")]
    [ProducesResponseType(typeof(ApiResponse<{pf}DetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {{
        var response = await mediator.Send(new Get{pf}ByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }}

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<{pf}SummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = true,
{controller_filter_params(cfg)}
        CancellationToken cancellationToken = default)
    {{
        var response = await mediator.Send(
            new List{cfg.plural_folder}Query(
                new SetupListQuery(page, pageSize, sortBy, sortDirection, search, isActive),
                {controller_filter_args(cfg)}),
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }}

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<{pf}LookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup(
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {{
        var response = await mediator.Send(new Lookup{cfg.plural_folder}Query(activeOnly), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }}

    [HttpGet("active")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<{pf}SummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListActive(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
{controller_filter_params(cfg)}
        CancellationToken cancellationToken = default)
    {{
        var response = await mediator.Send(
            new ListActive{cfg.plural_folder}Query(
                page,
                pageSize,
                sortBy,
                sortDirection,
                search,
                {controller_filter_args(cfg)}),
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }}

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<{pf}DetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] {pf}CreateDto request,
        CancellationToken cancellationToken)
    {{
        var response = await mediator.Send(new Create{pf}Command(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }}

    [HttpPut("{{id:long}}")]
    [ProducesResponseType(typeof(ApiResponse<{pf}DetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] {pf}UpdateDto request,
        CancellationToken cancellationToken)
    {{
        var response = await mediator.Send(new Update{pf}Command(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }}

    [HttpPatch("{{id:long}}")]
    [ProducesResponseType(typeof(ApiResponse<{pf}DetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] {pf}PatchDto request,
        CancellationToken cancellationToken)
    {{
        var response = await mediator.Send(new Patch{pf}Command(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }}

    [HttpDelete("{{id:long}}")]
    [ProducesResponseType(typeof(ApiResponse<{pf}DetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {{
        var response = await mediator.Send(new Delete{pf}Command(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }}
}}
"""
    write(API / "Controllers" / f"{cfg.controller}.cs", content)


def sample_value(field: Field, cfg: EntityConfig | None = None) -> str:
    if cfg and cfg.sales_applies_to_check and field.name == "AppliesToLead":
        return "true"
    if field.uppercase:
        return '"TEST"'
    if field.cs_type == "bool":
        return "true" if field.default != "false" else "false"
    if field.cs_type == "short":
        return "5"
    if field.cs_type == "short?":
        return "1"
    if field.cs_type == "int?":
        return "60"
    if field.cs_type == "long":
        return "1"
    if field.cs_type == "long?":
        if field.name in ("TenantId", "CompanyId"):
            return "10L" if field.name == "TenantId" else "20L"
        return "null"
    if field.cs_type == "int":
        return "1"
    if field.cs_type == "decimal":
        return "10.5m"
    if field.cs_type == "decimal?":
        return "10.5m"
    if field.cs_type == "DateOnly":
        return "DateOnly.FromDateTime(DateTime.UtcNow)"
    if field.cs_type == "DateOnly?":
        return "null"
    if field.cs_type == "TimeSpan":
        if field.name == "EndTime":
            return "TimeSpan.FromHours(17)"
        return "TimeSpan.FromHours(8)"
    if field.cs_type == "TimeSpan?":
        return "null"
    if field.cs_type == "Guid?":
        return "null"
    if field.cs_type.startswith("string"):
        if field.name == "CommunicationChannel":
            return '"Email"'
        if field.max_length:
            return f'"{field.name[:field.max_length]}"'
        return f'"{field.name} value"'
    return "null"


def test_detail_ctor_args(cfg: EntityConfig) -> str:
    args = ["1"]
    if cfg.base == "nullable_tenant_entity":
        args.extend(["10L", "20L"])
    else:
        args.extend(["10", "20"])
    skip_fields = {"TenantId", "CompanyId"} if cfg.base == "nullable_tenant_entity" else set()
    for field in cfg.fields:
        if field.name in skip_fields:
            continue
        args.append(sample_value(field, cfg))
    args.extend(["true", "DateTimeOffset.UtcNow", '"seed"', "null", '"seed"'])
    return ", ".join(args)


def code_field_meta(cfg: EntityConfig) -> Field | None:
    return next((f for f in cfg.fields if f.name == cfg.code_field), None)


def sample_create_args(cfg: EntityConfig) -> str:
    return ", ".join(sample_value(f, cfg) for f in cfg.fields if f.in_create)


def generate_tests(cfg: EntityConfig) -> None:
    pf = cfg.type_prefix
    create_args = sample_create_args(cfg)
    code_meta = code_field_meta(cfg)
    has_string_code = code_meta is not None and code_meta.cs_type.startswith("string")
    code_tests = ""
    if has_string_code and code_meta.required:
        missing_parts = []
        for f in cfg.fields:
            if not f.in_create:
                continue
            if f.name == cfg.code_field:
                missing_parts.append('""')
            else:
                missing_parts.append(sample_value(f, cfg))
        missing_args = ", ".join(missing_parts)
        code_tests = f"""
    [Fact]
    public async Task CreateValidator_When{cfg.code_field}Missing_HasValidationError()
    {{
        var validator = new Create{pf}CommandValidator(_readRepository.Object);
        var command = new Create{pf}Command(new {pf}CreateDto({missing_args}));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.{cfg.code_field}");
    }}
"""
        if code_meta.uppercase:
            code_tests += f"""
    [Fact]
    public async Task CreateValidator_When{cfg.code_field}NotUppercase_HasValidationError()
    {{
        var validator = new Create{pf}CommandValidator(_readRepository.Object);
        var args = new {pf}CreateDto({create_args});
        var command = new Create{pf}Command(args with {{ {cfg.code_field} = "test" }});

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.{cfg.code_field}");
    }}
"""
    unique_setup = ""
    if cfg.unique_code:
        unique_setup += f"""
        _readRepository
            .Setup(r => r.ExistsBy{cfg.code_field}Async("TEST", 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);"""
    if cfg.unique_composite:
        method = composite_exists_method(cfg)
        unique_setup += f"""
        _readRepository
            .Setup(r => r.{method}(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);""" if len(cfg.unique_composite) == 3 else f"""
        _readRepository
            .Setup(r => r.{method}(It.IsAny<string>(), It.IsAny<string>(), 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);"""
    name_setup = (
        f"""
        _readRepository
            .Setup(r => r.ExistsBy{cfg.name_field}Async(It.IsAny<string>(), 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);"""
        if cfg.unique_name
        else ""
    )
    fk_setup = ""
    for fk_field, method, _ in cfg.fk_checks:
        cs = next(f.cs_type for f in cfg.fields if f.name == fk_field).replace("?", "")
        fk_setup += f"""
        _readRepository
            .Setup(r => r.{method}(It.IsAny<{cs}>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);"""
    vcontent = f"""using Fgs.Setup.Application.Abstractions.{cfg.abstractions_folder};
using Fgs.Setup.Application.Features.{cfg.plural_folder}.Commands.Create{pf};
using Fgs.Setup.Application.Features.{cfg.plural_folder}.Commands.Patch{pf};
using Fgs.Setup.Application.Features.{cfg.plural_folder}.Commands.Update{pf};
using Fgs.Setup.Application.Features.{cfg.plural_folder}.Dtos;
using Fgs.Setup.Application.Features.{cfg.plural_folder}.Validators;
using Moq;

namespace Fgs.Setup.Tests.{cfg.plural_folder};

public sealed class {pf}ValidatorTests
{{
    private readonly Mock<{iface_name(cfg)}> _readRepository = new();
{code_tests}
    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {{
{unique_setup}{name_setup}{fk_setup}
        var validator = new Update{pf}CommandValidator(_readRepository.Object);
        var command = new Update{pf}Command(5, new {pf}UpdateDto({create_args}));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }}
}}
"""
    write(TESTS / cfg.plural_folder / f"{pf}ValidatorTests.cs", vcontent)

    ccontent = f"""using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Fgs.Setup.Application.Features.{cfg.plural_folder}.Commands.Create{pf};
using Fgs.Setup.Application.Features.{cfg.plural_folder}.Commands.Delete{pf};
using Fgs.Setup.Application.Features.{cfg.plural_folder}.Commands.Update{pf};
using Fgs.Setup.Application.Features.{cfg.plural_folder}.Dtos;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Common.Time;
using Fgs.Setup.Infrastructure.Database;
using Fgs.Setup.Infrastructure.{cfg.infra_folder};
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.Setup.Tests.{cfg.plural_folder};

public sealed class {pf}CommandHandlerTests
{{
    private const long TenantId = 10;
    private const long CompanyId = 20;

    [Fact]
    public async Task CreateHandler_CreatesWithAuditFields()
    {{
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();
        var handler = new Create{pf}CommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<Create{pf}CommandHandler>.Instance);

        var response = await handler.Handle(
            new Create{pf}Command(new {pf}CreateDto({create_args})),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data!.IsActive.Should().BeTrue();
        response.Data.TenantId.Should().Be(TenantId);
        response.Data.CompanyId.Should().Be(CompanyId);
        cache.Verify(
            c => c.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(TenantId, CompanyId, "{cfg.route}"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }}

    [Fact]
    public async Task DeleteHandler_SoftDeletes()
    {{
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();
        var createHandler = new Create{pf}CommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<Create{pf}CommandHandler>.Instance);
        var deleteHandler = new Delete{pf}CommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<Delete{pf}CommandHandler>.Instance);

        var created = await createHandler.Handle(
            new Create{pf}Command(new {pf}CreateDto({create_args})),
            CancellationToken.None);
        created.Success.Should().BeTrue();

        var response = await deleteHandler.Handle(
            new Delete{pf}Command(created.Data!.Id),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.IsActive.Should().BeFalse();
    }}

    private static ITenantContextAccessor CreateTenantContextAccessor() =>
        new TestTenantContextAccessor
        {{
            Current = new TenantContext {{ TenantId = TenantId, CompanyId = CompanyId, IsResolved = true }}
        }};

    private static {write_class(cfg)} CreateWriteService(FgsSetupDbContext context)
    {{
        var userContext = new Mock<IFgsUserContext>();
        userContext.SetupGet(x => x.TenantId).Returns(TenantId);
        userContext.SetupGet(x => x.CompanyId).Returns(CompanyId);
        userContext.SetupGet(x => x.UserId).Returns(Guid.Parse("11111111-1111-1111-1111-111111111111"));

        var tenantAccessor = new TestTenantContextAccessor
        {{
            Current = new TenantContext {{ TenantId = TenantId, CompanyId = CompanyId, IsResolved = true }}
        }};

        var auditHelper = new SetupEntityAuditHelper(
            userContext.Object,
            tenantAccessor,
            new DateTimeProvider());
        var unitOfWork = new EfUnitOfWork<FgsSetupDbContext>(context);
        return new {write_class(cfg)}(context, unitOfWork, auditHelper);
    }}

    private static async Task<FgsSetupDbContext> CreateContextAsync()
    {{
        var accessor = new TestTenantContextAccessor
        {{
            Current = new TenantContext {{ TenantId = TenantId, CompanyId = CompanyId, IsResolved = true }}
        }};

        var options = new DbContextOptionsBuilder<FgsSetupDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        var context = new FgsSetupDbContext(options, accessor);
        await context.Database.EnsureCreatedAsync();
        return context;
    }}

    private sealed class TestTenantContextAccessor : ITenantContextAccessor
    {{
        public ITenantContext? Current {{ get; set; }}
    }}
}}
"""
    write(TESTS / cfg.plural_folder / f"{pf}CommandHandlerTests.cs", ccontent)

    qcontent = f"""using Fgs.Contracts.Api;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.CatalogCrud;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.{cfg.abstractions_folder};
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.{cfg.plural_folder}.Dtos;
using Fgs.Setup.Application.Features.{cfg.plural_folder}.Queries.Get{pf}ById;
using Fgs.Setup.Application.Features.{cfg.plural_folder}.Queries.List{cfg.plural_folder};
using Moq;

namespace Fgs.Setup.Tests.{cfg.plural_folder};

public sealed class {pf}QueryHandlerTests
{{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {{
        var detail = new {pf}DetailDto({test_detail_ctor_args(cfg)});

        var readRepository = new Mock<{iface_name(cfg)}>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext {{ TenantId = 10, CompanyId = 20, IsResolved = true }});

        var handler = new Get{pf}ByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new Get{pf}ByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
        readRepository.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }}

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {{
        var readRepository = new Mock<{iface_name(cfg)}>();
        readRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync(({pf}DetailDto?)null);

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext {{ TenantId = 10, CompanyId = 20, IsResolved = true }});

        var handler = new Get{pf}ByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new Get{pf}ByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }}

    [Fact]
    public async Task List_ReturnsPagedResult()
    {{
        var readRepository = new Mock<{iface_name(cfg)}>();
        readRepository
            .Setup(r => r.ListAsync(It.IsAny<SetupListQuery>(), It.IsAny<{pf}ListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<{pf}SummaryDto>([], 1, 25, 0));

        var handler = new List{cfg.plural_folder}QueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new List{cfg.plural_folder}Query(new SetupListQuery(), new {pf}ListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
    }}
}}
"""
    write(TESTS / cfg.plural_folder / f"{pf}QueryHandlerTests.cs", qcontent)


def generate_entity(cfg: EntityConfig) -> None:
    print(f"\n=== {cfg.type_prefix} ===")
    generate_dtos(cfg)
    generate_abstractions(cfg)
    generate_sql(cfg)
    generate_dapper_rows(cfg)
    generate_read_repository(cfg)
    generate_write_service(cfg)
    generate_validators(cfg)
    generate_commands(cfg)
    generate_queries(cfg)
    if not cfg.skip_controller:
        generate_controller(cfg)
    generate_tests(cfg)


def resolve_entities(batch: int | None, regenerate_all: bool = False) -> list[EntityConfig]:
    if batch == 2:
        return NEW_ENTITIES
    if regenerate_all:
        return ENTITIES + NEW_ENTITIES
    entities = ENTITIES + NEW_ENTITIES
    result = []
    for cfg in entities:
        controller_path = API / "Controllers" / f"{cfg.controller}.cs"
        if controller_path.exists() and cfg not in NEW_ENTITIES:
            continue
        if batch != 2 and cfg in NEW_ENTITIES and controller_path.exists() and not cfg.skip_controller:
            continue
        result.append(cfg)
    return result if batch != 2 else NEW_ENTITIES


def patch_dependency_injection(entities: list[EntityConfig]) -> None:
    path = INFRA / "DependencyInjection.cs"
    text = path.read_text(encoding="utf-8")
    imports = []
    registrations = []
    for cfg in entities:
        imp = f"using Fgs.Setup.Application.Abstractions.{cfg.abstractions_folder};"
        imp2 = f"using Fgs.Setup.Infrastructure.{cfg.infra_folder};"
        reg_read = f"services.AddScoped<{iface_name(cfg)}, {repo_class(cfg)}>();"
        reg_write = f"services.AddScoped<{write_iface_name(cfg)}, {write_class(cfg)}>();"
        if imp not in text:
            imports.append(imp)
        if imp2 not in text:
            imports.append(imp2)
        if reg_read not in text:
            registrations.append(f"        {reg_read}")
        if reg_write not in text:
            registrations.append(f"        {reg_write}")

    if imports:
        anchor = "using Fgs.Setup.Infrastructure.TitlesOfCourtesy;"
        if anchor in text:
            text = text.replace(anchor, anchor + "\n" + "\n".join(sorted(set(imports))))

    if registrations:
        anchor = "        services.AddScoped<ITitleOfCourtesyWriteService, TitleOfCourtesyWriteService>();"
        if anchor in text:
            text = text.replace(anchor, anchor + "\n" + "\n".join(registrations))
    path.write_text(text, encoding="utf-8", newline="\n")
    print(f"  Updated {path.relative_to(ROOT)}")


def patch_audit_helper(entities: list[EntityConfig]) -> None:
    path = INFRA / "Common" / "SetupEntityAuditHelper.cs"
    text = path.read_text(encoding="utf-8")
    blocks = []

    if any(cfg.base == "lead_entity" for cfg in entities):
        if "StampForCreate<T>" not in text:
            blocks.append("""
    public void StampForCreate<T>(T entity)
        where T : FgsEntityBase, ITenantCompanyScoped
    {
        var now = _dateTimeProvider.UtcNow;
        var actor = ResolveActor();
        var (tenantId, companyId) = ResolveTenantCompany();

        entity.CreatedOn = now;
        entity.CreatedBy = actor;
        entity.UpdatedOn = now;
        entity.UpdatedBy = actor;
        entity.IsActive = true;
        entity.TenantId = tenantId;
        entity.CompanyId = companyId;
    }

    public void StampForUpdate<T>(T entity)
        where T : FgsEntityBase, ITenantCompanyScoped
    {
        entity.UpdatedOn = _dateTimeProvider.UtcNow;
        entity.UpdatedBy = ResolveActor();
    }
""")

    if any(cfg.domain_entity == "FgsTag" for cfg in entities) and "StampForCreate(FgsTag" not in text:
        blocks.append("""
    public void StampForCreate(FgsTag entity)
    {
        var now = _dateTimeProvider.UtcNow;
        var actor = ResolveActor();
        var (tenantId, companyId) = ResolveTenantCompany();

        entity.CreatedOn = now;
        entity.CreatedBy = actor;
        entity.UpdatedOn = now;
        entity.UpdatedBy = actor;
        entity.IsActive = true;
        entity.TenantId = tenantId;
        entity.CompanyId = companyId;
    }

    public void StampForUpdate(FgsTag entity)
    {
        entity.UpdatedOn = _dateTimeProvider.UtcNow;
        entity.UpdatedBy = ResolveActor();
    }
""")

    if any(cfg.domain_entity == "FgsSetupCommunicationTemplate" for cfg in entities) and "StampForCreate(FgsSetupCommunicationTemplate" not in text:
        blocks.append("""
    public void StampForCreate(FgsSetupCommunicationTemplate entity, long? tenantId, long? companyId)
    {
        var now = _dateTimeProvider.UtcNow;
        var actor = ResolveActor();

        entity.TenantId = tenantId;
        entity.CompanyId = companyId;
        entity.CreatedOn = now;
        entity.CreatedBy = actor;
        entity.UpdatedOn = now;
        entity.UpdatedBy = actor;
        entity.IsActive = true;
    }

    public void StampForUpdate(FgsSetupCommunicationTemplate entity)
    {
        entity.UpdatedOn = _dateTimeProvider.UtcNow;
        entity.UpdatedBy = ResolveActor();
    }
""")

    if blocks:
        text = text.replace("    private string ResolveActor()", "".join(blocks) + "    private string ResolveActor()")
        path.write_text(text, encoding="utf-8", newline="\n")
        print(f"  Updated {path.relative_to(ROOT)}")


def main() -> None:
    parser = argparse.ArgumentParser(description="Generate Setup Service CRUD layers.")
    parser.add_argument("--batch", type=int, choices=[1, 2], default=None, help="Generate batch 1 (9 entities) or batch 2 (21 entities).")
    parser.add_argument("--all", action="store_true", help="Regenerate all catalog entities including existing controllers.")
    args = parser.parse_args()
    entities = resolve_entities(args.batch, regenerate_all=args.all)
    for cfg in entities:
        generate_entity(cfg)
    if args.batch is not None:
        patch_audit_helper(entities)
        patch_dependency_injection(entities)
    print(f"\nDone. Generated {len(entities)} entities.")


if __name__ == "__main__":
    main()
