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

function Get-EntityIdVarName {
    param([string]$ControllerName)

    $entity = $ControllerName -replace 'Controller$', ''
    switch ($entity) {
        'GLBreak' { return 'glBreakId' }
        'Attachment' { return 'attachmentId' }
        'TenantStorage' { return 'tenantId' }
        default { return ((ConvertTo-CamelCase $entity) + 'Id') }
    }
}

function Format-PostmanIdVar {
    param([string]$VarName)
    return ('{{' + $VarName + '}}')
}

function Get-PostmanVarName {
    param(
        [string]$ParamName,
        [string]$EntityIdVar = 'recordId'
    )
    switch ($ParamName) {
        'id' { $EntityIdVar }
        default { $ParamName }
    }
}

function Convert-RouteToPostmanPath {
    param(
        [string]$Template,
        [string]$EntityIdVar = 'recordId'
    )
    if ([string]::IsNullOrWhiteSpace($Template)) { return '' }
    return [regex]::Replace($Template, '\{(\w+)(?::(?:long|guid))?\}', {
        param($m)
        $varName = Get-PostmanVarName -ParamName $m.Groups[1].Value -EntityIdVar $EntityIdVar
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

    # Most live APIs are mounted at /api/v1/{controllerRoute} with no service prefix.
    # Only worker/scaffold services (and BFF) keep a service segment in the public path.
    $prefixedServices = @{
        'BffService' = 'bff'
        'CrmService' = 'crm'
        'SchedulingService' = 'scheduling'
        'BillingService' = 'billing'
        'ReportingService' = 'reporting'
        'IntegrationService' = 'integration'
        'ServiceAgreementService' = 'serviceagreements'
        'CommunicationService' = 'communication'
        'PublisherService' = 'publisher'
        'ConsumerService' = 'consumer'
    }

    # Health endpoints for services that otherwise publish at root still use a service prefix when one exists historically.
    if ($RouteTemplate -eq 'health' -and $ServiceKey -eq 'SetupService') {
        return '/api/v1/setup/health'
    }
    if ($RouteTemplate -eq 'health' -and $ServiceKey -eq 'AuditService') {
        return '/api/v1/audit/health'
    }
    if ($RouteTemplate -eq 'health' -and $ServiceKey -eq 'FileService') {
        return '/api/v1/file/health'
    }
    if ($RouteTemplate -eq 'health' -and $ServiceKey -eq 'InventoryService') {
        return '/api/v1/inventory/health'
    }
    if ($RouteTemplate -eq 'health' -and $ServiceKey -eq 'AssetService') {
        return '/api/v1/asset/health'
    }
    if ($RouteTemplate -eq 'health' -and $ServiceKey -eq 'NotificationService') {
        return '/api/v1/notification/health'
    }

    if ($prefixedServices.ContainsKey($ServiceKey)) {
        return "/api/v1/$($prefixedServices[$ServiceKey])/$RouteTemplate"
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

function Convert-PascalToTitle {
    param([string]$Name)
    if ([string]::IsNullOrWhiteSpace($Name)) { return $Name }
    $spaced = [regex]::Replace($Name, '([a-z0-9])([A-Z])', '$1 $2')
    $spaced = [regex]::Replace($spaced, '([A-Z]+)([A-Z][a-z])', '$1 $2')
    return $spaced.Trim()
}

function Get-EntityStemFromController {
    param([string]$ControllerName)
    return ($ControllerName -replace 'Controller$', '')
}

function Get-EntitySampleProfile {
    param([string]$EntityStem)

    $profiles = @{
        BusinessType = @{ Code = 'PLUMB'; Name = 'Plumbing'; Description = 'Residential and commercial plumbing services' }
        BillingCategory = @{ Code = 'NI'; Name = 'Non Inventory'; Description = 'Non-inventory billing category' }
        JobCategory = @{ Code = 'REPAIR'; Name = 'Repair'; Description = 'General repair work category' }
        JobType = @{ Code = 'SVC'; Name = 'Service Call'; Description = 'Standard service call job type' }
        JobTypeCategory = @{ Code = 'SVC-REPAIR'; Name = 'Service Repair'; Description = 'Links service job types to repair categories' }
        JobTypeTask = @{ Code = 'DIAG'; Name = 'Diagnose Issue'; Description = 'Initial diagnosis task for the job type' }
        LaborRateType = @{ Code = 'STD'; Name = 'Standard Labor'; Description = 'Standard technician labor rate type' }
        LeadDisqualificationReason = @{ Code = 'NOBUDGET'; Name = 'No Budget'; Description = 'Lead has insufficient budget' }
        LeadSource = @{ Code = 'WEB'; Name = 'Website'; Description = 'Lead originated from company website' }
        LeadStatus = @{ Code = 'NEW'; Name = 'New Lead'; Description = 'Newly captured lead status' }
        CommunicationTemplate = @{ Code = 'INVITE'; Name = 'Invite Email'; Description = 'Company invitation email template' }
        Credential = @{ Code = 'DATABASE'; Name = 'Platform Database Connections'; Description = 'Platform database connection strings' }
        Employee = @{ Code = 'EMP001'; Name = 'Alex Rivera'; Description = 'Field technician employee profile' }
        GLBreak = @{ Code = 'LABOR'; Name = 'Labor GL'; Description = 'General ledger break for labor charges' }
        PaymentMethod = @{ Code = 'CHECK'; Name = 'Check'; Description = 'Payment by check' }
        PaymentTerm = @{ Code = 'NET30'; Name = 'Net 30'; Description = 'Payment due in 30 days' }
        PostalCode = @{ Code = '78701'; Name = 'Austin Downtown'; Description = 'Austin TX postal coverage' }
        PricingMatrix = @{ Code = 'FLATLABOR'; Name = 'Flat Labor'; Description = 'Flat labor pricing matrix' }
        PricingMatrixLabor = @{ Code = 'LABOR'; Name = 'Standard Labor Line'; Description = 'Labor rate line for pricing matrix' }
        PricingMatrixLaborTier = @{ Code = 'TIER1'; Name = 'First Hour Tier'; Description = 'Labor duration tier for tiered pricing' }
        PricingMatrixMaterialTier = @{ Code = 'MAT0'; Name = 'Material Tier 0'; Description = 'Material cost adjustment tier' }
        PricingMatrixOther = @{ Code = 'NI'; Name = 'Non-Inventory'; Description = 'Other pricing matrix category item' }
        ResolutionCode = @{ Code = 'FIXED'; Name = 'Issue Resolved'; Description = 'Work completed and issue fixed' }
        SalesActivityOutcome = @{ Code = 'WON'; Name = 'Won'; Description = 'Sales activity resulted in a win' }
        SalesActivityType = @{ Code = 'CALL'; Name = 'Phone Call'; Description = 'Outbound sales phone call' }
        SalesDispositionReason = @{ Code = 'NOBUDGET'; Name = 'No Budget'; Description = 'Opportunity closed due to budget' }
        SalesPipelineStatus = @{ Code = 'LEAD_NEW'; Name = 'New Lead'; Description = 'Newly captured lead pipeline status' }
        SetupDescription = @{ Code = 'NOTES'; Name = 'Work Notes'; Description = 'Standard work notes description type' }
        Tag = @{ Code = 'VIP'; Name = 'VIP Customer'; Description = 'High-priority customer tag' }
        Tax = @{ Code = 'COMBINED'; Name = 'Combined Tax'; Description = 'State and county combined tax' }
        TaxAuthority = @{ Code = 'TXSTATE'; Name = 'Texas State'; Description = 'Texas state tax authority' }
        TechSkillLevel = @{ Code = 'JOURNEY'; Name = 'Journeyman'; Description = 'Journeyman technician skill level' }
        TechTrade = @{ Code = 'HVAC'; Name = 'HVAC Services'; Description = 'Heating and cooling trade' }
        Timeslot = @{ Code = 'AM'; Name = 'Morning'; Description = 'Morning appointment timeslot' }
        TitleOfCourtesy = @{ Code = 'MR'; Name = 'Mr.'; Description = 'Courtesy title Mr.' }
        UniversalPricingService = @{ Code = 'DIAGFEE'; Name = 'Diagnostic Fee'; Description = 'Standard diagnostic service fee' }
        UniversalMatrixTier = @{ Code = 'STD'; Name = 'Standard'; Description = 'Standard pricing tier multiplier' }
        UniversalMatrixSizeTier = @{ Code = 'MED'; Name = 'Medium'; Description = 'Medium size tier multiplier' }
        UniversalMatrixItem = @{ Code = 'FILTER'; Name = 'Air Filter'; Description = 'Base priced matrix item' }
        UniversalMatrixFrequencyDiscount = @{ Code = 'MONTHLY'; Name = 'Monthly'; Description = 'Monthly frequency discount' }
        UniversalMatrixOneTimeFee = @{ Code = 'SETUP'; Name = 'Setup Fee'; Description = 'One-time setup fee' }
        UniversalMatrixAddOn = @{ Code = 'WARRANTY'; Name = 'Extended Warranty'; Description = 'Optional add-on price' }
        Vehicle = @{ Code = 'TRK-01'; Name = 'Service Truck 01'; Description = 'Primary service vehicle' }
        VehicleMaintenance = @{ Code = 'OIL'; Name = 'Oil Change'; Description = 'Scheduled oil change maintenance' }
        Zone = @{ Code = 'NORTH'; Name = 'North Zone'; Description = 'Northern service coverage zone' }
        ApiClient = @{ Code = 'FSINT'; Name = 'Field Service Integration'; Description = 'API client for field service integrations' }
        ApiEvent = @{ Code = 'WO_CREATED'; Name = 'Work Order Created'; Description = 'Fired when a work order is created' }
        ApiSecret = @{ Code = 'PRIMARY'; Name = 'Primary Secret'; Description = 'Primary API client secret' }
        ApiWebhook = @{ Code = 'WO_HOOK'; Name = 'Work Order Webhook'; Description = 'Webhook for work order events' }
        ApiWebhookSubscription = @{ Code = 'WO_SUB'; Name = 'Work Order Subscription'; Description = 'Subscription to work order created events' }
        DataAccess = @{ Code = 'OWN_COMP'; Name = 'Own Company'; Description = 'Restrict access to the user company' }
        DataAccessScope = @{ Code = 'COMPANY'; Name = 'Company Scope'; Description = 'Company-level data access scope' }
        Permission = @{ Code = 'USER_VIEW'; Name = 'View Users'; Description = 'Allows viewing user records' }
        PublicEndpoint = @{ Code = 'LOCAL'; Name = 'Local API Gateway'; Description = 'Local development public endpoint' }
        Role = @{ Code = 'ADMIN'; Name = 'Administrator'; Description = 'Full administrative access role' }
        RoleDataAccess = @{ Code = 'ADMIN_OWN'; Name = 'Admin Own Company'; Description = 'Administrator own-company data access' }
        RolePermission = @{ Code = 'ADMIN_USER_VIEW'; Name = 'Admin View Users'; Description = 'Administrator permission to view users' }
        User = @{ Code = 'OFFICE'; Name = 'Office User'; Description = 'Office staff user account' }
        UserRole = @{ Code = 'OFFICE_ADMIN'; Name = 'Office Administrator'; Description = 'Assigns administrator role to a user' }
        ServiceSetup = @{ Code = 'DEFAULT'; Name = 'Default Service Setup'; Description = 'Tenant service configuration defaults' }
        Asset = @{ Code = 'ASSET01'; Name = 'Primary Asset'; Description = 'Customer installed asset' }
        AssetAttribute = @{ Code = 'SERIAL'; Name = 'Serial Number'; Description = 'Asset serial number attribute' }
        AssetAttributeOption = @{ Code = 'OPTION1'; Name = 'Option 1'; Description = 'Dropdown option for asset attribute' }
        AssetManufacturer = @{ Code = 'CARRIER'; Name = 'Carrier'; Description = 'HVAC equipment manufacturer' }
        AssetModel = @{ Code = '24ACC'; Name = '24ACC Comfort'; Description = 'Residential comfort series model' }
        AssetStatus = @{ Code = 'ACTIVE'; Name = 'Active'; Description = 'Asset is installed and active' }
        AssetType = @{ Code = 'HVAC'; Name = 'HVAC Unit'; Description = 'Heating and cooling asset type' }
        AssetWarranty = @{ Code = 'OEM'; Name = 'OEM Warranty'; Description = 'Original manufacturer warranty' }
        InventoryLocation = @{ Code = 'WH-MAIN'; Name = 'Main Warehouse'; Description = 'Primary parts warehouse' }
        Vendor = @{ Code = 'VND01'; Name = 'Acme Supply Co'; Description = 'Primary parts vendor' }
        TruckStockTemplate = @{ Code = 'STDTRK'; Name = 'Standard Truck Stock'; Description = 'Default truck stock template' }
        Attachment = @{ Code = 'GENERAL'; Name = 'General Attachment'; Description = 'General file attachment' }
    }

    if ($profiles.ContainsKey($EntityStem)) {
        return $profiles[$EntityStem]
    }

    $title = Convert-PascalToTitle $EntityStem
    $code = ($EntityStem -creplace '[a-z]', '').ToUpperInvariant()
    if ([string]::IsNullOrWhiteSpace($code) -or $code.Length -lt 2) {
        $code = $EntityStem.ToUpperInvariant()
        if ($code.Length -gt 8) { $code = $code.Substring(0, 8) }
    }
    return @{
        Code = $code
        Name = $title
        Description = "$title used in field service operations"
    }
}

function Format-JsonString {
    param([string]$Value)
    return ('"' + ($Value -replace '\\', '\\' -replace '"', '\"') + '"')
}

function Get-PropertySampleOverride {
    param(
        [string]$PropertyName,
        [string]$EntityStem,
        [hashtable]$Profile
    )

    # Value '__null__' means JSON null; otherwise a string literal.
    $propertyOverrides = @{
        ApplicationName = 'Field Service Integration'
        ContactName = 'Ops Admin'
        ContactEmail = 'ops@example.com'
        ContactTitle = 'Buyer'
        Module = 'Users'
        Resource = 'User'
        Action = 'View'
        Operator = 'Equals'
        ScopeType = 'Company'
        ScopeValue = 'OWN'
        AuthenticationType = 'None'
        AuthenticationValue = '__null__'
        EndpointType = 'API'
        EnvironmentCode = 'LOCAL'
        EventCode = 'WO_CREATED'
        EventCategory = 'WorkOrder'
        EndpointUrl = 'https://hooks.example.com/fgs/workorders'
        BaseUrl = 'https://developer.fsm.com'
        PermissionCode = 'USER_VIEW'
        RoleCode = 'ADMIN'
        DataAccessCode = 'OWN_COMP'
        TradeCode = 'HVAC'
        CountryCode = 'US'
        StateProvinceCode = 'TX'
        StateProvince = 'TX'
        BillHoursFromDispatchOrArrive = 'Dispatch'
        InvoiceNumberPrefix = 'INV'
        QuoteNumberPrefix = 'QTE'
        PONumberPrefix = 'PO'
        WorkOrderNumberPrefix = 'WO'
        InvoiceBatchNumberFormat = 'INV-{YYYY}-{####}'
        Make = 'Ford'
        Model = 'Transit'
        Color = 'White'
        LicensePlate = 'FGS-100'
        LicensePlateState = 'TX'
        PurchasedFrom = 'Austin Ford'
        OwnershipCompany = 'Fleet Lease Partners'
        BusinessUnit = 'Field Services'
        ServiceProvider = 'Acme Field Services'
        LegalName = "$($Profile.Name) LLC"
        VendorAccountNumber = 'ACCT-1001'
        TaxIdNumber = '12-3456789'
        FirstName = 'Alex'
        LastName = 'Rivera'
        MiddleName = '__null__'
        Subject = "You are invited to {{CompanyName}}"
        ShortNote = 'Follow up with customer'
        Notes = "$($Profile.Name) notes"
        TaskName = $Profile.Name
        BillingCategoryName = $Profile.Name
        BillingCategoryType = 'NI'
        InventoryLocationType = 'WAREHOUSE'
        VendorType = 'VENDOR'
        VendorStatus = 'ACTIVE'
        OwnershipType = 'Owned'
        InputType = 'TEXT'
        CommunicationChannel = 'Email'
        TemplateType = 'Transactional'
        WarrantyType = 'OEM'
        DueDateMethod = 'NetDays'
        WarehouseType = 'Warehouse'
        AttributeCode = 'SERIAL'
        AttributeName = 'Serial Number'
        OptionCode = 'OPTION1'
        OptionName = 'Option 1'
        StatusCode = $Profile.Code
        StatusName = $Profile.Name
        OutcomeCode = $Profile.Code
        OutcomeName = $Profile.Name
        ActivityTypeCode = $Profile.Code
        ActivityTypeName = $Profile.Name
        DispositionReasonCode = $Profile.Code
        DispositionReasonName = $Profile.Name
        SourceCode = $Profile.Code
        SourceName = $Profile.Name
        ReasonCode = $Profile.Code
        ReasonName = $Profile.Name
        ResolutionCode = $Profile.Code
        ResolutionName = $Profile.Name
        DescriptionTypeCode = $Profile.Code
        SubCategoryCode = 'SUB'
        JobTypeCode = 'SVC'
        TaxCode = 'COMBINED'
        VendorCode = 'VND01'
        VendorName = 'Acme Supply Co'
        InventoryLocationCode = 'WH-MAIN'
        WarehouseCode = 'WH01'
        RegionCode = 'TX'
        TagCode = 'VIP'
        PostalCode = '78701'
        DisplayName = $Profile.Name
        Secret = '__null__'
        EmployeeNumber = 'EMP001'
        LegalFirstName = 'Alex'
        LegalMiddleName = '__null__'
        LegalLastName = 'Rivera'
        TemplateCode = 'STDTRK'
        BreakLabel = 'Labor'
    }

    if ($EntityStem -eq 'SetupDescription' -and $PropertyName -eq 'Body') {
        return 'Customer reported intermittent cooling. Inspected and cleaned condensate drain.'
    }
    if ($EntityStem -eq 'CommunicationTemplate' -and $PropertyName -eq 'Body') {
        return 'Hello, use {{InviteUrl}} to join {{CompanyName}}.'
    }

    if ($propertyOverrides.ContainsKey($PropertyName)) {
        return $propertyOverrides[$PropertyName]
    }

    switch -Regex ($PropertyName) {
        '^(Code|.*Code)$' {
            if ($PropertyName -eq 'Code' -or $PropertyName -eq ($EntityStem + 'Code')) { return $Profile.Code }
            $propStem = $PropertyName -replace 'Code$', ''
            if ($EntityStem -match ($propStem + '$')) { return $Profile.Code }
            return $Profile.Code
        }
        '^(Name|DisplayName|StatusName|OutcomeName|ActivityTypeName|DispositionReasonName|SourceName|ReasonName|ResolutionName|TaskName|BillingCategoryName|VendorName|AttributeName|OptionName)$' {
            return $Profile.Name
        }
        '^Description$' { return $Profile.Description }
    }

    return $null
}

function Get-SampleJsonValue {
    param(
        [string]$PropertyName,
        [string]$CsType,
        [bool]$IsPatch = $false,
        [hashtable]$Registry = $null,
        [int]$IndentLevel = 0,
        [string]$EntityStem = 'Record'
    )

    $nullable = $CsType.EndsWith('?')
    $baseType = $CsType.TrimEnd('?')
    $profile = Get-EntitySampleProfile $EntityStem

    if ($Registry -and $Registry.ContainsKey($baseType)) {
        $nested = Get-DtoSampleBody -DtoType $baseType -Registry $Registry -MethodName 'Create' -IndentLevel $IndentLevel -EntityStem $EntityStem
        if ($nested) { return $nested }
    }

    if ($baseType -match '^(IReadOnlyList|IEnumerable|ICollection|List|IList)<(?<item>.+)>$') {
        $itemType = $Matches.item.Trim()
        if ($itemType -eq 'string') {
            if ($PropertyName -match 'TradeCode') { return '["PLUMB"]' }
            return ('["{0}"]' -f $profile.Code)
        }
        if ($Registry -and $Registry.ContainsKey($itemType)) {
            $nested = Get-DtoSampleBody -DtoType $itemType -Registry $Registry -MethodName 'Create' -IndentLevel 0 -EntityStem $EntityStem
            if ($nested) { return "[$nested]" }
        }
        return '[]'
    }

    if ($IsPatch) {
        if ($PropertyName -eq 'IsActive') { return 'true' }
        if ($baseType -eq 'bool') { return 'null' }
        if ($baseType -in @('string', 'String')) {
            if ($PropertyName -match 'Name|DisplayName|TaskName|Subject|Body|Description|Notes|ShortNote') {
                return (Format-JsonString ("Updated {0}" -f $profile.Name))
            }
            return 'null'
        }
        if ($baseType -in @('short', 'int', 'long', 'decimal', 'double', 'float')) { return 'null' }
        if ($baseType -eq 'DateOnly') { return 'null' }
        if ($baseType -eq 'TimeSpan') { return 'null' }
        if ($baseType -eq 'Guid') { return 'null' }
        return 'null'
    }

    if ($nullable -and $PropertyName -match 'Id$|SyncToken|ExternalSystemId|IconFileId|AddressId|LogoFileId|NextSalesPipelineStatusId|PaymentTermId|FgsSetupZoneId|FgsSetupTaxId|FgsSetupTechTradeId|TenantId|CompanyId') {
        if ($PropertyName -eq 'TenantId') { return 'null' }
        if ($PropertyName -eq 'CompanyId') { return 'null' }
        if ($PropertyName -match 'WarehouseId|VehicleId|FgsSetupTaxId|FgsSetupTaxAuthorityId|JobTypeCategoryId') {
            return (Format-PostmanIdVar (ConvertTo-CamelCase $PropertyName))
        }
        return 'null'
    }

    # Domain enums serialized as numbers
    if ($baseType -eq 'TimeCardOption' -or $PropertyName -eq 'TimeCardOptionId') { return '2' }

    switch ($baseType) {
        'bool' {
            switch -Regex ($PropertyName) {
                '^IsActive$' { return 'true' }
                '^IsSystem$|^IsSystemDefined$|^IsSystemGenerated$|^Is1099Eligible$|^IsExternalSystemRecord$|^IsPurchaser$' { return 'false' }
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
            if ($PropertyName -eq 'UsedFor') { return '1' }
            if ($PropertyName -match 'TimeoutSeconds') { return '30' }
            if ($PropertyName -match 'MaximumRetryCount') { return '3' }
            if ($PropertyName -match 'EventVersion') { return '1' }
            if ($PropertyName -match 'RateLimitPerMinute') { return '60' }
            return '1'
        }
        'int' {
            if ($PropertyName -match 'Minutes|Mileage|NumberOfDays|VehicleMaintenanceTypeId|GloResolutionTypeId') { return '1' }
            if ($PropertyName -match 'WorkLocationRadiusForAutoArrive') { return '100' }
            if ($PropertyName -match 'RateLimitPerMinute') { return '60' }
            return '1'
        }
        'long' {
            if ($PropertyName -match 'Id$') { return (Format-PostmanIdVar (ConvertTo-CamelCase $PropertyName)) }
            if ($PropertyName -match 'StartNumber$') { return '1000' }
            return '1'
        }
        'decimal' {
            if ($PropertyName -match 'Latitude|Longitude') { return 'null' }
            if ($PropertyName -match 'Multiplier') { return '1.00' }
            if ($PropertyName -match 'Percent|TaxPercent') { return '8.25' }
            if ($PropertyName -match 'TripChargeAmount') { return '75.00' }
            if ($PropertyName -match 'RegularRate|OvertimeRate|DoubleTimeRate') { return '45.00' }
            if ($nullable -and $PropertyName -match 'LaborBurdenValue') { return 'null' }
            if ($PropertyName -match 'Price|Cost|Amount|PurchasePrice|BaseRate') { return '100.00' }
            if ($PropertyName -match 'TargetQuantity') { return '5.00' }
            if ($PropertyName -match 'MinimumQuantity') { return '1.00' }
            if ($nullable) { return 'null' }
            return '0.00'
        }
        'double' { return '0.0' }
        'float' { return '0.0' }
        'DateOnly' {
            if ($nullable) {
                if ($PropertyName -match 'Hire|Birth|Purchase|Install|Manufacture') { return '"2024-03-15"' }
                return 'null'
            }
            return '"2026-06-21"'
        }
        'TimeSpan' {
            if ($PropertyName -match 'Begin|Start|Arrived|OTStart|DTStart') { return '"08:00:00"' }
            if ($PropertyName -match 'End|Delayed|Completion|OTEnd|DTEnd') { return '"17:00:00"' }
            return '"09:00:00"'
        }
        'Guid' { return '"11111111-1111-1111-1111-111111111111"' }
        default {
            if ($PropertyName -eq 'ContactEmail') { return '"ops@example.com"' }
            if ($PropertyName -match 'Email') { return '"office@example.com"' }
            if ($PropertyName -match 'Phone|Mobile') { return '"+15551234567"' }
            if ($PropertyName -eq 'EndpointUrl') { return '"https://hooks.example.com/fgs/workorders"' }
            if ($PropertyName -eq 'BaseUrl') { return '"https://developer.fsm.com"' }
            if ($PropertyName -match 'Website|Url') { return '"https://acme.example.com"' }
            if ($PropertyName -eq 'UsedFor') { return '1' }
            if ($PropertyName -eq 'VIN' -or $PropertyName -eq 'Vin') { return '"1FTBW2CM5PKA12345"' }
            if ($PropertyName -eq 'AddressLine1' -or $PropertyName -eq 'Address1') { return '"100 Main St"' }
            if ($PropertyName -eq 'AddressLine2' -or $PropertyName -eq 'Address2') { return '"Suite 200"' }
            if ($PropertyName -match '^AddressLine[34]$') { return 'null' }
            if ($PropertyName -eq 'City') { return '"Austin"' }
            if ($PropertyName -eq 'State') { return '"TX"' }
            if ($PropertyName -eq 'County') { return 'null' }
            if ($PropertyName -eq 'Country') { return '"US"' }
            if ($PropertyName -eq 'FormattedAddress' -or $PropertyName -eq 'PlaceId') { return 'null' }
            if ($PropertyName -match 'BackgroundColor') { return '"#3366FF"' }
            if ($PropertyName -match 'TextColor') { return '"#FFFFFF"' }

            $override = Get-PropertySampleOverride -PropertyName $PropertyName -EntityStem $EntityStem -Profile $profile
            if ($null -eq $override) {
                if ($nullable) { return 'null' }
                return (Format-JsonString $profile.Name)
            }
            if ($override -eq '__null__') { return 'null' }
            return (Format-JsonString ([string]$override))
        }
    }
}

function Build-DtoRegistry {
    param([string]$Root)

    $registry = @{}
    $dtoFiles = Get-ChildItem -Path (Join-Path $Root 'src') -Filter '*Dtos.cs' -Recurse -ErrorAction SilentlyContinue |
        Where-Object { $_.FullName -match 'Application[\\/](Features|Common)[\\/]' }

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
                if ($line -match '^(?<type>(?:[\w\.]+(?:\s*<\s*[^>]+>\s*)?)\??)\s+(?<name>\w+)(?:\s*=\s*.+)?$') {
                    $props += [pscustomobject]@{
                        Name = $Matches.name
                        CsType = ($Matches.type -replace '\s+', '')
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
        [string]$MethodName,
        [int]$IndentLevel = 0,
        [string]$EntityStem = 'Record'
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

    $pad = '  ' * ($IndentLevel + 1)
    $closePad = '  ' * $IndentLevel
    $lines = @()
    foreach ($p in $props) {
        $value = Get-SampleJsonValue -PropertyName $p.Name -CsType $p.CsType -IsPatch:$isPatch -Registry $Registry -IndentLevel ($IndentLevel + 1) -EntityStem $EntityStem
        $lines += ('{0}"{1}": {2}' -f $pad, (ConvertTo-CamelCase $p.Name), $value)
    }

    return "{`n" + ($lines -join ",`n") + "`n$closePad}"
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
    $entityIdVar = Get-EntityIdVarName $fileName

    $controllerDescription = "$fileName - $routeTemplate"

    $items = @()
    $methodRegex = [regex]::Matches($content, '(?s)((?:\s*///[^\r\n]*\r?\n)*)\s*(?:\[(?:AllowAnonymous|Authorize[^\]]*)\]\s*)*(\[Http(Get|Post|Put|Patch|Delete)(?:\("([^"]*)"(?:[^)]*)?\))?\])\s*(?:\[[^\]]+\]\s*)*public\s+(?:async\s+)?(?:Task<(?:IActionResult|ActionResult(?:<[^>]+>)?)>|ContentResult|IActionResult)\s+(\w+)\s*\(')

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
        $pathSuffix = Convert-RouteToPostmanPath -Template $routeSuffix -EntityIdVar $entityIdVar
        if ($routeSuffix -match '^~?/') {
            $absolutePath = ($routeSuffix -replace '^~/','/') -replace 'v\{version:apiVersion\}','v1'
            $absolutePath = Convert-RouteToPostmanPath -Template $absolutePath.TrimStart('/') -EntityIdVar $entityIdVar
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
        if ($methodName -eq 'Lookup') {
            $queryItems = @(Get-ListFilterQueryItemsFromBlock $signatureBlock)
        }
        if ($methodName -eq 'GetActive' -and $routeTemplate -eq 'communicationtemplates') {
            $query = @{ tenantId = '{{tenantId}}'; companyId = '{{companyId}}'; templateType = 'Email'; code = 'INVITE' }
            $headers['X-Internal-Service-Key'] = '{{internalServiceKey}}'
            $useAuth = $false
        }
        if ($methodName -eq 'Get' -and $fileName -eq 'DashboardController') {
            $fullPath = '{{gatewayUrl}}/api/v1/dashboard?token={{accessToken}}'
            $useAuth = $false
        }
        if ($httpAttr -in @('Post','Put','Patch') -and $methodName -notin @('EntraCallback','EntraConnector','CompanySignup','Start','Upload')) {
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
  "businessTypeIds": [1],
  "authenticationMethod": 2
}
'@
        }
        if ($methodName -eq 'EntraConnector') {
            $body = '{ "email": "{{signupEmail}}", "objectId": null }'
        }
        if ($fileName -eq 'TenantController' -and $methodName -eq 'UpdateDetails') {
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
        if ($fileName -eq 'TenantController' -and $methodName -eq 'UpdateStatus') {
            $body = '{ "fgsTenantStatusId": 3 }'
        }
        if ($fileName -eq 'TenantController' -and $methodName -eq 'UpdateStorageBucket') {
            $body = '{ "storageBucketName": "fgs-dev-tenant-{{tenantId}}-demo" }'
        }
        if ($fileName -eq 'TenantCompanyBusinessTypeController' -and $methodName -eq 'AddCompanyBusinessTypes') {
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
        if ($fileName -eq 'CredentialController') {
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
        if ($fileName -eq 'AttachmentController') {
            if ($methodName -eq 'Upload') {
                $body = $null
            }
            if ($methodName -eq 'List') {
                $queryItems = @(
                    (Get-StandardPaginationQueryItems) + @(Get-ListFilterQueryItemsFromBlock $signatureBlock)
                ) | Where-Object { $_.key -ne 'isActive' }
                $queryItems += @(
                    @{ key = 'entityType'; value = 'Company'; description = '' }
                    @{ key = 'entityId'; value = '{{companyId}}'; description = '' }
                    @{ key = 'isVisibleToCustomer'; value = 'true'; description = '' }
                    @{ key = 'isVisibleToFieldTechnician'; value = 'true'; description = '' }
                )
            }
            if ($methodName -eq 'BulkDeleteByEntity') {
                $query = @{ entityType = 'Company'; entityId = '{{companyId}}'; category = 'general' }
            }
            if ($methodName -in @('Download', 'Thumbnail')) {
                $headers['Accept'] = '*/*'
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
        if ($fileName -eq 'NotificationController' -and $methodName -eq 'Dispatch') {
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
    "InviteUrl": "https://developer.fsm.com/api/v1/invite/start?token={{inviteToken}}"
  }
}
'@
        }
        if ($fileName -eq 'CredentialAuditController' -and $methodName -eq 'Record') {
            $body = @'
{
  "tenantId": 4,
  "companyId": 1,
  "credentialId": "11111111-1111-1111-1111-111111111111",
  "actionType": "READ",
  "remarks": "Credential read during configuration resolve",
  "oldVersionNo": null,
  "newVersionNo": 1,
  "createdBy": "setup-service"
}
'@
        }
        if ($fileName -eq 'TechTradeController') {
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
        if ($fileName -eq 'GLBreakController') {
            if ($methodName -eq 'Lookup') {
                $query = @{ activeOnly = 'true' }
            }
            $headers['X-Tenant-Id'] = '{{tenantId}}'
            $headers['X-Company-Id'] = '{{companyId}}'
        }
        if ($fileName -eq 'TaxController') {
            if ($methodName -eq 'Create') {
                $body = @'
{
  "taxCode": "COMBINED",
  "name": "Combined Tax",
  "isExternalSystemRecord": false,
  "externalSystemId": null,
  "syncToken": null,
  "showTaxDetail": true,
  "description": "State + county combined tax"
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
  "description": "State + county combined tax"
}
'@
            }
            if ($methodName -eq 'Patch') {
                $body = @'
{
  "name": "Combined Tax Updated",
  "description": "Patched description",
  "isActive": true
}
'@
            }
        }
        if ($fileName -eq 'PricingMatrixController') {
            if ($methodName -eq 'Create') {
                $body = Get-PricingMatrixCreateBody -Variant 'FlatLabor'
            }
            if ($methodName -eq 'Update') {
                $body = @'
{
  "name": "FLATLABOR",
  "description": "Flat Labor Pricing Updated",
  "isDefault": false,
  "isLaborTierStructure": false,
  "isLaborRateBySkillLevel": false,
  "priceAdjustmentTypeId": null,
  "effectiveFrom": null,
  "effectiveTo": null,
  "isMobileVisible": true
}
'@
            }
            if ($methodName -eq 'Patch') {
                $body = @'
{
  "description": "Pricing Matrix Patched",
  "isDefault": false,
  "isActive": true
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
                $entityStem = Get-EntityStemFromController $fileName
                $generatedBody = Get-DtoSampleBody -DtoType $dtoType -Registry $DtoRegistry -MethodName $methodName -EntityStem $entityStem
                if ($generatedBody) { $body = $generatedBody }
            }
        }

        $displayName = $methodName
        if ($docSummary -and $docSummary.Length -le 48 -and $docSummary -notmatch '[.!?]') {
            $displayName = $docSummary
        }
        $description = if ($docSummary) { $docSummary } else { $methodName }
        $req = New-PostmanRequest -Name $displayName -Method $verb -Url $fullPath -UseAuth $useAuth -Description $description -Query $query -QueryItems $queryItems -Body $body -Headers $headers
        $createIdScript = @(
            'const body = pm.response.json();',
            'if (body.success && body.data && body.data.id) {',
            ('  pm.environment.set("{0}", String(body.data.id));' -f $entityIdVar),
            '}'
        )
        if ($methodName -eq 'Create' -and $httpAttr -eq 'Post') {
            $req['event'] = @(@{
                listen = 'test'
                script = @{
                    type = 'text/javascript'
                    exec = $createIdScript
                }
            })
        }
        if ($fileName -eq 'AttachmentController' -and $methodName -eq 'Upload') {
            $req['event'] = @(@{
                listen = 'test'
                script = @{
                    type = 'text/javascript'
                    exec = @(
                        'const body = pm.response.json();',
                        'if (body.success && body.data && body.data.attachmentId) {',
                        '  pm.environment.set("attachmentId", String(body.data.attachmentId));',
                        '  pm.environment.set("fileId", String(body.data.attachmentId));',
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
  "businessTypeIds": [1],
  "authenticationMethod": 2
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

function New-UiLoginFlowFolder {
    $loginBody = @'
{
  "email": "{{signupEmail}}"
}
'@

    $startLogin = New-PostmanRequest -Name '1. Start Login' -Method POST -Url '{{gatewayUrl}}/api/v1/login' -UseAuth $false -Body $loginBody
    $startLogin.request.description = 'POST with active platform user email. Copy data.redirectUrl and open in browser. Callback state is user:{userId}.'

    $loginCallback = New-PostmanRequest -Name '3. Entra Login Callback' -Method GET -Url '{{gatewayUrl}}/api/v1/auth/entra/callback?code={{authCode}}&state=user:{{loginUserId}}' -UseAuth $false
    $loginCallback.request.description = 'Invitation-free callback. state must be user:{platformUserId}.'

    return @{
        name = '01 - UI Login Flow'
        description = 'UI login: validates active user and returns Entra redirect URL (no invitation).'
        item = @(
            $startLogin
            @{ name = '2. Manual - Entra sign-in in browser'; request = @{ method = 'GET'; auth = @{ type = 'noauth' }; url = @{ raw = '(open redirectUrl from step 1)' }; description = 'Sign in with the same email. Copy code from callback; set loginUserId from state if needed.' } }
            $loginCallback
            (New-PostmanRequest -Name '4. Get Me' -Method GET -Url '{{gatewayUrl}}/api/v1/auth/me' -UseAuth $true)
        )
    }
}

function New-AttachmentUploadFormBody {
    param(
        [string]$Category = 'general',
        [string]$LogoVariant = '',
        [string]$Description = 'Sample attachment upload'
    )

    $form = @(
        @{ key = 'file'; type = 'file'; src = @() }
        @{ key = 'entityType'; value = 'Company'; type = 'text' }
        @{ key = 'entityId'; value = '{{companyId}}'; type = 'text' }
        @{ key = 'category'; value = $Category; type = 'text' }
        @{ key = 'description'; value = $Description; type = 'text' }
        @{ key = 'tags'; value = 'postman'; type = 'text' }
        @{ key = 'isVisibleToCustomer'; value = 'true'; type = 'text' }
        @{ key = 'isVisibleToFieldTechnician'; value = 'true'; type = 'text' }
    )
    if (-not [string]::IsNullOrWhiteSpace($LogoVariant)) {
        $form += @{ key = 'logoVariant'; value = $LogoVariant; type = 'text' }
    }
    return $form
}

function Set-AttachmentUploadRequestBody {
    param(
        $RequestItem,
        [string]$Category = 'general',
        [string]$LogoVariant = '',
        [string]$Description
    )

    $RequestItem.request.header = @(
        @{ key = 'Accept'; value = 'application/json'; type = 'text' }
        @{ key = 'X-Tenant-Id'; value = '{{tenantId}}'; type = 'text' }
        @{ key = 'X-Company-Id'; value = '{{companyId}}'; type = 'text' }
    )
    $RequestItem.request.body = @{
        mode = 'formdata'
        formdata = New-AttachmentUploadFormBody -Category $Category -LogoVariant $LogoVariant -Description $Description
    }
}

function Get-PricingMatrixCreateBody {
    param([ValidateSet('FlatLabor', 'FlatLaborBySkill', 'LaborTiers', 'MaterialTiers', 'OtherItems')][string]$Variant)

    switch ($Variant) {
        'FlatLabor' {
            return @'
{
  "name": "FLATLABOR",
  "description": "Flat Labor Pricing",
  "isDefault": false,
  "isLaborTierStructure": false,
  "isLaborRateBySkillLevel": false,
  "priceAdjustmentTypeId": null,
  "effectiveFrom": null,
  "effectiveTo": null,
  "isMobileVisible": true
}
'@
        }
        'FlatLaborBySkill' {
            return @'
{
  "name": "FLATSKILL",
  "description": "Flat Labor by Skill Level",
  "isDefault": false,
  "isLaborTierStructure": false,
  "isLaborRateBySkillLevel": true,
  "priceAdjustmentTypeId": null,
  "effectiveFrom": null,
  "effectiveTo": null,
  "isMobileVisible": true
}
'@
        }
        'LaborTiers' {
            return @'
{
  "name": "LABORTIER",
  "description": "Tiered Labor Pricing",
  "isDefault": false,
  "isLaborTierStructure": true,
  "isLaborRateBySkillLevel": false,
  "priceAdjustmentTypeId": null,
  "effectiveFrom": null,
  "effectiveTo": null,
  "isMobileVisible": true
}
'@
        }
        'MaterialTiers' {
            return @'
{
  "name": "MATERIAL",
  "description": "Material Cost-Range Markup",
  "isDefault": false,
  "isLaborTierStructure": false,
  "isLaborRateBySkillLevel": false,
  "priceAdjustmentTypeId": 1,
  "effectiveFrom": null,
  "effectiveTo": null,
  "isMobileVisible": true
}
'@
        }
        'OtherItems' {
            return @'
{
  "name": "OTHER",
  "description": "Other Category Markup",
  "isDefault": false,
  "isLaborTierStructure": false,
  "isLaborRateBySkillLevel": false,
  "priceAdjustmentTypeId": 1,
  "effectiveFrom": null,
  "effectiveTo": null,
  "isMobileVisible": true
}
'@
        }
    }
}

function Expand-CreateRequestScenarios {
    param(
        $Folder,
        [string]$CreateItemName = 'Create',
        [array]$Scenarios
    )

    if ($null -eq $Scenarios -or $Scenarios.Count -eq 0) { return $Folder }

    $createTemplate = $null
    $createEvent = $null
    $newItems = @()
    foreach ($item in $Folder.item) {
        if ($item.name -eq $CreateItemName) {
            $createTemplate = $item
            $createEvent = $item.event
            continue
        }
        $newItems += $item
    }

    if ($null -eq $createTemplate) {
        $Folder.item = $newItems
        return $Folder
    }

    $createRequests = @()
    foreach ($scenario in $Scenarios) {
        $clone = @{
            name = $scenario.Name
            request = ($createTemplate.request | ConvertTo-Json -Depth 30 | ConvertFrom-Json)
            event = $createEvent
        }
        if ($scenario.Description) {
            $clone.request.description = $scenario.Description
        }
        if ($scenario.ContainsKey('Body') -and $null -ne $scenario.Body) {
            if (-not $clone.request.body) {
                $clone.request.body = @{ mode = 'raw'; raw = ''; options = @{ raw = @{ language = 'json' } } }
            }
            $clone.request.body.mode = 'raw'
            $clone.request.body.raw = $scenario.Body
            $clone.request.body.options = @{ raw = @{ language = 'json' } }
            $hasContentType = $false
            foreach ($h in @($clone.request.header)) {
                if ($h.key -eq 'Content-Type') { $hasContentType = $true; break }
            }
            if (-not $hasContentType) {
                $clone.request.header += @{ key = 'Content-Type'; value = 'application/json'; type = 'text' }
            }
        }
        if ($scenario.ContainsKey('Query') -and $null -ne $scenario.Query) {
            $clone.request.url.query = @($scenario.Query)
            $baseRaw = ($clone.request.url.raw -split '\?')[0]
            $qParts = @()
            foreach ($q in $scenario.Query) {
                if ($q.disabled) { continue }
                $qParts += ('{0}={1}' -f $q.key, $q.value)
            }
            if ($qParts.Count -gt 0) {
                $clone.request.url.raw = "$baseRaw?" + ($qParts -join '&')
            }
            else {
                $clone.request.url.raw = $baseRaw
            }
        }
        $createRequests += $clone
    }

    $ordered = @()
    $inserted = $false
    foreach ($item in $newItems) {
        if (-not $inserted -and $item.name -in @('Update', 'Patch', 'Delete')) {
            $ordered += $createRequests
            $inserted = $true
        }
        $ordered += $item
    }
    if (-not $inserted) {
        $ordered += $createRequests
    }

    $Folder.item = $ordered
    return $Folder
}

function Add-PricingMatrixEnhancementsToFolder {
    param($Folder)

    if ($Folder.name -ne 'PricingMatrixController') { return $Folder }

    $Folder.description = 'Pricing matrix header CRUD via {{gatewayUrl}}/api/v1/pricingmatrix. Children use separate controllers: pricingmatrixlabor, pricingmatrixlabortier, pricingmatrixmaterialtier, pricingmatrixother.'

    $variants = @(
        @{ Name = 'Create - Flat Labor Header'; Variant = 'FlatLabor'; Description = 'Header only (isLaborTierStructure=false). Add labor lines via PricingMatrixLaborController.' }
        @{ Name = 'Create - Flat Labor By Skill Header'; Variant = 'FlatLaborBySkill'; Description = 'Header only (isLaborRateBySkillLevel=true). Add skill-level labor lines via PricingMatrixLaborController.' }
        @{ Name = 'Create - Labor Tiers Header'; Variant = 'LaborTiers'; Description = 'Header only (isLaborTierStructure=true). Add labor lines then tiers via PricingMatrixLaborController / PricingMatrixLaborTierController.' }
        @{ Name = 'Create - Material Markup Header'; Variant = 'MaterialTiers'; Description = 'Header only with priceAdjustmentTypeId. Add tiers via PricingMatrixMaterialTierController (mutually exclusive with Other).' }
        @{ Name = 'Create - Other Markup Header'; Variant = 'OtherItems'; Description = 'Header only with priceAdjustmentTypeId. Add items via PricingMatrixOtherController (mutually exclusive with MaterialTiers).' }
    )

    $scenarios = @()
    foreach ($v in $variants) {
        $scenarios += @{
            Name = $v.Name
            Description = $v.Description
            Body = (Get-PricingMatrixCreateBody -Variant $v.Variant)
        }
    }

    return Expand-CreateRequestScenarios -Folder $Folder -Scenarios $scenarios
}

function Get-InventoryLocationCreateBody {
    param([string]$Type)

    $code = switch ($Type) {
        'WAREHOUSE' { 'WH-MAIN' }
        'TRUCK' { 'TRK-01' }
        'TRAILER' { 'TRL-01' }
        'JOBSITE' { 'JOB-01' }
        'CONSIGNMENT' { 'CON-01' }
        'VENDOR' { 'VND-LOC' }
        default { 'LOC-01' }
    }
    $name = switch ($Type) {
        'WAREHOUSE' { 'Main Warehouse' }
        'TRUCK' { 'Service Truck 01' }
        'TRAILER' { 'Parts Trailer 01' }
        'JOBSITE' { 'Jobsite Staging' }
        'CONSIGNMENT' { 'Vendor Consignment' }
        'VENDOR' { 'Vendor Stock Location' }
        default { 'Inventory Location' }
    }

    return @"
{
  "inventoryLocationCode": "$code",
  "name": "$name",
  "inventoryLocationType": "$Type",
  "parentInventoryLocationId": null,
  "description": "$name inventory location",
  "address1": "100 Main St",
  "address2": null,
  "city": "Austin",
  "stateProvince": "TX",
  "postalCode": "78701",
  "country": "US",
  "contactName": "Warehouse Desk",
  "phoneNumber": "+15551234567",
  "email": "warehouse@example.com",
  "isDefault": false
}
"@
}

function Get-VendorCreateBody {
    param([string]$VendorType, [string]$VendorStatus = 'ACTIVE')

    $code = if ($VendorType -eq 'SUBCONTRACTOR') { 'SUB01' } else { 'VND01' }
    $name = if ($VendorType -eq 'SUBCONTRACTOR') { 'Acme Subcontractor' } else { 'Acme Supply Co' }

    return @"
{
  "vendorCode": "$code",
  "name": "$name",
  "legalName": "$name LLC",
  "vendorType": "$VendorType",
  "vendorStatus": "$VendorStatus",
  "vendorAccountNumber": "ACCT-1001",
  "paymentTermId": null,
  "contactName": "Purchasing Desk",
  "contactTitle": "Buyer",
  "email": "purchasing@example.com",
  "purchaseOrderEmail": "po@example.com",
  "phoneNumber": "+15551234567",
  "mobileNumber": "+15557654321",
  "faxNumber": null,
  "website": "https://vendor.example.com",
  "address1": "200 Supply Rd",
  "address2": null,
  "city": "Austin",
  "stateProvince": "TX",
  "postalCode": "78702",
  "country": "US",
  "taxIdNumber": "12-3456789",
  "licenseNumber": null,
  "insurancePolicyNumber": null,
  "notes": "$name vendor record",
  "is1099Eligible": false
}
"@
}

function Add-InventoryEnhancementsToFolder {
    param($Folder)

    if ($Folder.name -eq 'InventoryLocationController') {
        $Folder.description = 'Inventory locations via {{gatewayUrl}}/api/v1/inventorylocation. Create scenarios cover each valid inventoryLocationType.'
        $types = @('WAREHOUSE', 'TRUCK', 'TRAILER', 'JOBSITE', 'CONSIGNMENT', 'VENDOR')
        $scenarios = @()
        foreach ($t in $types) {
            $scenarios += @{
                Name = "Create - $t"
                Description = "Create inventory location with inventoryLocationType=$t (code must be uppercase)."
                Body = (Get-InventoryLocationCreateBody -Type $t)
            }
        }
        return Expand-CreateRequestScenarios -Folder $Folder -Scenarios $scenarios
    }

    if ($Folder.name -eq 'VendorController') {
        $Folder.description = 'Vendors via {{gatewayUrl}}/api/v1/vendor. Create scenarios cover vendorType and vendorStatus combinations.'
        $scenarios = @(
            @{ Name = 'Create - Vendor Active'; Description = 'vendorType=VENDOR, vendorStatus=ACTIVE'; Body = (Get-VendorCreateBody -VendorType 'VENDOR' -VendorStatus 'ACTIVE') }
            @{ Name = 'Create - Vendor On Hold'; Description = 'vendorType=VENDOR, vendorStatus=ON_HOLD'; Body = (Get-VendorCreateBody -VendorType 'VENDOR' -VendorStatus 'ON_HOLD') }
            @{ Name = 'Create - Subcontractor Active'; Description = 'vendorType=SUBCONTRACTOR, vendorStatus=ACTIVE'; Body = (Get-VendorCreateBody -VendorType 'SUBCONTRACTOR' -VendorStatus 'ACTIVE') }
            @{ Name = 'Create - Subcontractor Inactive'; Description = 'vendorType=SUBCONTRACTOR, vendorStatus=INACTIVE'; Body = (Get-VendorCreateBody -VendorType 'SUBCONTRACTOR' -VendorStatus 'INACTIVE') }
        )
        return Expand-CreateRequestScenarios -Folder $Folder -Scenarios $scenarios
    }

    return $Folder
}

function Get-VehicleCreateBody {
    param([string]$OwnershipType)

    $company = if ($OwnershipType -eq 'Owned') { $null } else { '"Fleet Lease Partners"' }
    $companyJson = if ($null -eq $company) { 'null' } else { $company }

    return @"
{
  "inventoryLocationId": {{inventoryLocationId}},
  "ownershipType": "$OwnershipType",
  "ownershipCompany": $companyJson,
  "year": 2024,
  "make": "Ford",
  "model": "Transit",
  "color": "White",
  "vin": "1FTBW2CM5PKA12345",
  "licensePlate": "FGS-100",
  "licensePlateState": "TX",
  "purchaseDate": "2024-03-15",
  "purchasePrice": 42000.00,
  "purchasedFrom": "Austin Ford",
  "isPurchasedNew": true,
  "notes": "$OwnershipType service vehicle"
}
"@
}

function Get-JobTypeCreateBody {
    param([int]$UsedFor, [string]$Label)

    $code = switch ($UsedFor) {
        1 { 'SVC' }
        2 { 'MNT' }
        3 { 'WAR' }
        4 { 'INS' }
        default { 'JOB' }
    }

    return @"
{
  "jobTypeCode": "$code",
  "name": "$Label",
  "usedFor": $UsedFor,
  "businessUnit": "Field Services",
  "backgroundColor": "#3366FF",
  "textColor": "#FFFFFF",
  "showToFieldTech": true,
  "showOnCustomerPortal": true,
  "displayOrder": $UsedFor
}
"@
}

function Get-CommunicationTemplateCreateBody {
    param([string]$Channel)

    $code = switch ($Channel) {
        'Email' { 'INVITE' }
        'SMS' { 'INVITE_SMS' }
        'PushNotification' { 'JOB_ALERT' }
        'SystemNotification' { 'SYSTEM_INFO' }
        default { 'TMPL' }
    }
    $subject = if ($Channel -eq 'Email') { '"You are invited to {{CompanyName}}"' } else { 'null' }
    $body = switch ($Channel) {
        'Email' { '"Hello, use {{InviteUrl}} to join {{CompanyName}}."' }
        'SMS' { '"{{CompanyName}} invite: {{InviteUrl}}"' }
        'PushNotification' { '"New job assigned for {{CompanyName}}."' }
        default { '"System notice for {{CompanyName}}."' }
    }

    return @"
{
  "communicationChannel": "$Channel",
  "templateType": "Transactional",
  "code": "$code",
  "name": "$Channel $code template",
  "subject": $subject,
  "body": $body,
  "isMobileVisible": true
}
"@
}

function Get-CredentialCreateBody {
    param([ValidateSet('Global', 'Tenant')][string]$Scope)

    if ($Scope -eq 'Global') {
        return @'
{
  "scope": 1,
  "providerCode": "DATABASE",
  "credentialName": "PlatformDatabaseConnections",
  "payload": "{\"FgsUser\":\"Host=localhost;Port=5432;Database=fgs_dev_db;Username=postgres;Password=secret\"}",
  "description": "Platform database connection strings (Global)",
  "tenantId": null,
  "companyId": null
}
'@
    }

    return @'
{
  "scope": 2,
  "providerCode": "DATABASE",
  "credentialName": "TenantDatabaseConnections",
  "payload": "{\"FgsTenant\":\"Host=localhost;Port=5432;Database=fgs_tenant_db;Username=postgres;Password=secret\"}",
  "description": "Tenant-scoped database connections",
  "tenantId": {{tenantId}},
  "companyId": {{companyId}}
}
'@
}

function Get-AssetAttributeCreateBody {
    param([string]$InputType)

    $code = ($InputType.Substring(0, [Math]::Min(8, $InputType.Length))).ToUpperInvariant()
    $text = 'null'
    $integer = 'null'
    $decimal = 'null'
    $date = 'null'
    $boolean = 'null'
    switch ($InputType) {
        'TEXT' { $text = '"Serial number"' }
        'TEXTAREA' { $text = '"Installed on rooftop unit"' }
        'INTEGER' { $integer = '10' }
        'DECIMAL' { $decimal = '12.50' }
        'DATE' { $date = '"2026-06-21"' }
        'BOOLEAN' { $boolean = 'true' }
        'DROPDOWN' { $text = 'null' }
    }

    return @"
{
  "assetTypeId": {{assetTypeId}},
  "attributeCode": "ATTR_$code",
  "attributeName": "$InputType Attribute",
  "inputType": "$InputType",
  "defaultOptionId": null,
  "defaultValueText": $text,
  "defaultValueInteger": $integer,
  "defaultValueDecimal": $decimal,
  "defaultValueDate": $date,
  "defaultValueBoolean": $boolean,
  "isRequired": false,
  "isSearchable": true,
  "displayOrder": 1
}
"@
}

function Add-SetupScenarioEnhancementsToFolder {
    param($Folder)

    switch ($Folder.name) {
        'PricingMatrixController' {
            return Add-PricingMatrixEnhancementsToFolder -Folder $Folder
        }
        'CredentialController' {
            $Folder.description = 'Credential admin via {{gatewayUrl}}/api/v1/credential. Create covers Global vs Tenant scope; Rotate covers Full vs KmsReEncrypt.'
            $Folder = Expand-CreateRequestScenarios -Folder $Folder -Scenarios @(
                @{ Name = 'Create - Global Scope'; Description = 'scope=1 (Global). tenantId/companyId must be null.'; Body = (Get-CredentialCreateBody -Scope 'Global') }
                @{ Name = 'Create - Tenant Scope'; Description = 'scope=2 (Tenant). Requires tenantId + companyId.'; Body = (Get-CredentialCreateBody -Scope 'Tenant') }
            )
            $rotateScenarios = @(
                @{ Name = 'Rotate - Full'; Description = 'rotationMode=1 (Full). Query scope=Global.'; Body = '{ "rotationMode": 1 }'; Query = @(@{ key = 'scope'; value = 'Global'; description = 'CredentialScope: Global or Tenant' }) }
                @{ Name = 'Rotate - Kms ReEncrypt'; Description = 'rotationMode=2 (KmsReEncrypt). Query scope=Global.'; Body = '{ "rotationMode": 2 }'; Query = @(@{ key = 'scope'; value = 'Global'; description = 'CredentialScope: Global or Tenant' }) }
            )
            return Expand-CreateRequestScenarios -Folder $Folder -CreateItemName 'Rotate' -Scenarios $rotateScenarios
        }
        'VehicleController' {
            $Folder.description = 'Vehicles via {{gatewayUrl}}/api/v1/vehicle. Create scenarios for Owned, Leased, and Rented. Set inventoryLocationId first (from InventoryLocation Create).'
            return Expand-CreateRequestScenarios -Folder $Folder -Scenarios @(
                @{ Name = 'Create - Owned'; Description = 'ownershipType=Owned'; Body = (Get-VehicleCreateBody -OwnershipType 'Owned') }
                @{ Name = 'Create - Leased'; Description = 'ownershipType=Leased'; Body = (Get-VehicleCreateBody -OwnershipType 'Leased') }
                @{ Name = 'Create - Rented'; Description = 'ownershipType=Rented'; Body = (Get-VehicleCreateBody -OwnershipType 'Rented') }
            )
        }
        'JobTypeController' {
            $Folder.description = 'Job types via {{gatewayUrl}}/api/v1/jobtype. Create scenarios for UsedFor 1=Service, 2=Maintenance, 3=Warranty, 4=Installation.'
            return Expand-CreateRequestScenarios -Folder $Folder -Scenarios @(
                @{ Name = 'Create - Service'; Description = 'usedFor=1 (Service)'; Body = (Get-JobTypeCreateBody -UsedFor 1 -Label 'Service Call') }
                @{ Name = 'Create - Maintenance'; Description = 'usedFor=2 (Maintenance)'; Body = (Get-JobTypeCreateBody -UsedFor 2 -Label 'Preventive Maintenance') }
                @{ Name = 'Create - Warranty'; Description = 'usedFor=3 (Warranty)'; Body = (Get-JobTypeCreateBody -UsedFor 3 -Label 'Warranty Repair') }
                @{ Name = 'Create - Installation'; Description = 'usedFor=4 (Installation)'; Body = (Get-JobTypeCreateBody -UsedFor 4 -Label 'New Installation') }
            )
        }
        'CommunicationTemplateController' {
            $Folder.description = 'Communication templates via {{gatewayUrl}}/api/v1/communicationtemplate. Create scenarios per communicationChannel.'
            return Expand-CreateRequestScenarios -Folder $Folder -Scenarios @(
                @{ Name = 'Create - Email'; Description = 'communicationChannel=Email'; Body = (Get-CommunicationTemplateCreateBody -Channel 'Email') }
                @{ Name = 'Create - SMS'; Description = 'communicationChannel=SMS'; Body = (Get-CommunicationTemplateCreateBody -Channel 'SMS') }
                @{ Name = 'Create - Push Notification'; Description = 'communicationChannel=PushNotification'; Body = (Get-CommunicationTemplateCreateBody -Channel 'PushNotification') }
                @{ Name = 'Create - System Notification'; Description = 'communicationChannel=SystemNotification'; Body = (Get-CommunicationTemplateCreateBody -Channel 'SystemNotification') }
            )
        }
        'BillingCategoryController' {
            $Folder.description = 'Billing categories via {{gatewayUrl}}/api/v1/billingcategory. Create scenarios for common 2-char billingCategoryType values.'
            $scenarios = @()
            foreach ($pair in @(
                @{ Type = 'NI'; Name = 'Non Inventory' }
                @{ Type = 'OT'; Name = 'Other Charges' }
                @{ Type = 'SF'; Name = 'Service Fee' }
                @{ Type = 'LB'; Name = 'Labor' }
                @{ Type = 'TX'; Name = 'Tax' }
            )) {
                $scenarios += @{
                    Name = "Create - $($pair.Type)"
                    Description = "billingCategoryType=$($pair.Type) (2-char uppercase)"
                    Body = @"
{
  "billingCategoryType": "$($pair.Type)",
  "billingCategoryName": "$($pair.Name)",
  "description": "$($pair.Name) billing category",
  "displayOrder": 1,
  "isSystemDefined": false,
  "showToFieldTech": true,
  "allowToPick": true
}
"@
                }
            }
            return Expand-CreateRequestScenarios -Folder $Folder -Scenarios $scenarios
        }
        'SalesPipelineStatusController' {
            $Folder.description = 'Sales pipeline statuses. Create scenarios for lead-only vs opportunity-only applicability.'
            return Expand-CreateRequestScenarios -Folder $Folder -Scenarios @(
                @{
                    Name = 'Create - Lead Pipeline'
                    Description = 'appliesToLead=true, appliesToOpportunity=false'
                    Body = @'
{
  "statusCode": "LEAD_NEW",
  "statusName": "New Lead",
  "description": "Newly captured lead",
  "displayOrder": 1,
  "isSystem": false,
  "appliesToLead": true,
  "appliesToOpportunity": false,
  "isTerminal": false,
  "allowManualSelection": true
}
'@
                }
                @{
                    Name = 'Create - Opportunity Pipeline'
                    Description = 'appliesToLead=false, appliesToOpportunity=true'
                    Body = @'
{
  "statusCode": "OPP_QUAL",
  "statusName": "Qualified Opportunity",
  "description": "Qualified sales opportunity",
  "displayOrder": 1,
  "isSystem": false,
  "appliesToLead": false,
  "appliesToOpportunity": true,
  "isTerminal": false,
  "allowManualSelection": true
}
'@
                }
            )
        }
        default { return $Folder }
    }
}

function Add-AssetScenarioEnhancementsToFolder {
    param($Folder)

    if ($Folder.name -ne 'AssetAttributeController') { return $Folder }

    $Folder.description = 'Asset attributes via {{gatewayUrl}}/api/v1/assetattribute. Create scenarios cover each valid inputType. Set assetTypeId first (from AssetType Create). For DROPDOWN, create options next.'
    $types = @('TEXT', 'TEXTAREA', 'INTEGER', 'DECIMAL', 'DATE', 'BOOLEAN', 'DROPDOWN')
    $scenarios = @()
    foreach ($t in $types) {
        $scenarios += @{
            Name = "Create - $t"
            Description = "inputType=$t"
            Body = (Get-AssetAttributeCreateBody -InputType $t)
        }
    }
    return Expand-CreateRequestScenarios -Folder $Folder -Scenarios $scenarios
}

function Add-NotificationScenarioEnhancementsToFolder {
    param($Folder)

    if ($Folder.name -ne 'NotificationController') { return $Folder }

    $Folder.description = 'Notification dispatch via {{gatewayUrl}}/api/v1/notification/dispatch. Separate requests per channel scenario.'
    $scenarios = @(
        @{
            Name = 'Dispatch - Email Invite'
            Description = 'channel=Email, templateCode=INVITE'
            Body = @'
{
  "tenantId": {{tenantId}},
  "companyId": {{companyId}},
  "channel": "Email",
  "templateCode": "INVITE",
  "recipient": "{{entraUserEmail}}",
  "correlationId": "postman-email-001",
  "tokens": {
    "CompanyName": "Plumbing Ltd",
    "InviteUrl": "https://developer.fsm.com/api/v1/invite/start?token={{inviteToken}}"
  }
}
'@
        }
        @{
            Name = 'Dispatch - SMS Invite'
            Description = 'channel=SMS, templateCode=INVITE_SMS'
            Body = @'
{
  "tenantId": {{tenantId}},
  "companyId": {{companyId}},
  "channel": "SMS",
  "templateCode": "INVITE_SMS",
  "recipient": "+15551234567",
  "correlationId": "postman-sms-001",
  "tokens": {
    "CompanyName": "Plumbing Ltd",
    "InviteUrl": "https://developer.fsm.com/api/v1/invite/start?token={{inviteToken}}"
  }
}
'@
        }
    )
    return Expand-CreateRequestScenarios -Folder $Folder -CreateItemName 'Dispatch' -Scenarios $scenarios
}

function Add-AttachmentEnhancementsToFolder {
    param($Folder)

    if ($Folder.name -ne 'AttachmentController') { return $Folder }

    $Folder.description = 'Attachment management via {{gatewayUrl}}/api/v1/attachment. Upload scenarios: general file + each logoVariant.'

    $newItems = @()
    foreach ($item in $Folder.item) {
        if ($item.name -eq 'Upload') {
            $item.name = 'Upload - General'
            $item.request.description = 'Upload a general file via multipart/form-data. Select a local file for the file field.'
            Set-AttachmentUploadRequestBody -RequestItem $item -Category 'general' -Description 'General attachment upload'
            $newItems += $item
            foreach ($variant in @('full', 'compact', 'icon', 'favicon')) {
                $logoUpload = @{
                    name = "Upload - Company Logo ($variant)"
                    request = ($item.request | ConvertTo-Json -Depth 30 | ConvertFrom-Json)
                    event = $item.event
                }
                $logoUpload.request.description = "Upload company logo (category=logo, logoVariant=$variant)."
                Set-AttachmentUploadRequestBody -RequestItem $logoUpload -Category 'logo' -LogoVariant $variant -Description "Company logo ($variant)"
                $newItems += $logoUpload
            }
            continue
        }
        if ($item.name -eq 'Download') {
            $item.request.description = 'Stream/download attachment bytes (supports Range requests for video/audio).'
            $item.request.header = @(
                @{ key = 'Accept'; value = '*/*'; type = 'text' }
                @{ key = 'X-Tenant-Id'; value = '{{tenantId}}'; type = 'text' }
                @{ key = 'X-Company-Id'; value = '{{companyId}}'; type = 'text' }
            )
        }
        if ($item.name -eq 'Thumbnail') {
            $item.request.description = 'Stream attachment thumbnail/preview image.'
            $item.request.header = @(
                @{ key = 'Accept'; value = '*/*'; type = 'text' }
                @{ key = 'X-Tenant-Id'; value = '{{tenantId}}'; type = 'text' }
                @{ key = 'X-Company-Id'; value = '{{companyId}}'; type = 'text' }
            )
        }
        $newItems += $item
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
        $items += New-UiLoginFlowFolder
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
    @{ Key = 'UserService'; Path = 'src\UserService\Fgs.User.API\Controllers'; Desc = 'Company onboarding, UI login, Entra auth, active-user cache, dashboard, and tenant admin APIs via {{gatewayUrl}}.'; AuthFlow = $true }
    @{ Key = 'BffService'; Path = 'src\BffService\Fgs.Bff.API\Controllers'; Desc = 'BFF orchestration (cross-domain workflows) via {{gatewayUrl}}/api/v1/bff/...'; AuthFlow = $false; SkipGenerate = $true }
    @{ Key = 'SetupService'; Path = 'src\SetupService\Fgs.Setup.API\Controllers'; Desc = 'Platform setup catalog APIs via {{gatewayUrl}}/api/v1/{catalog}. Multi-scenario Create requests for pricing matrix, credentials, vehicles, job types, communication templates, and billing categories.'; AuthFlow = $false }
    @{ Key = 'NotificationService'; Path = 'src\NotificationService\Fgs.Notification.API\Controllers'; Desc = 'Notification dispatch via {{gatewayUrl}}/api/v1/notification/...'; AuthFlow = $false }
    @{ Key = 'FileService'; Path = 'src\FileService\Fgs.File.API\Controllers'; Desc = 'Tenant S3 provisioning and attachment management via {{gatewayUrl}}/api/v1/attachment and /api/v1/tenant/{id}/bucket. Upload scenarios include general + logo variants.'; AuthFlow = $false }
    @{ Key = 'AuditService'; Path = 'src\AuditService\Fgs.Audit.API\Controllers'; Desc = 'Credential audit trail via {{gatewayUrl}}/api/v1/credentialaudit.'; AuthFlow = $false }
    @{ Key = 'AssetService'; Path = 'src\AssetService\Fgs.Asset.API\Controllers'; Desc = 'Asset catalog APIs via {{gatewayUrl}}/api/v1/{asset*}. Attribute Create scenarios cover each inputType.'; AuthFlow = $false }
    @{ Key = 'InventoryService'; Path = 'src\InventoryService\Fgs.Inventory.API\Controllers'; Desc = 'Inventory catalog and purchasing APIs via {{gatewayUrl}}/api/v1/{inventory-*|vendor|vendorinventoryitem|purchaseorder|truckstocktemplate}. Aggregates: inventoryitem (alternates/dependencies), purchaseorder (details), truckstocktemplate (items).'; AuthFlow = $false }
)

New-Item -ItemType Directory -Force -Path $OutputDir | Out-Null

Write-Host "Building DTO registry from Application layer..."
$script:DtoRegistry = Build-DtoRegistry -Root $RepoRoot
Write-Host "  Loaded $($script:DtoRegistry.Count) DTO types"

foreach ($svc in $serviceConfigs) {
    if ($svc.SkipGenerate) {
        Write-Host "Skip $($svc.Key): curated collection (docs/api/$($svc.Key).postman_collection.json)"
        continue
    }

    $controllerRoot = Join-Path $RepoRoot $svc.Path
    if (-not (Test-Path $controllerRoot)) { Write-Warning "Skip $($svc.Key): $controllerRoot"; continue }

    $files = Get-ChildItem -Path $controllerRoot -Filter '*Controller.cs' -Recurse |
        Where-Object { $_.Name -notmatch '^AuthController\..+\.cs$' -and $_.Name -ne 'HealthController.cs' } |
        Sort-Object FullName
    $folders = @()

    foreach ($file in $files) {
        $folder = Parse-ControllerFile -FilePath $file.FullName -ServiceKey $svc.Key -DtoRegistry $script:DtoRegistry
        if ($null -ne $folder) {
            if ($svc.Key -eq 'FileService') {
                $folder = Add-AttachmentEnhancementsToFolder -Folder $folder
            }
            if ($svc.Key -eq 'SetupService') {
                $folder = Add-SetupScenarioEnhancementsToFolder -Folder $folder
            }
            if ($svc.Key -eq 'InventoryService') {
                $folder = Add-InventoryEnhancementsToFolder -Folder $folder
            }
            if ($svc.Key -eq 'AssetService') {
                $folder = Add-AssetScenarioEnhancementsToFolder -Folder $folder
            }
            if ($svc.Key -eq 'NotificationService') {
                $folder = Add-NotificationScenarioEnhancementsToFolder -Folder $folder
            }
            $folders += $folder
        }
    }

    $outFile = Join-Path $OutputDir "$($svc.Key).postman_collection.json"
    if ($folders.Count -eq 0) {
        if (Test-Path $outFile) {
            Remove-Item -Path $outFile -Force
            Write-Host "Removed empty collection $outFile"
        }
        else {
            Write-Host "Skip $($svc.Key): no non-health controllers"
        }
        continue
    }

    $includeAuth = ($svc.Key -eq 'UserService')
    $collection = New-Collection -ServiceName $svc.Key -Description $svc.Desc -Folders $folders -IncludeAuthFlow:$includeAuth
    $collection | ConvertTo-Json -Depth 100 | Set-Content -Path $outFile -Encoding UTF8
    Write-Host "Generated $outFile ($($folders.Count) controllers)"
}

# Remove stale health-only / empty collections no longer generated.
$staleCollections = @(
    'PublisherService'
    'ConsumerService'
    'BillingService'
    'CommunicationService'
    'CrmService'
    'IntegrationService'
    'ReportingService'
    'SchedulingService'
    'ServiceAgreementService'
)
foreach ($name in $staleCollections) {
    $stalePath = Join-Path $OutputDir "$name.postman_collection.json"
    if (Test-Path $stalePath) {
        Remove-Item -Path $stalePath -Force
        Write-Host "Removed stale collection $stalePath"
    }
}

Write-Host "Done. Import FGS-Globals.postman_environment.json and select it in Postman."
