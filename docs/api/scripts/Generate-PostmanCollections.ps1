<#
.SYNOPSIS
    Generates Postman v2.1 collections from FGS API controllers.
#>
param(
    [string]$RepoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path,
    [string]$OutputDir = (Join-Path $RepoRoot "docs\api")
)

$ErrorActionPreference = "Stop"

function New-PostmanUuid { return [guid]::NewGuid().ToString() }

function Get-RouteTemplate {
    param([string]$Raw, [string]$ControllerName)
    if ($Raw -eq '[controller]') {
        return ($ControllerName -replace 'Controller$','').ToLowerInvariant()
    }
    return $Raw.Trim().Trim('/')
}

function Test-AllowAnonymous {
    param([string]$ClassBlock, [string]$MethodBlock)
    return ($ClassBlock -match '\[AllowAnonymous\]') -or ($MethodBlock -match '\[AllowAnonymous\]')
}

function Test-RequiresAuth {
    param([string]$ClassHeader, [string]$MethodBlock)
    if ($MethodBlock -match '\[AllowAnonymous\]') { return $false }
    if ($ClassHeader -match '(?m)^\s*\[AllowAnonymous\]') { return $false }
    return $true
}

function Get-PostmanVarName {
    param([string]$ParamName)
    switch ($ParamName) {
        'id' { 'recordId' }
        default { $ParamName }
    }
}

function Convert-RouteToPostmanPath {
    param([string]$Template)
    if ([string]::IsNullOrWhiteSpace($Template)) { return '' }
    return [regex]::Replace($Template, '\{(\w+)(?::(?:long|guid))?\}', {
        param($m)
        $varName = Get-PostmanVarName $m.Groups[1].Value
        '{' + '{' + $varName + '}' + '}'
    })
}

function Get-HttpVerb {
    param([string]$HttpAttributeName)
    return ($HttpAttributeName -replace '^Http','').ToUpper()
}

function Join-UrlPath {
    param([string]$Base, [string]$Suffix)
    if ([string]::IsNullOrWhiteSpace($Suffix)) { return $Base }
    return ('{0}/{1}' -f $Base.TrimEnd('/'), $Suffix.TrimStart('/'))
}

function Get-MethodSignatureBlock {
    param(
        [string]$Content,
        [int]$StartIndex,
        [string]$MethodName
    )

    $anchor = $Content.IndexOf("$MethodName(", $StartIndex, [System.StringComparison]::Ordinal)
    if ($anchor -lt 0) { return '' }

    $openIdx = $anchor + $MethodName.Length
    $depth = 0
    for ($i = $openIdx; $i -lt $Content.Length; $i++) {
        switch ($Content[$i]) {
            '(' { $depth++ }
            ')' {
                $depth--
                if ($depth -eq 0) {
                    return $Content.Substring($anchor, $i - $anchor + 1)
                }
            }
        }
    }

    return ''
}

function Get-StandardPaginationQueryItems {
    return @(
        @{ key = 'page'; value = '{{page}}'; description = 'Page number (1-based). Default: 1.' }
        @{ key = 'pageSize'; value = '{{pageSize}}'; description = 'Items per page (1-100). Default: 25.' }
        @{ key = 'sortBy'; value = '{{sortBy}}'; description = 'Property to sort by (entity-specific). Optional.'; disabled = $true }
        @{ key = 'sortDirection'; value = '{{sortDirection}}'; description = 'Sort direction: Asc or Desc. Default: Asc.'; disabled = $true }
        @{ key = 'search'; value = '{{search}}'; description = 'Free-text search across searchable fields. Optional.'; disabled = $true }
        @{ key = 'isActive'; value = 'true'; description = 'Filter by active records. Default: true.' }
    )
}

function Test-IsPaginatedListAction {
    param([string]$MethodBlock)
    return $MethodBlock -match '\[FromQuery\]\s+int\s+page\s*='
}

function Get-ListFilterQueryItemsFromBlock {
    param([string]$MethodBlock)

    $skip = @{
        page = $true
        pageSize = $true
        sortBy = $true
        sortDirection = $true
        search = $true
        isActive = $true
        cancellationToken = $true
    }

    $items = @()
    $matches = [regex]::Matches($MethodBlock, '\[FromQuery\]\s+[\w\?\.]+\s+(\w+)\s*(?:=\s*[^,\)]+)?')
    foreach ($m in $matches) {
        $name = $m.Groups[1].Value
        if ($skip.ContainsKey($name)) { continue }
        $items += @{
            key = (ConvertTo-CamelCase $name)
            value = ''
            description = "Optional $name filter."
            disabled = $true
        }
    }

    return $items
}

function Convert-ToPostmanUrl {
    param(
        [string]$Url,
        [array]$QueryItems = @()
    )

    $urlObj = @{ raw = $Url }
    $pathPart = $Url
    $inlineQuery = @()

    if ($Url -match '^([^?]+)\?(.*)$') {
        $pathPart = $Matches[1]
        foreach ($pair in ($Matches[2] -split '&')) {
            if ($pair -match '^([^=]+)=(.*)$') {
                $inlineQuery += @{ key = $Matches[1]; value = $Matches[2]; description = "" }
            }
        }
    }

    if ($pathPart -match '^(https?)://(.+)$') {
        $urlObj.protocol = $Matches[1]
        $pathPart = $Matches[2]
    }

    $segments = @($pathPart.Trim('/').Split('/') | Where-Object { $_ })
    if ($segments.Count -gt 0) {
        $urlObj.host = @($segments[0])
        if ($segments.Count -gt 1) {
            $urlObj.path = @($segments[1..($segments.Count - 1)])
        }
    }

    $allQuery = @($inlineQuery) + @($QueryItems)
    if ($allQuery.Count -gt 0) {
        $urlObj.query = $allQuery
    }

    return $urlObj
}

function New-PostmanRequest {
    param(
        [string]$Name,
        [string]$Method,
        [string]$Url,
        [bool]$UseAuth,
        [string]$Description = "",
        [hashtable]$Query = @{},
        [array]$QueryItems = @(),
        [string]$Body = $null,
        [hashtable]$Headers = @{}
    )

    if ($QueryItems.Count -eq 0) {
        foreach ($k in $Query.Keys) {
            $QueryItems += @{ key = $k; value = [string]$Query[$k]; description = "" }
        }
    }

    $headerItems = @(
        @{ key = "Accept"; value = "application/json"; type = "text" }
    )
    foreach ($k in $Headers.Keys) {
        $headerItems += @{ key = $k; value = [string]$Headers[$k]; type = "text" }
    }

    $request = @{
        method = $Method.ToUpper()
        header = $headerItems
        url = (Convert-ToPostmanUrl -Url $Url -QueryItems $queryItems)
        description = $Description
    }

    if ($UseAuth) {
        $request.auth = @{
            type = "bearer"
            bearer = @(@{ key = "token"; value = "{{accessToken}}"; type = "string" })
        }
    }
    else {
        $request.auth = @{ type = "noauth" }
    }

    if (-not [string]::IsNullOrWhiteSpace($Body) -and $Method -in @('POST','PUT','PATCH')) {
        $request.header += @{ key = "Content-Type"; value = "application/json"; type = "text" }
        $request.body = @{ mode = "raw"; raw = $Body; options = @{ raw = @{ language = "json" } } }
    }

    return @{
        name = $Name
        request = $request
    }
}

function Get-GatewayExternalPath {
    param([string]$ServiceKey, [string]$RouteTemplate)

    # Setup catalog routes are exposed at /api/v1/{catalog} (no /setup prefix).
    if ($ServiceKey -eq 'SetupService' -and $RouteTemplate -ne 'health') {
        return "/api/v1/$RouteTemplate"
    }

    # Credential audits and tenant bucket routes use direct gateway paths.
    if ($ServiceKey -eq 'AuditService' -and $RouteTemplate -eq 'credential-audits') {
        return '/api/v1/credential-audits'
    }
    if ($ServiceKey -eq 'FileService' -and $RouteTemplate -eq 'tenants') {
        return '/api/v1/tenants'
    }
    if ($ServiceKey -eq 'FileService' -and $RouteTemplate -eq 'files') {
        return '/api/v1/files'
    }

    # Notification nginx rewrite: /api/v1/notifications/* -> /api/v1/*
    if ($ServiceKey -eq 'NotificationService' -and $RouteTemplate -eq 'notifications') {
        return '/api/v1/notifications/notifications'
    }

    $gatewayPrefix = @{
        'NotificationService' = 'notifications'
        'CrmService' = 'crm'
        'SchedulingService' = 'scheduling'
        'BillingService' = 'billing'
        'InventoryService' = 'inventory'
        'ReportingService' = 'reporting'
        'IntegrationService' = 'integration'
        'AssetService' = 'asset'
        'ServiceAgreementService' = 'service-agreements'
        'CommunicationService' = 'communication'
        'PublisherService' = 'publisher'
        'ConsumerService' = 'consumer'
        'AuditService' = 'audit'
        'SetupService' = 'setup'
    }

    if ($gatewayPrefix.ContainsKey($ServiceKey)) {
        return "/api/v1/$($gatewayPrefix[$ServiceKey])/$RouteTemplate"
    }

    return "/api/v1/$RouteTemplate"
}

function Get-ServiceBaseUrl {
    param([string]$ServiceKey, [string]$RouteTemplate)

    $path = Get-GatewayExternalPath -ServiceKey $ServiceKey -RouteTemplate $RouteTemplate
    return "{{gatewayUrl}}$path"
}

function ConvertTo-CamelCase {
    param([string]$Name)
    if ([string]::IsNullOrWhiteSpace($Name)) { return $Name }
    if ($Name.Length -eq 1) { return $Name.ToLowerInvariant() }
    return ($Name.Substring(0, 1).ToLowerInvariant() + $Name.Substring(1))
}

function Get-SampleJsonValue {
    param(
        [string]$PropertyName,
        [string]$CsType,
        [bool]$IsPatch = $false
    )

    $nullable = $CsType.EndsWith('?')
    $baseType = $CsType.TrimEnd('?')
    $camel = ConvertTo-CamelCase $PropertyName

    if ($IsPatch) {
        if ($PropertyName -eq 'IsActive') { return 'true' }
        if ($baseType -eq 'bool') { return 'null' }
        if ($baseType -in @('string', 'String')) {
            if ($PropertyName -match 'Name|DisplayName|TaskName|Subject|Body|Description|Notes|ShortNote') {
                return '"Updated sample"'
            }
            if ($PropertyName -match 'Code|Type|Category|Channel|VIN|PostalCode|VendorCode|WarehouseCode|TaxCode|RegionCode|TagCode|StatusCode|OutcomeCode|ActivityTypeCode|DispositionReasonCode|SourceCode|ReasonCode|JobTypeCode|CategoryCode|SubCategoryCode|BillingCategoryType|DescriptionTypeCode|ResolutionCode|DueDateMethod|UsedFor|OwnershipType|VendorType|WarehouseType|CommunicationChannel|TemplateType') {
                return 'null'
            }
            return 'null'
        }
        if ($baseType -in @('short', 'int', 'long', 'decimal', 'double', 'float')) { return 'null' }
        if ($baseType -eq 'DateOnly') { return 'null' }
        if ($baseType -eq 'TimeSpan') { return 'null' }
        if ($baseType -eq 'Guid') { return 'null' }
        return 'null'
    }

    if ($nullable -and $PropertyName -match 'Id$|SyncToken|ExternalSystemId|IconFileId|AddressId|LogoFileId|JobTypeSubCategoryId|NextSalesPipelineStatusId|PaymentTermId|FgsSetupZoneId|FgsSetupTaxId|FgsSetupTechTradeId|TenantId|CompanyId') {
        if ($PropertyName -eq 'TenantId') { return 'null' }
        if ($PropertyName -eq 'CompanyId') { return 'null' }
        if ($PropertyName -match 'WarehouseId|VehicleId|FgsSetupTaxId|FgsSetupTaxAuthorityId|JobTypeCategoryId') { return '{{recordId}}' }
        return 'null'
    }

    switch ($baseType) {
        'bool' {
            switch -Regex ($PropertyName) {
                '^IsActive$' { return 'true' }
                '^IsSystem$|^IsSystemDefined$|^IsSystemGenerated$|^Is1099Eligible$|^IsExternalSystemRecord$' { return 'false' }
                '^IsCompleted$' { return 'true' }
                '^IsDefault$' { return 'false' }
                '^Show|^Allow|^IsMobileVisible$|^IsCustomerPortalVisible$|^ShowToFieldTech$|^ShowOnCustomerPortal$|^AllowToPick$|^AllowManualSelection$|^AppliesToLead$|^AppliesToOpportunity$' { return 'true' }
                '^IsTerminal$|^RequireComment$' { return 'false' }
                default { return 'true' }
            }
        }
        'short' {
            if ($PropertyName -match 'Year') { return '2024' }
            if ($PropertyName -match 'Priority') { return '5' }
            if ($PropertyName -match 'DisplayOrder|SortOrder') { return '1' }
            return '1'
        }
        'int' {
            if ($PropertyName -match 'Minutes|Mileage|NumberOfDays|VehicleMaintenanceTypeId|GloResolutionTypeId') { return '1' }
            return '1'
        }
        'long' {
            if ($PropertyName -match 'Id$') { return '{{recordId}}' }
            return '1'
        }
        'decimal' {
            if ($PropertyName -match 'Percent|TaxPercent') { return '8.25' }
            if ($PropertyName -match 'Price|Cost') { return '100.00' }
            return '0.00'
        }
        'double' { return '0.0' }
        'float' { return '0.0' }
        'DateOnly' { return '"2026-06-21"' }
        'TimeSpan' {
            if ($PropertyName -match 'Begin|Start|Arrived') { return '"08:00:00"' }
            if ($PropertyName -match 'End|Delayed|Completion') { return '"17:00:00"' }
            return '"09:00:00"'
        }
        'Guid' { return '"11111111-1111-1111-1111-111111111111"' }
        default {
            if ($PropertyName -match 'Email') { return '"office@example.com"' }
            if ($PropertyName -match 'Phone|Mobile') { return '"+15551234567"' }
            if ($PropertyName -match 'Website|Url') { return '"https://example.com"' }
            if ($PropertyName -match 'VIN') { return '"1HGBH41JXMN109186"' }
            if ($PropertyName -match 'PostalCode') { return '"78701"' }
            if ($PropertyName -match 'BackgroundColor|TextColor') { return '"#FFFFFF"' }
            if ($PropertyName -match 'Body|Subject|Description|Notes|ShortNote|TaskName|LegalName|ServiceProvider|InvoiceNumber|PurchasedFrom|OwnershipCompany|BusinessUnit|Trade|UsedFor|DueDateMethod|VendorType|WarehouseType|OwnershipType|CommunicationChannel|TemplateType|BillingCategoryName|BillingCategoryType') {
                return (Get-StringSampleValue $PropertyName)
            }
            if ($PropertyName -match 'Code|Type$|Category') {
                return (Get-CodeSampleValue $PropertyName)
            }
            if ($PropertyName -match 'Name|DisplayName') {
                return (Get-NameSampleValue $PropertyName)
            }
            if ($nullable) { return 'null' }
            return '"sample"'
        }
    }
}

function Get-CodeSampleValue {
    param([string]$PropertyName)
    $map = @{
        Code = 'SAMPLE'
        CategoryCode = 'GEN'
        SubCategoryCode = 'SUB'
        JobTypeCode = 'SVC'
        TaxCode = 'TX'
        VendorCode = 'VND01'
        WarehouseCode = 'WH01'
        PostalCode = '78701'
        StatusCode = 'NEW'
        OutcomeCode = 'WON'
        ActivityTypeCode = 'CALL'
        DispositionReasonCode = 'NOBUDGET'
        SourceCode = 'WEB'
        ReasonCode = 'OTHER'
        ResolutionCode = 'FIXED'
        DescriptionTypeCode = 'NOTES'
        BillingCategoryType = 'IN'
        CommunicationChannel = 'Email'
        TemplateType = 'Email'
        DueDateMethod = 'NetDays'
        UsedFor = 'Service'
        OwnershipType = 'Owned'
        VendorType = 'VENDOR'
        WarehouseType = 'Warehouse'
        RegionCode = 'TX'
        TagCode = 'TAG01'
    }
    foreach ($key in $map.Keys) {
        if ($PropertyName -eq $key) { return ('"' + $map[$key] + '"') }
    }
    if ($PropertyName -match 'Code$') {
        $stem = ($PropertyName -replace 'Code$','').ToUpperInvariant()
        if ($stem.Length -gt 6) { $stem = $stem.Substring(0, 6) }
        return ('"' + $stem + '"')
    }
    return '"CODE"'
}

function Get-NameSampleValue {
    param([string]$PropertyName)
    $map = @{
        Name = 'Sample Name'
        DisplayName = 'Sample Display Name'
        TaskName = 'Sample Service Call'
        StatusName = 'New Lead'
        OutcomeName = 'Qualified'
        ActivityTypeName = 'Phone Call'
        DispositionReasonName = 'No Budget'
        SourceName = 'Website'
        ReasonName = 'Other'
        ResolutionName = 'Issue Resolved'
        BillingCategoryName = 'Service Invoice'
        VendorName = 'Sample Vendor'
        LegalName = 'Sample Vendor LLC'
    }
    foreach ($key in $map.Keys) {
        if ($PropertyName -eq $key) { return ('"' + $map[$key] + '"') }
    }
    if ($PropertyName -match 'Name$') { return '"Sample Name"' }
    return '"Sample"'
}

function Get-StringSampleValue {
    param([string]$PropertyName)
    if ($PropertyName -eq 'Body') { return '"Hello {{CompanyName}}, this is a sample template body."' }
    if ($PropertyName -eq 'Subject') { return '"Sample subject line"' }
    if ($PropertyName -eq 'Description') { return '"Sample description"' }
    if ($PropertyName -eq 'Notes') { return '"Sample notes"' }
    if ($PropertyName -eq 'ShortNote') { return '"Note"' }
    if ($PropertyName -eq 'BillingCategoryType') { return '"IN"' }
    if ($PropertyName -eq 'CommunicationChannel') { return '"Email"' }
    if ($PropertyName -eq 'TemplateType') { return '"Email"' }
    if ($PropertyName -eq 'DueDateMethod') { return '"NetDays"' }
    if ($PropertyName -eq 'UsedFor') { return '"Service"' }
    if ($PropertyName -eq 'OwnershipType') { return '"Owned"' }
    if ($PropertyName -eq 'VendorType') { return '"VENDOR"' }
    if ($PropertyName -eq 'WarehouseType') { return '"Warehouse"' }
    return '"sample"'
}

function Build-DtoRegistry {
    param([string]$Root)

    $registry = @{}
    $dtoFiles = Get-ChildItem -Path (Join-Path $Root 'src') -Filter '*Dtos.cs' -Recurse -ErrorAction SilentlyContinue |
        Where-Object { $_.FullName -match 'Application\\Features' }

    foreach ($file in $dtoFiles) {
        $content = Get-Content -Raw -Path $file.FullName
        $matches = [regex]::Matches($content, 'public\s+sealed\s+record\s+(\w+)\s*\((.*?)\)\s*;', [System.Text.RegularExpressions.RegexOptions]::Singleline)
        foreach ($m in $matches) {
            $typeName = $m.Groups[1].Value
            if ($typeName -match 'SummaryDto|DetailDto|LookupDto|ListFilters$') { continue }

            $props = @()
            foreach ($line in ($m.Groups[2].Value -split ',')) {
                $line = $line.Trim()
                if ([string]::IsNullOrWhiteSpace($line)) { continue }
                if ($line -match '^(?<type>[\w\?\.]+)\s+(?<name>\w+)(?:\s*=\s*[^,]+)?$') {
                    $props += [pscustomobject]@{
                        Name = $Matches.name
                        CsType = $Matches.type
                    }
                }
            }
            if ($props.Count -gt 0) {
                $registry[$typeName] = $props
            }
        }
    }

    return $registry
}

function Get-DtoSampleBody {
    param(
        [string]$DtoType,
        [hashtable]$Registry,
        [string]$MethodName
    )

    if (-not $Registry.ContainsKey($DtoType)) { return $null }

    $props = $Registry[$DtoType]
    $isPatch = ($MethodName -eq 'Patch') -or $DtoType -match 'PatchDto$'

    if ($isPatch) {
        $selected = @()
        foreach ($p in $props) {
            if ($p.Name -eq 'IsActive') { $selected += $p; continue }
            if ($p.Name -match 'Name|DisplayName|TaskName|Description|Body|Subject|SortOrder|DisplayOrder') {
                $selected += $p
            }
        }
        if ($selected.Count -eq 0) {
            $selected = @($props | Select-Object -First 2)
        }
        $props = $selected | Select-Object -Unique -Property Name, CsType
    }

    $lines = @()
    foreach ($p in $props) {
        $value = Get-SampleJsonValue -PropertyName $p.Name -CsType $p.CsType -IsPatch:$isPatch
        $lines += ('  "{0}": {1}' -f (ConvertTo-CamelCase $p.Name), $value)
    }

    return "{`n" + ($lines -join ",`n") + "`n}"
}

function Get-FromBodyDtoType {
    param(
        [string]$Content,
        [int]$StartIndex,
        [string]$MethodName
    )

    $length = [Math]::Min(1200, $Content.Length - $StartIndex)
    if ($length -le 0) { return $null }
    $snippet = $Content.Substring($StartIndex, $length)
    if ($snippet -match '(?s)public\s+(?:async\s+)?(?:Task<[^>]+>|IActionResult|ActionResult(?:<[^>]+>)?)\s+' + [regex]::Escape($MethodName) + '\s*\(.*?\[FromBody\]\s+(\w+)') {
        return $Matches[1]
    }
    return $null
}

function Parse-ControllerFile {
    param(
        [string]$FilePath,
        [string]$ServiceKey,
        [hashtable]$DtoRegistry
    )

    $content = Get-Content -Raw -Path $FilePath
    $fileName = [IO.Path]::GetFileNameWithoutExtension($FilePath)
    if ($fileName -notmatch 'Controller$') { return $null }

    # Merge partial AuthController files
    if ($fileName -eq 'AuthController') {
        $dir = Split-Path $FilePath -Parent
        Get-ChildItem -Path $dir -Filter 'AuthController.*.cs' | ForEach-Object {
            $content += "`n" + (Get-Content -Raw -Path $_.FullName)
        }
    }

    $routeMatch = [regex]::Match($content, '\[FgsVersionedRoute\("([^"]+)"\)\]')
    if (-not $routeMatch.Success) { return $null }

    $routeTemplate = Get-RouteTemplate $routeMatch.Groups[1].Value $fileName
    $classHeader = ($content -split 'public\s+(?:sealed\s+)?(?:partial\s+)?class')[0]
    $baseUrl = Get-ServiceBaseUrl $ServiceKey $routeTemplate

    $controllerDescription = "$fileName - $routeTemplate"

    $items = @()
    $methodRegex = [regex]::Matches($content, '(?s)((?:\s*///[^\r\n]*\r?\n)*)\s*(?:\[(?:AllowAnonymous|Authorize[^\]]*)\]\s*)*(\[Http(Get|Post|Put|Patch|Delete)(?:\("([^"]*)"\))?\])\s*(?:\[[^\]]+\]\s*)*public\s+(?:async\s+)?(?:Task<(?:IActionResult|ActionResult(?:<[^>]+>)?)>|ContentResult|IActionResult)\s+(\w+)\s*\(')

    foreach ($m in $methodRegex) {
        $block = $m.Value
        $httpAttr = $m.Groups[3].Value
        $routeSuffix = $m.Groups[4].Value
        $methodName = $m.Groups[5].Value
        $verb = Get-HttpVerb $httpAttr

        $docSummary = $null
        if ($m.Groups[1].Success -and $m.Groups[1].Value -match '///\s*<summary>\s*(.*?)\s*</summary>') {
            $docSummary = $Matches[1].Trim()
        }

        $useAuth = Test-RequiresAuth $classHeader $block
        $pathSuffix = Convert-RouteToPostmanPath $routeSuffix
        if ($routeSuffix -match '^~?/') {
            $absolutePath = ($routeSuffix -replace '^~/','/') -replace 'v\{version:apiVersion\}','v1'
            $absolutePath = Convert-RouteToPostmanPath $absolutePath.TrimStart('/')
            $serviceHost = if ($baseUrl -match '^(\{\{[^}]+\}\})') { $Matches[1] } else { '{{gatewayUrl}}' }
            $fullPath = Join-UrlPath $serviceHost $absolutePath
        }
        else {
            $fullPath = Join-UrlPath $baseUrl $pathSuffix
        }

        $query = @{}
        $queryItems = @()
        $headers = @{}
        $body = $null
        $signatureBlock = Get-MethodSignatureBlock -Content $content -StartIndex $m.Index -MethodName $methodName

        if ($methodName -eq 'List' -and (Test-IsPaginatedListAction $signatureBlock)) {
            $queryItems = @(Get-StandardPaginationQueryItems) + @(Get-ListFilterQueryItemsFromBlock $signatureBlock)
        }
        if ($methodName -eq 'GetActive' -and $routeTemplate -eq 'communication-templates') {
            $query = @{ tenantId = '{{tenantId}}'; companyId = '{{companyId}}'; templateType = 'Email'; code = 'INVITE' }
            $headers['X-Internal-Service-Key'] = '{{internalServiceKey}}'
            $useAuth = $false
        }
        if ($methodName -eq 'Get' -and $fileName -eq 'DashboardController') {
            $fullPath = '{{gatewayUrl}}/api/v1/dashboard?token={{accessToken}}'
            $useAuth = $false
        }
        if ($httpAttr -in @('Post','Put','Patch') -and $methodName -notin @('EntraCallback','EntraConnector','CompanySignup','Start','CompleteUpload')) {
            $body = "{}"
        }
        if ($methodName -eq 'CompanySignup') {
            $body = @'
{
  "contact": { "name": "Admin User", "phoneNumber": "+15551234567", "email": "{{signupEmail}}" },
  "company": {
    "name": "Acme Field Services",
    "website": "https://acme.example.com",
    "companySize": "11-50",
    "address": { "addressLine1": "100 Main St", "city": "Austin", "state": "TX", "postalCode": "78701", "country": "US" }
  },
  "businessTypeIds": [1]
}
'@
        }
        if ($methodName -eq 'EntraConnector') {
            $body = '{ "email": "{{signupEmail}}", "objectId": null }'
        }
        if ($fileName -eq 'TenantsController' -and $methodName -eq 'UpdateDetails') {
            $body = @'
{
  "tenant": {
    "name": "Plumbing Ltd",
    "legalName": "Plumbing Ltd LLC",
    "email": "admin@plumbing.example.com",
    "phoneNumber": "+15551234567",
    "website": "https://plumbing.example.com",
    "timeZone": "America/Chicago",
    "defaultCurrency": "USD",
    "defaultLanguageId": null
  },
  "company": {
    "name": "Plumbing Ltd",
    "legalName": "Plumbing Ltd LLC",
    "email": "office@plumbing.example.com",
    "phoneNumber": "+15559876543",
    "website": "https://plumbing.example.com",
    "taxId": "12-3456789",
    "companySize": "11-50",
    "businessTypeId": 1,
    "physicalAddress": {
      "addressLine1": "100 Main St",
      "city": "Dallas",
      "state": "TX",
      "postalCode": "75201",
      "country": "US"
    },
    "billingAddress": {
      "addressLine1": "100 Main St",
      "city": "Dallas",
      "state": "TX",
      "postalCode": "75201",
      "country": "US"
    }
  }
}
'@
        }
        if ($fileName -eq 'TenantsController' -and $methodName -eq 'UpdateStatus') {
            $body = '{ "fgsTenantStatusId": 3 }'
        }
        if ($fileName -eq 'TenantsController' -and $methodName -eq 'UpdateStorageBucket') {
            $body = '{ "storageBucketName": "fgs-dev-tenant-{{tenantId}}-demo" }'
        }
        if ($fileName -eq 'BusinessTypesController' -and $methodName -eq 'AddCompanyBusinessTypes') {
            $body = @'
{
  "businessTypeIds": [1],
  "companyGuid": "11111111-1111-1111-1111-111111111111",
  "code": "PLUMB-CO",
  "name": "Plumbing Ltd",
  "isActive": true
}
'@
            $headers['X-Tenant-Id'] = '{{tenantId}}'
            $headers['X-Company-Id'] = '{{companyId}}'
        }
        if ($fileName -eq 'CredentialsController') {
            if ($methodName -eq 'List') {
                $query = @{ scope = 'Global'; activeOnly = 'true' }
            }
            if ($methodName -in @('Get', 'Update', 'Delete', 'Rotate', 'ResolveSecret')) {
                $query = @{ scope = 'Global' }
            }
            if ($methodName -eq 'GetResolvedConfiguration') {
                $headers['X-FGS-Internal-Service-Key'] = '{{internalServiceKey}}'
                $headers['X-FGS-Service-Name'] = 'fgs-user-service'
                $useAuth = $false
            }
            if ($methodName -eq 'Create') {
                $body = @'
{
  "scope": 1,
  "providerCode": "DATABASE",
  "credentialName": "PlatformDatabaseConnections",
  "payload": "{\"FgsUser\":\"Host=localhost;Port=5432;Database=fgs_dev_db;Username=postgres;Password=secret\"}",
  "description": "Platform database connection strings",
  "tenantId": null,
  "companyId": null
}
'@
            }
            if ($methodName -eq 'Update') {
                $body = @'
{
  "credentialName": "PlatformDatabaseConnections",
  "description": "Updated platform database connections",
  "payload": "{\"FgsUser\":\"Host=localhost;Port=5432;Database=fgs_dev_db;Username=postgres;Password=secret\"}",
  "isActive": true
}
'@
            }
            if ($methodName -eq 'Rotate') {
                $body = '{ "rotationMode": 1 }'
            }
        }
        if ($fileName -eq 'TenantProvisioningController' -and $methodName -eq 'ProvisionTenant') {
            $body = @'
{
  "tenantId": 4,
  "companyId": 1,
  "tenantCode": "plumbing-ltd",
  "correlationId": "11111111-1111-1111-1111-111111111111",
  "userId": null
}
'@
        }
        if ($fileName -eq 'FilesController') {
            if ($methodName -eq 'CreateUploadUrl') {
                $body = @'
{
  "fileName": "company-logo.png",
  "contentType": "image/png",
  "fileSizeBytes": 102400,
  "entityType": "Company",
  "entityId": {{companyId}},
  "requestedVariant": "full",
  "description": "Company logo upload",
  "tags": []
}
'@
            }
            if ($methodName -eq 'CompleteUpload') {
                $body = $null
            }
            if ($methodName -eq 'GetByEntity') {
                $query = @{ entityType = 'Company'; entityId = '{{companyId}}' }
            }
            $headers['X-Tenant-Id'] = '{{tenantId}}'
            $headers['X-Company-Id'] = '{{companyId}}'
        }
        if ($fileName -eq 'TenantStorageController' -and $methodName -eq 'ProvisionBucket') {
            $body = @'
{
  "tenantId": 4,
  "existingBucketName": null,
  "companyNumbers": [1]
}
'@
        }
        if ($fileName -eq 'NotificationsController' -and $methodName -eq 'Dispatch') {
            $body = @'
{
  "tenantId": 4,
  "companyId": 1,
  "channel": "Email",
  "templateCode": "INVITE",
  "recipient": "{{entraUserEmail}}",
  "correlationId": "postman-test-001",
  "tokens": {
    "CompanyName": "Plumbing Ltd",
    "InviteUrl": "https://localhost:8443/api/v1/invite/start?token=sample"
  }
}
'@
        }
        if ($fileName -eq 'CredentialAuditsController' -and $methodName -eq 'Record') {
            $body = @'
{
  "tenantId": 4,
  "companyId": 1,
  "credentialId": "11111111-1111-1111-1111-111111111111",
  "actionType": "READ",
  "remarks": "Postman audit test",
  "oldVersionNo": null,
  "newVersionNo": 1,
  "createdBy": "postman"
}
'@
        }
        if ($fileName -eq 'TechTradesController') {
            if ($methodName -eq 'Lookup') {
                $query = @{ activeOnly = 'true' }
            }
            if ($methodName -eq 'Create') {
                $body = '{ "tradeCode": "HVAC", "name": "HVAC Services", "description": "Heating and cooling", "sortOrder": 1 }'
            }
            if ($methodName -eq 'Update') {
                $body = '{ "tradeCode": "HVAC", "name": "HVAC Services Updated", "description": "Heating and cooling", "sortOrder": 1 }'
            }
            if ($methodName -eq 'Patch') {
                $body = '{ "name": "HVAC Services Updated", "sortOrder": 2 }'
            }
            $headers['X-Tenant-Id'] = '{{tenantId}}'
            $headers['X-Company-Id'] = '{{companyId}}'
        }
        if ($fileName -eq 'GLBreaksController') {
            if ($methodName -eq 'Lookup') {
                $query = @{ activeOnly = 'true' }
            }
            if ($methodName -eq 'Create') {
                $body = @'
{
  "code": "PLUMB",
  "name": "Plumbing Division",
  "breakLabel": "Plumbing Services",
  "breakLevel": 1,
  "logoFileId": null,
  "address": {
    "addressLine1": "456 Oak Ave",
    "city": "Austin",
    "state": "TX",
    "country": "US",
    "postalCode": "78701"
  },
  "tradeCodes": ["PLUMB"]
}
'@
            }
            if ($methodName -eq 'Update') {
                $body = @'
{
  "code": "PLUMB",
  "name": "Plumbing Division Updated",
  "breakLabel": "Plumbing Services",
  "breakLevel": 1,
  "logoFileId": null,
  "address": {
    "addressLine1": "456 Oak Ave",
    "city": "Austin",
    "state": "TX",
    "country": "US",
    "postalCode": "78701"
  },
  "tradeCodes": ["PLUMB"]
}
'@
            }
            if ($methodName -eq 'Patch') {
                $body = '{ "name": "Plumbing Division Updated", "breakLabel": "Residential Plumbing" }'
            }
            $headers['X-Tenant-Id'] = '{{tenantId}}'
            $headers['X-Company-Id'] = '{{companyId}}'
        }
        if ($fileName -eq 'TaxesController') {
            if ($methodName -eq 'Create') {
                $body = @'
{
  "taxCode": "COMBINED",
  "name": "Combined Tax",
  "isExternalSystemRecord": false,
  "externalSystemId": null,
  "syncToken": null,
  "showTaxDetail": true,
  "description": "State + county",
  "taxDetails": [
    {
      "fgsSetupTaxAuthorityId": {{recordId}},
      "effectiveFromDate": "2026-01-01",
      "effectiveToDate": null,
      "isExternalSystemRecord": false
    }
  ]
}
'@
            }
            if ($methodName -eq 'Update') {
                $body = @'
{
  "taxCode": "COMBINED",
  "name": "Combined Tax Updated",
  "isExternalSystemRecord": false,
  "externalSystemId": null,
  "syncToken": null,
  "showTaxDetail": true,
  "description": "State + county",
  "taxDetails": [
    {
      "fgsSetupTaxAuthorityId": {{recordId}},
      "effectiveFromDate": "2026-01-01",
      "effectiveToDate": null,
      "isExternalSystemRecord": false
    }
  ]
}
'@
            }
            if ($methodName -eq 'Patch') {
                $body = @'
{
  "name": "Combined Tax Updated",
  "taxDetails": [
    {
      "fgsSetupTaxAuthorityId": {{recordId}},
      "effectiveFromDate": "2026-01-01",
      "effectiveToDate": null,
      "isExternalSystemRecord": false
    }
  ]
}
'@
            }
        }
        if ($useAuth) {
            $headers['X-Tenant-Id'] = '{{tenantId}}'
            $headers['X-Company-Id'] = '{{companyId}}'
        }

        if ($methodName -eq 'Start') {
            $fullPath = '{{gatewayUrl}}/api/v1/invite/start?token={{inviteToken}}'
        }
        if ($methodName -eq 'EntraCallback') {
            $fullPath = '{{gatewayUrl}}/api/v1/auth/entra/callback?code={{authCode}}&state={{invitationId}}'
        }

        if ($httpAttr -in @('Post','Put','Patch') -and ($body -eq '{}' -or [string]::IsNullOrWhiteSpace($body)) -and $null -ne $DtoRegistry) {
            $dtoType = Get-FromBodyDtoType -Content $content -StartIndex $m.Index -MethodName $methodName
            if ($dtoType) {
                $generatedBody = Get-DtoSampleBody -DtoType $dtoType -Registry $DtoRegistry -MethodName $methodName
                if ($generatedBody) { $body = $generatedBody }
            }
        }

        $displayName = $methodName
        if ($docSummary -and $docSummary.Length -le 48 -and $docSummary -notmatch '[.!?]') {
            $displayName = $docSummary
        }
        $description = if ($docSummary) { $docSummary } else { $methodName }
        $req = New-PostmanRequest -Name $displayName -Method $verb -Url $fullPath -UseAuth $useAuth -Description $description -Query $query -QueryItems $queryItems -Body $body -Headers $headers
        if ($fileName -eq 'TechTradesController' -and $methodName -eq 'Create') {
            $req['event'] = @(@{
                listen = 'test'
                script = @{
                    type = 'text/javascript'
                    exec = @(
                        'const body = pm.response.json();',
                        'if (body.success && body.data && body.data.id) {',
                        '  pm.environment.set("recordId", String(body.data.id));',
                        '}'
                    )
                }
            })
        }
        if ($fileName -eq 'GLBreaksController' -and $methodName -eq 'Create') {
            $req['event'] = @(@{
                listen = 'test'
                script = @{
                    type = 'text/javascript'
                    exec = @(
                        'const body = pm.response.json();',
                        'if (body.success && body.data && body.data.id) {',
                        '  pm.environment.set("recordId", String(body.data.id));',
                        '}'
                    )
                }
            })
        }
        if ($methodName -eq 'Create' -and $httpAttr -eq 'Post' -and -not $req.ContainsKey('event')) {
            $req['event'] = @(@{
                listen = 'test'
                script = @{
                    type = 'text/javascript'
                    exec = @(
                        'const body = pm.response.json();',
                        'if (body.success && body.data && body.data.id) {',
                        '  pm.environment.set("recordId", String(body.data.id));',
                        '}'
                    )
                }
            })
        }
        if ($fileName -eq 'CredentialsController' -and $methodName -eq 'Create') {
            $req['event'] = @(@{
                listen = 'test'
                script = @{
                    type = 'text/javascript'
                    exec = @(
                        'const body = pm.response.json();',
                        'if (body.success && body.data && body.data.id) {',
                        '  pm.environment.set("recordId", String(body.data.id));',
                        '}'
                    )
                }
            })
        }
        if ($fileName -eq 'FilesController' -and $methodName -eq 'CreateUploadUrl') {
            $req['event'] = @(@{
                listen = 'test'
                script = @{
                    type = 'text/javascript'
                    exec = @(
                        'const body = pm.response.json();',
                        'if (body.success && body.data) {',
                        '  if (body.data.fileId) {',
                        '    pm.environment.set("fileId", String(body.data.fileId));',
                        '    pm.environment.set("recordId", String(body.data.fileId));',
                        '  }',
                        '  if (body.data.uploadUrl) {',
                        '    pm.environment.set("uploadUrl", body.data.uploadUrl);',
                        '  }',
                        '  if (body.data.requiredHeaders) {',
                        '    const headers = body.data.requiredHeaders;',
                        '    const contentType = headers["Content-Type"] || headers["content-type"] || "image/png";',
                        '    pm.environment.set("uploadContentType", contentType);',
                        '  }',
                        '}'
                    )
                }
            })
        }
        $items += $req
    }

    if ($items.Count -eq 0) { return $null }

    return @{
        name = $fileName
        description = $controllerDescription
        item = $items
    }
}

function New-AuthFlowFolder {
    $signupBody = @'
{
  "contact": { "name": "Admin User", "phoneNumber": "+15551234567", "email": "{{signupEmail}}" },
  "company": {
    "name": "Acme Field Services",
    "website": "https://acme.example.com",
    "companySize": "11-50",
    "address": { "addressLine1": "100 Main St", "city": "Austin", "state": "TX", "postalCode": "78701", "country": "US" }
  },
  "businessTypeIds": [1]
}
'@

    $signup = New-PostmanRequest -Name '1. Company Signup' -Method POST -Url '{{gatewayUrl}}/api/v1/signup/company' -UseAuth $false -Body $signupBody
    $signup | Add-Member -NotePropertyName event -NotePropertyValue @(@{
        listen = 'test'
        script = @{
            type = 'text/javascript'
            exec = @(
                'const body = pm.response.json();',
                'if (body.data) {',
                '  pm.environment.set("tenantId", String(body.data.tenantId));',
                '  pm.environment.set("companyId", String(body.data.companyNumber));',
                '  const m = body.data.inviteUrl.match(/[?&]token=([^&]+)/);',
                '  if (m) pm.environment.set("inviteToken", decodeURIComponent(m[1]));',
                '}'
            )
        }
    })

    $invite = New-PostmanRequest -Name '2. Start Invitation' -Method GET -Url '{{gatewayUrl}}/api/v1/invite/start?token={{inviteToken}}' -UseAuth $false
    $invite.request.description = 'Disable Follow Redirects. Copy Entra URL from Location header, open in browser, sign up, paste auth code into environment.'

    $token = @{
        name = '4. Entra Token Exchange'
        request = @{
            method = 'POST'
            auth = @{ type = 'noauth' }
            header = @(@{ key = 'Content-Type'; value = 'application/x-www-form-urlencoded'; type = 'text' })
            body = @{
                mode = 'urlencoded'
                urlencoded = @(
                    @{ key = 'grant_type'; value = 'authorization_code' }
                    @{ key = 'client_id'; value = '{{entraClientId}}' }
                    @{ key = 'client_secret'; value = '{{entraClientSecret}}' }
                    @{ key = 'code'; value = '{{authCode}}' }
                    @{ key = 'redirect_uri'; value = '{{redirectUri}}' }
                    @{ key = 'scope'; value = 'openid profile email offline_access' }
                )
            }
            url = (Convert-ToPostmanUrl -Url '{{entraAuthority}}/{{entraTenantId}}/oauth2/v2.0/token')
            description = 'Run after browser sign-up. Set authCode from callback URL.'
        }
        event = @(@{
            listen = 'test'
            script = @{
                type = 'text/javascript'
                exec = @(
                    'const body = pm.response.json();',
                    'if (body.access_token) pm.environment.set("accessToken", body.access_token);'
                )
            }
        })
    }

    return @{
        name = '00 - Authentication Flow'
        description = 'Run in order. Step 3 is manual: sign up in Entra with signupEmail, then set authCode.'
        item = @(
            $signup
            $invite
            @{ name = '3. Manual - Entra sign-up in browser'; request = @{ method = 'GET'; auth = @{ type = 'noauth' }; url = @{ raw = '(open Location URL from step 2)' }; description = 'Sign up with signupEmail. Copy code from callback into authCode.' } }
            $token
            (New-PostmanRequest -Name '5. Auth Me' -Method GET -Url '{{gatewayUrl}}/api/v1/auth/me' -UseAuth $true)
        )
    }
}

function New-UploadFileToS3Request {
    return @{
        name = 'UploadFileToS3'
        request = @{
            method = 'PUT'
            auth = @{ type = 'noauth' }
            header = @(@{ key = 'Content-Type'; value = '{{uploadContentType}}'; type = 'text' })
            body = @{ mode = 'file'; file = @{} }
            url = @{ raw = '{{uploadUrl}}' }
            description = 'Step 2 of 3: PUT binary file to the presigned S3 URL from CreateUploadUrl. Body tab: binary — select a local image file. No Bearer token or tenant headers. Expect HTTP 200 from S3, then run CompleteUpload.'
        }
        event = @(@{
            listen = 'prerequest'
            script = @{
                type = 'text/javascript'
                exec = @(
                    "if (!pm.environment.get('uploadUrl')) {",
                    "  throw new Error('Run CreateUploadUrl first to set uploadUrl.');",
                    "}",
                    "const contentType = pm.environment.get('uploadContentType') || 'image/png';",
                    "pm.request.headers.upsert({ key: 'Content-Type', value: contentType });"
                )
            }
        })
    }
}

function Add-FileUploadWorkflowToFolder {
    param($Folder)

    if ($Folder.name -ne 'FilesController') { return $Folder }

    $Folder.description = 'File upload workflow (CreateUploadUrl -> UploadFileToS3 -> CompleteUpload), metadata, and company logo lookup via {{gatewayUrl}}.'

    $newItems = @()
    foreach ($item in $Folder.item) {
        if ($item.name -eq 'CreateUploadUrl') {
            $item.request.description = 'Step 1 of 3: Request a presigned S3 upload URL. Saves fileId, uploadUrl, and uploadContentType to the environment.'
        }
        if ($item.name -eq 'CompleteUpload') {
            $item.request.description = 'Step 3 of 3: Finalize upload after S3 PUT succeeds. Generates the logo variant.'
        }
        $newItems += $item
        if ($item.name -eq 'CreateUploadUrl') {
            $newItems += New-UploadFileToS3Request
        }
    }

    $Folder.item = $newItems
    return $Folder
}

function New-Collection {
    param(
        [string]$ServiceName,
        [string]$Description,
        [array]$Folders,
        [array]$ExtraVariables = @(),
        [switch]$IncludeAuthFlow
    )

    $variables = @(
        @{ key = "apiVersion"; value = "v1" }
    ) + $ExtraVariables

    $items = @()
    if ($IncludeAuthFlow) {
        $items += New-AuthFlowFolder
    }
    $items += $Folders

    $collection = @{
        info = @{
            _postman_id = New-PostmanUuid
            name = "FGS $ServiceName"
            description = $Description
            schema = "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
        }
        variable = $variables
        auth = @{
            type = "bearer"
            bearer = @(@{ key = "token"; value = "{{accessToken}}"; type = "string" })
        }
        event = @(
            @{
                listen = "prerequest"
                script = @{
                    type = "text/javascript"
                    exec = @(
                        "if (pm.environment.get('tenantId')) {",
                        "  pm.request.headers.upsert({ key: 'X-Tenant-Id', value: pm.environment.get('tenantId') });",
                        "  pm.request.headers.upsert({ key: 'X-Company-Id', value: pm.environment.get('companyId') });",
                        "}"
                    )
                }
            }
        )
        item = $items
    }

    return $collection
}

$serviceConfigs = @(
    @{ Key = 'UserService'; Path = 'src\UserService\Fgs.User.API\Controllers'; Desc = 'Company onboarding, Entra auth, dashboard, and tenant admin APIs via {{gatewayUrl}}.'; AuthFlow = $true }
    @{ Key = 'SetupService'; Path = 'src\SetupService\Fgs.Setup.API\Controllers'; Desc = 'Platform setup catalog APIs via {{gatewayUrl}}/api/v1/{catalog}.'; AuthFlow = $false }
    @{ Key = 'NotificationService'; Path = 'src\NotificationService\Fgs.Notification.API\Controllers'; Desc = 'Notification dispatch via {{gatewayUrl}}/api/v1/notifications/...'; AuthFlow = $false }
    @{ Key = 'FileService'; Path = 'src\FileService\Fgs.File.API\Controllers'; Desc = 'Tenant S3 provisioning and file upload workflow via {{gatewayUrl}}. Upload flow: 1) CreateUploadUrl 2) UploadFileToS3 (direct PUT to S3 presigned URL) 3) CompleteUpload.'; AuthFlow = $false }
    @{ Key = 'AuditService'; Path = 'src\AuditService\Fgs.Audit.API\Controllers'; Desc = 'Credential audit trail via {{gatewayUrl}}.'; AuthFlow = $false }
    @{ Key = 'PublisherService'; Path = 'src\PublisherService\Fgs.Publisher.API\Controllers'; Desc = 'Outbox publisher worker API via {{gatewayUrl}}/api/v1/publisher/...'; AuthFlow = $false }
    @{ Key = 'ConsumerService'; Path = 'src\ConsumerService\Fgs.Consumer.API\Controllers'; Desc = 'Message consumer worker API via {{gatewayUrl}}/api/v1/consumer/...'; AuthFlow = $false }
    @{ Key = 'AssetService'; Path = 'src\AssetService\Fgs.Asset.API\Controllers'; Desc = 'Asset service scaffold (health) via {{gatewayUrl}}/api/v1/asset/...'; AuthFlow = $false }
    @{ Key = 'BillingService'; Path = 'src\BillingService\Fgs.Billing.API\Controllers'; Desc = 'Billing service scaffold (health) via {{gatewayUrl}}/api/v1/billing/...'; AuthFlow = $false }
    @{ Key = 'CommunicationService'; Path = 'src\CommunicationService\Fgs.Communication.API\Controllers'; Desc = 'Communication service scaffold (health) via {{gatewayUrl}}/api/v1/communication/...'; AuthFlow = $false }
    @{ Key = 'CrmService'; Path = 'src\CrmService\Fgs.Crm.API\Controllers'; Desc = 'CRM service scaffold (health) via {{gatewayUrl}}/api/v1/crm/...'; AuthFlow = $false }
    @{ Key = 'IntegrationService'; Path = 'src\IntegrationService\Fgs.Integration.API\Controllers'; Desc = 'Integration service scaffold (health) via {{gatewayUrl}}/api/v1/integration/...'; AuthFlow = $false }
    @{ Key = 'InventoryService'; Path = 'src\InventoryService\Fgs.Inventory.API\Controllers'; Desc = 'Inventory service scaffold (health) via {{gatewayUrl}}/api/v1/inventory/...'; AuthFlow = $false }
    @{ Key = 'ReportingService'; Path = 'src\ReportingService\Fgs.Reporting.API\Controllers'; Desc = 'Reporting service scaffold (health) via {{gatewayUrl}}/api/v1/reporting/...'; AuthFlow = $false }
    @{ Key = 'SchedulingService'; Path = 'src\SchedulingService\Fgs.Scheduling.API\Controllers'; Desc = 'Scheduling service scaffold (health) via {{gatewayUrl}}/api/v1/scheduling/...'; AuthFlow = $false }
    @{ Key = 'ServiceAgreementService'; Path = 'src\ServiceAgreementService\Fgs.ServiceAgreement.API\Controllers'; Desc = 'Service agreement scaffold (health) via {{gatewayUrl}}/api/v1/service-agreements/...'; AuthFlow = $false }
)

New-Item -ItemType Directory -Force -Path $OutputDir | Out-Null

Write-Host "Building DTO registry from Application layer..."
$script:DtoRegistry = Build-DtoRegistry -Root $RepoRoot
Write-Host "  Loaded $($script:DtoRegistry.Count) DTO types"

foreach ($svc in $serviceConfigs) {
    $controllerRoot = Join-Path $RepoRoot $svc.Path
    if (-not (Test-Path $controllerRoot)) { Write-Warning "Skip $($svc.Key): $controllerRoot"; continue }

    $files = Get-ChildItem -Path $controllerRoot -Filter '*Controller.cs' -Recurse |
        Where-Object { $_.Name -notmatch '^AuthController\..+\.cs$' } |
        Sort-Object FullName
    $folders = @()

    foreach ($file in $files) {
        $folder = Parse-ControllerFile -FilePath $file.FullName -ServiceKey $svc.Key -DtoRegistry $script:DtoRegistry
        if ($null -ne $folder) {
            if ($svc.Key -eq 'FileService') {
                $folder = Add-FileUploadWorkflowToFolder -Folder $folder
            }
            $folders += $folder
        }
    }

    if ($folders.Count -eq 0) { continue }

    $includeAuth = ($svc.Key -eq 'UserService')
    $collection = New-Collection -ServiceName $svc.Key -Description $svc.Desc -Folders $folders -IncludeAuthFlow:$includeAuth
    $outFile = Join-Path $OutputDir "$($svc.Key).postman_collection.json"
    $collection | ConvertTo-Json -Depth 100 | Set-Content -Path $outFile -Encoding UTF8
    Write-Host "Generated $outFile ($($files.Count) controllers)"
}

Write-Host "Done. Import FGS-Globals.postman_environment.json and select it in Postman."
